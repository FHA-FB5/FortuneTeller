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

        //Fill group list
        ///TODO Database based
        public void ReadGroups() { }
        public void ReadTutors() { }
        public Group FindBestGroup(Member member)
        {
            Group BestGroup = FindNextEmptyGroup();
            if (BestGroup == null)
            {
                double WorstMemberCourseRateInGroup = Groups[0].CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                foreach (Group group in Groups)
                {
                    double GroupCourseRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                    if (GroupCourseRate == WorstMemberCourseRateInGroup)
                    {
                        if (BestGroup != null && BestGroup.MemberList.Count > group.MemberList.Count)
                        {
                            BestGroup = group;
                            WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                        }
                        else if (BestGroup == null)
                        {
                            BestGroup = group;
                            WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                        }                       
                    }
                    else if (GroupCourseRate < WorstMemberCourseRateInGroup)
                    {
                        BestGroup = group;
                        WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                    }
                }
            }

            ///TODO Eventuell von ausßerhalb, da die methode FINDE beste gruppe heißt. (oder umbennen zu HINZUFÜGEN zu beste gruppe).
            BestGroup.AddMember(member);

            return BestGroup;
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
                FindBestGroup(member);
            }

        }

    }
}
