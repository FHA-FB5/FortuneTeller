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
        public ActionResult SubmitData(DataInputModel model)
        {
            DataOutputModel dataOutputModel = new DataOutputModel();           

            //Testing
            Group g1 = new Group();
            g1.Name = "GRP1";
            g1.AddMember("grpdata_01.txt");
            Group g2 = new Group();
            g2.AddMember("grpdata_02.txt");
            g2.Name = "GRP2";
            Member member = new Member(model.Name, model.Alter, model.Studiengang, model.Geschlecht);
            double sg1 = g1.CalculateScore(member);
            double sg2 = g2.CalculateScore(member);
            if (sg1 < sg2)
            {
                g1.AddMember(member);
                dataOutputModel.GruppenName = g1.Name;
            }
            else
            {
                g2.AddMember(member);
                dataOutputModel.GruppenName = g2.Name;
            }
            //
            
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
