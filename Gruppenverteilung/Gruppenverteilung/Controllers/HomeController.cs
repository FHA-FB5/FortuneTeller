using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gruppenverteilung.Models;
using Gruppenverteilung.Code;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DataInputView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitData(DataInputModel model)
        {
            DataOutputModel dataOutputModel = new DataOutputModel();

            Group BestGroup = GlobalVariables.sorter.FindBestGroup(new Member(model.Name, model.Alter, model.Studiengang, model.Geschlecht));
            dataOutputModel.GruppenName = BestGroup.Name;

            ///Rückgabe der view mit passendem Model.
            return View("../DataOutput/DataOutputView", dataOutputModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
