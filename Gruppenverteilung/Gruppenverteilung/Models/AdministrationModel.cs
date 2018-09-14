using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gruppenverteilung.Models
{
    public class AdministrationModel
    {
        public List<Group> groups;
        public List<Tutor> tutors;
        public Group SelectedGroup{ get; set; }
        public Tutor SelectedTutor { get; set; }
        public IEnumerable<SelectListItem> GroupSelectList { get; set; }
        public String SelectedGroupName { get; set; }
        public IEnumerable<SelectListItem> TutorSelectList { get; set; }
        public String SelectedTutorName { get; set; }
        public string SelectedGroupRoom { get; set; }
        public String AddTutorMessage { get; set; }
        public bool AddIsSuccessful { get; set; }
        public IEnumerable<SelectListItem> ZugewieseneTutorenSelectList { get; set; }
        public Group CurrentSelectedGroup { get; set; }

        public AdministrationModel()
        {
            SelectedGroup = new Group();
            RefreshGroups();
            RefreshTutors();
            CurrentSelectedGroup = new Group();
            //HACK:
            GlobalVariables.ToAssignTutors_ForAssignView = tutors;
        }

        public void RefreshGroups()
        {
            groups = GlobalVariables.sorter.Groups;

            List<SelectListItem> lst = new List<SelectListItem>();

            foreach(Group group in groups)
            {
                //GroupSelectList.ToList().Add(new SelectListItem(g.Name, g.Name));
                lst.Add(new SelectListItem(group.Name, group.Name));
            }

            GroupSelectList = lst;
        }

        public void RefreshTutors()
        {
            tutors = GlobalVariables.sorter.Tutors;

            List<SelectListItem> lstTutoren = new List<SelectListItem>();

            foreach (Tutor tutor in tutors)
            {
                //GroupSelectList.ToList().Add(new SelectListItem(g.Name, g.Name));
                SelectListItem sli = new SelectListItem(tutor.Name, tutor.Name);
                if (tutor.HasGroup ==false)
                {
                    lstTutoren.Add(sli);
                }
            }
            TutorSelectList = lstTutoren;

            List<SelectListItem> lstZT = new List<SelectListItem>();

            foreach(Tutor tutor in SelectedGroup.TutorList)
            {
                SelectListItem sli = new SelectListItem(tutor.Name, tutor.Name);
                lstZT.Add(sli);
            }

            ZugewieseneTutorenSelectList = lstZT;
        }
        public Group FindGroupByName(string selectedGroupName)
        {
            return groups.Find(i => i.Name == selectedGroupName);
        }
        public Tutor FindTutorByName(string selectedTutorName)
        {
            return tutors.Find(i => i.Name == selectedTutorName);
        }
    }
}
