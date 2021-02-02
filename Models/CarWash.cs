using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DT102GMoment2.Models
{
    public class CarWash
    {
        public IList<SelectListItem> WhoWash { get; set; }
        public string Text { get; set; }
        public string Info { get; set; }

        public int Grad { get; set; }

        [Required]
        public string Regnr { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Schampo(dl)")]
        public double AmountSchampo { get; set; }

        public string Betyg { get; set; }

        [Range(1, 50)]
        [Required]
        [Display(Name = "Hinkens storlek i liter")]
        public int SizeOfBucket { get; set; }
        public void dlSchampo()
        {
            AmountSchampo  = Math.Round(arrAmount[Grad] / 100 * SizeOfBucket, 1);
        }

        public double[] arrAmount = {0, 5, 10, 20 };
        public string[] arrText = {"dummy", "Lätt smutsig", "Medel smutsig", "Jätte skitig" };


        public Dictionary<string, int> timeUnit = new Dictionary<string, int>();

        public CarWash()
        {
             timeUnit.Add("Avspolning", 20);
             timeUnit.Add("Schamponering", 10);
             timeUnit.Add("Tvättning", 10);
             timeUnit.Add("Torka", 20);
             timeUnit.Add("Waxning", 20);
        }
    }
}
