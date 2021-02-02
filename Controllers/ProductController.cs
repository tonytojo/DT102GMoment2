using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DT102GMoment2.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
