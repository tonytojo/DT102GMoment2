using DT102GMoment2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DT102GMoment2.Controllers
{
    public class CarController : Controller
    {
        [HttpGet] //Initiering to wash a car
        public IActionResult Index()
        {
            return View();
        }

        //Read json and list all washes on the screen
        [HttpGet] //Initiering to wash a car
        public IActionResult List()
        {
            string json = System.IO.File.ReadAllText("CarWash.json");
            List<CarWash> obj = JsonConvert.DeserializeObject<List<CarWash>>(json);
            return View(obj);
        }


        //Set a grade on how good the car wash was
        [HttpGet("/Betyg")]
        public IActionResult Bedomning()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Dåligt", Value = "1" });
            list.Add(new SelectListItem() { Text = "Bra", Value = "2" });
            list.Add(new SelectListItem() { Text = "Super", Value = "3" });
            return View(list);
        }

        [HttpGet]
        public IActionResult SmutsEstimat()
        {
            //Model
            CarWash wasch = new CarWash();

            //Load the list with different level of how dirty the car is
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = wasch.arrText[1], Value = "1" });
            list.Add(new SelectListItem() { Text = wasch.arrText[2], Value = "2" });
            list.Add(new SelectListItem() { Text = wasch.arrText[3], Value = "3" });
            Multiple mp = new Multiple()
            {
                Data = wasch,
                Dirty = list
            };

            //Who wash the car me or my sambo or both
            List<SelectListItem> whoWash = new List<SelectListItem>() 
            {
                new SelectListItem() { Text = "Tony", Value = "1" },
                new SelectListItem() { Text = "Sambo", Value = "2"}
            };
            wasch.WhoWash = whoWash;

            //Save model in Session
            string json = JsonConvert.SerializeObject(wasch);
            HttpContext.Session.SetString("SessionName", json);

            ViewBag.vatten = "Det är dåligt med varmvatten säger mack ägaren eftersom andra har slösat";
            return View(mp);
        }

        
        [HttpPost]
        public IActionResult Calculation(Multiple m)
        {
          string whowash="";

          //Find out who wash the car me, you or both
          if (m.Data.WhoWash[0].Selected) //Jag
            whowash = "Tony tvättar";

          if (m.Data.WhoWash[1].Selected) //Sambo
               whowash = whowash =="" ? "Sambo tvättar" : "Tony och sambo tvättar";

           //Get session and Deserialize
            var json = HttpContext.Session.GetString("SessionName");
            CarWash wasch = JsonConvert.DeserializeObject<CarWash>(json);

            ////Update object with value from input
            wasch.Info = whowash;
            wasch.Grad = m.Data.Grad;
            wasch.Text = wasch.arrText[m.Data.Grad];
            wasch.SizeOfBucket =  m.Data.SizeOfBucket; //hinkens storlek
            wasch.dlSchampo();
            wasch.Date = m.Data.Date;
            wasch.Name = m.Data.Name;
            wasch.Regnr = m.Data.Regnr.ToUpper();

            ////Serialisera and write back to session
            json = JsonConvert.SerializeObject(wasch);
            HttpContext.Session.SetString("SessionName", json);
             return View(wasch);
        }

        [HttpPost]
        public IActionResult Betyg(CarWash cw)
        {
            //Get Session and Deserialize
            var json = HttpContext.Session.GetString("SessionName");
            CarWash wasch = JsonConvert.DeserializeObject<CarWash>(json);

            //Find out what grade was choosen
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Dåligt", Value = "1" },
                new SelectListItem() { Text = "Bra", Value = "2" },
                new SelectListItem() { Text = "Super", Value = "3" }
            };
            wasch.Betyg = list[int.Parse(cw.Betyg)-1].Text;

            //Calculate wash time
            var washTime = wasch.timeUnit;
            var totaltime = washTime.Sum(x => x.Value);

            //Read json and Deserialize
            json = System.IO.File.ReadAllText("CarWash.json");
            List<DataOnlyCarWash> obj = JsonConvert.DeserializeObject<List<DataOnlyCarWash>>(json);

            //Add the new DataOnlyCarWash object to be used for json file
            obj.Add(new DataOnlyCarWash()
            {
                Name = wasch.Name,
                Text = wasch.Text,
                Betyg = wasch.Betyg,
                Regnr = wasch.Regnr,
                AmountSchampo = wasch.AmountSchampo,
                SizeOfBucket = wasch.SizeOfBucket,
                Date = wasch.Date
            });

            //Update json file with a new car wash
            using (StreamWriter file = System.IO.File.CreateText("CarWash.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, obj);
            }

            ViewData["washtime"] = washTime.Sum(x => x.Value);
            return View("SlutResultat", wasch);
        }
    }
}
