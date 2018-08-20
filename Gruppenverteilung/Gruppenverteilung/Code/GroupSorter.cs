using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Code
{
    public class GroupSorter
    {
        public List<Group> groups;
        public List<double> groupscores;
        public GroupSorter()
        {
            groups = new List<Group>();
            //TODO Remove (Groups should be created from json)
            groups.Add(new Group("TestGruppe1"));
            groups.Add(new Group("TestGruppe2"));
            groups.Add(new Group("TestGruppe3"));
            groups.Add(new Group("TestGruppe4"));
            groups.Add(new Group("TestGruppe5"));

            groupscores = new List<double>();
        } 

        //Fill group list
        ///TODO Database based
        public void ReadGroups() { }

        public Group FindBestGroup(Member member)
        {
            Group BestGroup = FindNextEmptyGroup();
            if (BestGroup == null)
            {
                double WorstMemberCourseRateInGroup = groups[0].CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                foreach (Group group in groups)
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
            foreach (Group g in groups)
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
