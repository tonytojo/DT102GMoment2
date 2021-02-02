using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DT102GMoment2.Models
{
    public class Multiple
    {
        public CarWash Data { get; set; }
        public List<SelectListItem> Dirty { get; set; }
    }
}
