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
            DataInputModel InputModel = new DataInputModel();
            return View(InputModel);
        }

        [HttpPost]
        public IActionResult SubmitData(DataInputModel model)
        {
            DataOutputModel dataOutputModel = new DataOutputModel();
            Group BestGroup = new Group();

            Member Ersti = new Member(model.Name, model.Vorname, model.Alter, model.Studiengang, model.Geschlecht);

            //GlobalVariables.DatabaseConnection.InsertMember(model.Vorname, model.Name, model.Alter, model.Geschlecht, model.Studiengang);
            BestGroup = GlobalVariables.sorter.FindBestGroup(Ersti);

            using (var context = new StudentDistributorDbContext())
            {
                TblMember member = context.TblMember.SingleOrDefault(m => m.Email == model.Email);
                //Check if new User (Email)
                if (member == null)
                { 
                    //Insert new member
                    member = new TblMember
                    {
                        FirstName = model.Vorname,
                        LastName = model.Name,
                        Age = model.Alter,
                        Email = model.Email,
                        Course = model.Studiengang.ToString(),
                        Gender = model.Geschlecht.ToString()
                    };

                    context.Add(member);
                    context.SaveChanges();

                    //Assign member to best group
                    TblGroup tblGroup = context.TblGroup.Single(group => group.Name == BestGroup.Name);
                    TblMember tblMember = context.TblMember.OrderByDescending(m => m.MemberId).FirstOrDefault();
                    TblGroupMember grp_member = new TblGroupMember { GroupId = tblGroup.GroupId, MemberId = tblMember.MemberId };
                    context.Add(grp_member);
                    context.SaveChanges();
                }
                else
                {
                    //Return to Input view with Panelinfo (Duplicate Email)
                    DataInputModel InputModel = new DataInputModel();
                    InputModel.EmailIsDuplicate = true;
                    return View("../Home/Index", InputModel);
                }
            }

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

            dataOutputModel.GruppenName = BestGroup.Name;

            ///Rückgabe der view mit passendem Model.
            return View("../DataOutput/ShowGroupView", dataOutputModel);
        }

        [HttpPost]
        public IActionResult GroupLookup(DataInputModel model)
        {
            DataOutputModel dataOutputModel = new DataOutputModel();

            TblGroup Group;
            //Get Group
            using (var context = new StudentDistributorDbContext())
            {
                TblMember member = context.TblMember.Single(mem => mem.Email == model.Email);
                TblGroupMember grpmember = context.TblGroupMember.SingleOrDefault(gm => gm.MemberId == member.MemberId);
                Group = context.TblGroup.Single(grp => grp.GroupId == grpmember.GroupId);
            }

            dataOutputModel.GruppenName = Group.Name;

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
