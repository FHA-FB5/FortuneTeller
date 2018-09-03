using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Code
{
    public class GroupSorter
    {
        public List<Group> Groups { get; set; }
        public List<Tutor> Tutors { get; set; }
        public List<double> groupscores;
        public GroupSorter()
        {
            Groups = new List<Group>();
            //TODO Remove (Groups should be created from json)
            Groups.Add(new Group("TestGruppe1"));
            Groups.Add(new Group("TestGruppe2"));
            Groups.Add(new Group("TestGruppe3"));
            Groups.Add(new Group("TestGruppe4"));
            Groups.Add(new Group("TestGruppe5"));

            Tutors = new List<Tutor>();
            Tutors.Add(new Tutor("TestTutor1", Studiengang.Informatik));
            Tutors.Add(new Tutor("TestTutor2", Studiengang.Informatik));
            Tutors.Add(new Tutor("TestTutor3", Studiengang.Informatik));
            Tutors.Add(new Tutor("TestTutor4", Studiengang.Informatik));
            Tutors.Add(new Tutor("TestTutor5", Studiengang.Informatik));
            groupscores = new List<double>();
        }
        public double AverageMemberCount
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.MemberList.Count;
                }

                return averge / Groups.Count;
            }
            set { AverageMemberCount = value; }
        }
        public double AverageMaleRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Maennlich).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageMaleRate = value; }
        }
        public double AverageFemaleRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Weiblich).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageFemaleRate = value; }
        }
        public double AverageMCDRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.MCD).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageMaleRate = value; }
        }
        public double AverageINFRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Informatik).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageINFRate = value; }
        }
        public double AverageWINFRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Wirtschaftsinformatik).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageWINFRate = value; }
        }
        public double AverageETECHRate
        {
            get
            {
                double averge = 0.0;

                foreach (Group group in Groups)
                {
                    averge += group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Elektrotechnik).Value;
                }

                return averge / Groups.Count;
            }
            set { AverageETECHRate = value; }
        }        
       
        //Fill group list
        ///TODO Database based
        public void ReadGroups() { }
        public void ReadTutors() { }
        //public Group FindBestGroup(Member member)
        //{
        //    Group BestGroup = FindNextEmptyGroup();
        //    if (BestGroup == null)
        //    {
        //        double WorstMemberCourseRateInGroup = Groups[0].CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
        //        foreach (Group group in Groups)
        //        {
        //            double GroupCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
        //            if (GroupCourseRate == WorstMemberCourseRateInGroup)
        //            {
        //                if (BestGroup != null && BestGroup.MemberList.Count > group.MemberList.Count)
        //                {
        //                    BestGroup = group;
        //                    WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
        //                }
        //                else if (BestGroup == null)
        //                {
        //                    BestGroup = group;
        //                    WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
        //                }                       
        //            }
        //            else if (GroupCourseRate < WorstMemberCourseRateInGroup)
        //            {
        //                BestGroup = group;
        //                WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
        //            }
        //        }
        //    }

        //    ///TODO Eventuell von ausßerhalb, da die methode FINDE beste gruppe heißt. (oder umbennen zu HINZUFÜGEN zu beste gruppe).
        //    BestGroup.AddMember(member);

        //    return BestGroup;
        //}

        public Group FindBestGroup(Member member)
        {
            //We haven't filtered any groups yet.
            List<Group> FilterdGroups = GlobalVariables.sorter.Groups;

            FilterdGroups = GetLowestMemberGroupsFromGroupList(FilterdGroups);
            FilterdGroups = GetLowestGenderRateFromGroupList(FilterdGroups, member.Geschlecht);
            FilterdGroups = GetLowestCourseRateFromGroupList(FilterdGroups, member.Studiengang);

            //TODO Auslagern
            FilterdGroups[0].AddMember(member);

            return FilterdGroups[0];
        }

        private List<Group> GetLowestMemberGroupsFromGroupList(List<Group> grouplist)
        {
            /*
             What is a low member Group?:
                |--------|--------|
             Most     avarage   Lowest
             Member   Member    Member
                         |--------|
                             Low
                            Member
             */
            //Hold all low member groups in the end.
            List<Group> FilterdGroups = new List<Group>();

            foreach (Group group in grouplist)
            {
                //If Groups membercount is lesser than the AverageCount
                if (group.MemberList.Count <= this.AverageMemberCount)
                {
                    FilterdGroups.Add(group);
                }
            }

            return (FilterdGroups.Count == 0) ? grouplist : FilterdGroups;
        }
        private List<Group> GetLowestGenderRateFromGroupList(List<Group> grouplist, Geschlecht gender)
        {
            /*
             What is a low gender rate Group?:
                |--------|--------|
             Highest   Avarage   Lowest
             Gender    Gender    Gender
             Rate      Rate      Rate
                         |--------|
                             Low
                            Gender
                             Rates
             */

            //Hold all low gender rate groups in the end.
            List<Group> FilterdGroups = new List<Group>();
            double GroupGenderRate = 0.0;
            double AverageGenderRate = 0.0;
            foreach (Group group in grouplist)
            {
                //Searching for the right Rates
                GroupGenderRate = (gender == Geschlecht.Maennlich) ? group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Maennlich).Value : group.GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Weiblich).Value;
                AverageGenderRate = (gender == Geschlecht.Maennlich) ? AverageMaleRate : AverageFemaleRate;

                //If Groups Genderrate is lesser than the AverageCount, add to filterd Groups
                if (GroupGenderRate <= AverageGenderRate)
                {
                    FilterdGroups.Add(group);
                }
            }

            return (FilterdGroups.Count == 0) ? grouplist : FilterdGroups;
        }
        private List<Group> GetLowestCourseRateFromGroupList(List<Group> grouplist, Studiengang course)
        {
            /*
             What is a low gender rate Group?:
                |--------|--------|
             Highest   Avarage   Lowest
             Course    Course    Course
             Rate      Rate      Rate
                         |--------|
                             Low
                            Course
                             Rates
             */

            //Hold all low course rate groups in the end.
            List<Group> FilterdGroups = new List<Group>();

            double GroupeCourseRate = 0.0;
            double AverageCourseRate = 0.0;
            foreach (Group group in grouplist)
            {
                //Searching for the right Rates
                switch (course)
                {
                    case Studiengang.MCD:
                        GroupeCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.MCD).Value;
                        AverageCourseRate = AverageMCDRate;
                        break;
                    case Studiengang.Informatik:
                        GroupeCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Informatik).Value;
                        AverageCourseRate = AverageINFRate;
                        break;
                    case Studiengang.Wirtschaftsinformatik:
                        GroupeCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Wirtschaftsinformatik).Value;
                        AverageCourseRate = AverageWINFRate;
                        break;
                    case Studiengang.Elektrotechnik:
                        GroupeCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Elektrotechnik).Value;
                        AverageCourseRate = AverageETECHRate;
                        break;
                }
                //If Groups courserate is lesser than the AverageCourseRate
                if (GroupeCourseRate <= AverageCourseRate)
                {
                    FilterdGroups.Add(group);
                }
            }

            return (FilterdGroups.Count == 0) ? grouplist : FilterdGroups;
        }

        private Group FindNextEmptyGroup()
        {
            foreach (Group g in Groups)
            {
                if (g.MemberList.Count == 0)
                    return g;
            }

            return null;
        }

        public void SimulateByFile(string path)
        {
            string line;
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            while ((line = sr.ReadLine()) != null)
            {
                string[] splittedLine = line.Split(';');
                Studiengang stdgang = Studiengang.Informatik;
                if (splittedLine[3] == "inf")
                    stdgang = Studiengang.Informatik;
                else if (splittedLine[3] == "winf")
                    stdgang = Studiengang.Wirtschaftsinformatik;
                else if (splittedLine[3] == "MCD")
                    stdgang = Studiengang.MCD;
                else if (splittedLine[3] == "etech")
                    stdgang = Studiengang.Elektrotechnik;

                Geschlecht gender = Geschlecht.Maennlich;
                if (splittedLine[2] == "m")
                    gender = Geschlecht.Maennlich;
                else if (splittedLine[2] == "w")
                    gender = Geschlecht.Weiblich;

                Member member = new Member(splittedLine[0], Convert.ToInt16(splittedLine[1]), stdgang, gender);
                Group BestGroup = FindBestGroup(member);

                //LOGGING: Added Member Log
                GlobalVariables.TestLogger.LogLine(String.Format("(Member Added) -> {0} ({1}), {2}, {3} - in Group \"{4}\"", member.Name, member.Age, member.Studiengang.ToString(), member.Geschlecht.ToString(), BestGroup.Name));

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

        }

    }
}
