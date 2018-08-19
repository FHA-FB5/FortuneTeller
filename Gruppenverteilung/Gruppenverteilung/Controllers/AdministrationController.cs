using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gruppenverteilung.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult AdministrationView()
        {
            return View();
        }
    }
}