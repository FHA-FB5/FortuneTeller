using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gruppenverteilung.Code;
using Gruppenverteilung.Models;

namespace Gruppenverteilung.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult AdministrationView(AdministrationModel model)
        {
            return View(model);
        }

        public IActionResult ShowGroups(AdministrationModel model)
        {
            return View();
        }
        
        public IActionResult RefreshGroups(AdministrationModel model)
        {
            model.groups = GlobalVariables.sorter.groups;
            return View("../Administration/AdministrationView", model);
        }
    }
}