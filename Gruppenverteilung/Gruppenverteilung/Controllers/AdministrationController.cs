using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gruppenverteilung.Code;
using Gruppenverteilung.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Gruppenverteilung.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            AdministrationLoginModel model = new AdministrationLoginModel();
            return View("AdministrationLogin", model);
        }
        public IActionResult AdministrationView(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            return View(model);
        }

        public IActionResult AdministrationImportView()
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            return View();
        }

        public IActionResult AdministrationEditView(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            model.SelectedGroup = model.groups[0];
            return View(model);
        }

        public IActionResult ShowGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            return View(model);
        }

        public IActionResult AddTutor(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }

            try
            {
                model.SelectedGroup.AddTutor(model.SelectedTutor);
                model.SelectedTutor.HasGroup = true;
                model.AddTutorMessage = String.Format("Tutor {0} wurde erfolgreich der Gruppe: {1} zugewiesen", model.SelectedTutor.Name, model.SelectedGroup.Name);
                model.AddIsSuccessful = true;

                model.RefreshTutors();
                
            }
            catch
            {
                model.AddTutorMessage = String.Format("Tutor {0} konnte nicht der Gruppe: {1} zugewiesen werden!", model.SelectedTutor.Name, model.SelectedGroup.Name);
                model.AddIsSuccessful = false;
            }

            return View("../Administration/AdministrationEditView", model);
        }
        public IActionResult AddRoom(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }

            model.SelectedGroup.Room = model.SelectedGroupRoom;

            try
            {
                model.AddTutorMessage = String.Format("Raum {0} wurde erfolgreich der Gruppe: {1} zugewiesen", model.SelectedGroup.Room, model.SelectedGroup.Name);
                model.AddIsSuccessful = true;
            }
            catch
            {
                model.AddTutorMessage = String.Format("Raum {0} konnte nicht der Gruppe: {1} zugewiesen werden!", model.SelectedGroup.Room, model.SelectedGroup.Name);
                model.AddIsSuccessful = false;
            }

            return View("../Administration/AdministrationEditView", model);
        }

        public IActionResult RefreshGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            model.groups = GlobalVariables.sorter.Groups;
            return View("../Administration/AdministrationView", model);
        }

        public IActionResult SimulateGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            GlobalVariables.sorter.SimulateByFile("grpdata_01.txt");
            model.groups = GlobalVariables.sorter.Groups;

            return View("../Administration/AdministrationView", model);
        }

        [HttpPost]
        public IActionResult Login(AdministrationLoginModel model)
        {
            HttpContext.Session.SetString("Name", model.SessionInfo_Username);


            var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hashbytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(model.SessionInfo_Password));
            if (hashbytes.SequenceEqual(GlobalVariables.passwordbyte_hashed))
            {
                HttpContext.Session.SetString("LoggedIn", "true");
                return View("../Administration/AdministrationView", new AdministrationModel());
            }

            return View("../Administration/AdministrationLogin", model);
        }
    }
}