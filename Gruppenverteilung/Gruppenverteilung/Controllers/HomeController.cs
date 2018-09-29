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
            //return View("DataInputView");
            return View();
        }

        [HttpPost]
        public IActionResult SubmitData(DataInputModel model)
        {
            DataOutputModel dataOutputModel = new DataOutputModel();
            Group BestGroup = new Group();
            Member Ersti = new Member(model.Name, model.Vorname, model.Alter, model.Studiengang, model.Geschlecht);

            //If member already in Database => Get current group! ELSE find best group.
            string CurrentMemberGroupname = GlobalVariables.DatabaseConnection.GetMembersGroupName(Ersti);
            
            if (CurrentMemberGroupname == "")
            {
                GlobalVariables.DatabaseConnection.InsertMember(model.Vorname, model.Name, model.Alter, model.Geschlecht, model.Studiengang);
                BestGroup = GlobalVariables.sorter.FindBestGroup(Ersti);
                GlobalVariables.DatabaseConnection.AssignMemberToGroup(GlobalVariables.DatabaseConnection.GetLastMemberID(), GlobalVariables.DatabaseConnection.GetGroupIDByName(BestGroup.Name));


                //LOGGING: Added Member Log
                GlobalVariables.TestLogger.LogLine(String.Format("(Member Added) -> {0} ({1}), {2}, {3} - in Group \"{4}\"", model.Name, model.Alter, model.Studiengang.ToString(), model.Geschlecht.ToString(), BestGroup.Name));

                //LOGGING: Group-State Log
                foreach (Group group in GlobalVariables.sorter.Groups)
                {
                    GlobalVariables.TestLogger.LogLine(String.Format("(Group State) -> GroupName: {0}, MemberCount: {1}, Male: {2:N2}%, Female: {3:N2}%, MCD: {4:N2}%, INF: {5:N2}%, WINF: {6:N2}%, ETECH: {7:N2}% ",
                        group.Name,
                        group.MemberList.Count,
                        group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Weiblich).Value * 100,
                        group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Maennlich).Value * 100,
                        group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.MCD).Value * 100,
                        group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Informatik).Value * 100,
                        group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Wirtschaftsinformatik).Value * 100,
                        group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Elektrotechnik).Value * 100
                        ));
                }
            }
            else
            {
                BestGroup = GlobalVariables.sorter.Groups.Find(group => group.Name.Replace(" ", "") == CurrentMemberGroupname);
            }
            dataOutputModel.GruppenName = BestGroup.Name;

            ///Rückgabe der view mit passendem Model.
            return View("../DataOutput/ShowGroupView", dataOutputModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
