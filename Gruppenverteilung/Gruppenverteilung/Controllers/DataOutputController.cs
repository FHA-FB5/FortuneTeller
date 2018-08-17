using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gruppenverteilung.Models;
namespace Gruppenverteilung.Controllers
{
    public class DataOutputController : Controller
    {
        public IActionResult DataOutputView(DataOutputModel model)
        {
            return View(model);
        }
    }
}