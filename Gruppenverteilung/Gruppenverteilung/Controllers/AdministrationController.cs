using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gruppenverteilung.Code;
using Gruppenverteilung.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using Newtonsoft.Json;

namespace Gruppenverteilung.Controllers
{
    public class AdministrationController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            { 
                GlobalVariables.CurrentSelectedGroupInTutorAssignView = GlobalVariables.sorter.Groups[0];
                GlobalVariables.CurrentSelectedGroupInEditGroupViewAssignView = GlobalVariables.sorter.Groups[0];
                //GlobalVariables.CurrentSelectedGroupInGroupEditView = GlobalVariables.sorter.Groups[0];
                GlobalVariables.ToAssignTutors_ForAssignView = GlobalVariables.sorter.Tutors;
                AdministrationLoginModel loginmodel = new AdministrationLoginModel();
                return View("AdminLogInView", loginmodel);
            }
            AdministrationModel model = new AdministrationModel();
            model.CurrentSelectedGroup = model.groups[0];
            return View(model);
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
            //return View(model);
            return View("../Administration/AdminTestView");
        }

        [HttpPost]
        public IActionResult ShowGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult RefreshGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            model.groups = GlobalVariables.sorter.Groups;

            return View("../Administration/Index", model);
        }

        [HttpPost]
        public IActionResult SimulateGroups(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }
            GlobalVariables.sorter.SimulateByFile("grpdata_01.txt");
            model.groups = GlobalVariables.sorter.Groups;
            return View("../Administration/Index", model);
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
                return RedirectToAction("Index");
                //return View("../Administration/Index", new AdministrationModel());
            }

            
            return View("../Administration/AdminLogInView", model);
        }

        [HttpPost]
        public PartialViewResult TutorUpdate(string groupename, AdministrationModel model)
        {
            GlobalVariables.CurrentSelectedGroupInTutorAssignView = GlobalVariables.sorter.Groups.FirstOrDefault(g => g.Name == groupename);            

            return PartialView("_AssignTutorListView", model);
        }

        [HttpPost]
        public PartialViewResult EditGroupUpdate(string groupename, AdministrationModel model)
        {
            GlobalVariables.CurrentSelectedGroupInGroupEditView = GlobalVariables.sorter.Groups.FirstOrDefault(g => g.Name == groupename);

            return PartialView("_EditGroupView", model);
        }

        [HttpPost]
        public PartialViewResult RemoveTutorFromGroup(string tutorname, AdministrationModel model)
        {  
            GlobalVariables.ToAssignTutors_ForAssignView.Add(GlobalVariables.CurrentSelectedGroupInTutorAssignView.TutorList.FirstOrDefault(t => t.Name == tutorname));
            GlobalVariables.CurrentSelectedGroupInTutorAssignView.TutorList.Remove(GlobalVariables.CurrentSelectedGroupInTutorAssignView.TutorList.FirstOrDefault(t => t.Name == tutorname));

            return PartialView("_AssignTutorListView", model);
        }

        [HttpPost]
        public PartialViewResult AddTutorToGroup(string tutorname, AdministrationModel model)
        {
            GlobalVariables.CurrentSelectedGroupInTutorAssignView.TutorList.Add(GlobalVariables.ToAssignTutors_ForAssignView.FirstOrDefault(t => t.Name == tutorname));
            GlobalVariables.ToAssignTutors_ForAssignView.Remove(GlobalVariables.CurrentSelectedGroupInTutorAssignView.TutorList.FirstOrDefault(t => t.Name == tutorname));

            return PartialView("_ToAssignTutorListView", model);
        }

        [HttpPost]
        public PartialViewResult UpdateAssignedTutorListView(AdministrationModel model)
        {
            return PartialView("_AssignTutorListView", model);
        }

        [HttpPost]
        public PartialViewResult UpdateToAssignTutorListView(AdministrationModel model)
        {
            return PartialView("_ToAssignTutorListView", model);
        }

        [HttpPost]
        public PartialViewResult CurrentSelectedGroupInEditViewUpdate(string groupname, AdministrationModel model)
        {
            GlobalVariables.CurrentSelectedGroupInEditGroupViewAssignView = GlobalVariables.sorter.Groups.FirstOrDefault(g => g.Name == groupname);

            return PartialView("../Administration/_GroupInfo", model);
        }
        #region "Old ADMINISTRATIONEDITVIEW"

        public IActionResult AddTutor(AdministrationModel model)
        {
            if (HttpContext.Session.GetString("LoggedIn") == null)
            {
                return View("LogInError");
            }

            //SelectedGruppeSuchen
            model.SelectedGroup = model.FindGroupByName(model.SelectedGroupName);
            model.SelectedTutor = model.FindTutorByName(model.SelectedTutorName);

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

            model.SelectedGroup = model.FindGroupByName(model.SelectedTutorName);
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

        [HttpPost]
        public PartialViewResult AddGroup(GroupCreationModel model)
        {
            Group gr = new Group("Neue Gruppe");
            gr.Name = model.Name;
            gr.Room = model.Room;
            GlobalVariables.sorter.Groups.Add(gr);

            AdministrationModel adminModel = new AdministrationModel();
            adminModel.CurrentSelectedGroup = adminModel.groups[0];
            return PartialView("_StatsView", adminModel);
        }

        public IActionResult DeleteSelectedGroup(AdministrationModel model)
        {
            model.SelectedGroup = model.FindGroupByName("Neue Gruppe");
            GlobalVariables.sorter.Groups.Remove(model.SelectedGroup);
            model.RefreshGroups();
            return View("../Administration/AdminTestView", model);
        }

        #endregion

        #region "New ADMINISTRATIONEDITVIEW"
        [HttpPost]
        public IActionResult SerializeGroups(AdministrationModel model)
        {
            Serializer.SaveGroupSorter();

            return View("../Administration/AdministrationImportView", model);
        }

        public PartialViewResult UpdateGroupProperties(string groupName, string groupRoom, AdministrationModel model)
        {
            GlobalVariables.sorter.Groups.FirstOrDefault(x => x.Name == GlobalVariables.CurrentSelectedGroupInEditGroupViewAssignView.Name).Name = groupName;
            model.RefreshGroups();
            GlobalVariables.CurrentSelectedGroupInEditGroupViewAssignView = GlobalVariables.sorter.Groups.FirstOrDefault(x => x.Name == groupName);

            return PartialView("../Administration/_EditGroupView", model);
        }
        #endregion
    }
}