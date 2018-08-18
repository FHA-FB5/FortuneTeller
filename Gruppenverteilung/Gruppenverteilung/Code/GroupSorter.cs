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
                double WorstMemberCourseRateInGroup = 1;
                foreach (Group group in groups)
                {
                    double GroupRate = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                    if ((GroupRate - 0.25) < (WorstMemberCourseRateInGroup - 0.25))
                    {
                        WorstMemberCourseRateInGroup = group.CourseRates.FirstOrDefault(kvp => kvp.Key == member.Studiengang).Value;
                        BestGroup = group;
                    }
                }
            }

            BestGroup.AddMember(member);

            return BestGroup;
        }

        public Group SortMemberIntoGroup(Member member)
        {
            ///First fill emptygroups with at least on member
            Group g = FindNextEmptyGroup();
            ///If all groups have at least one member calculate best group
            if (g == null)
            {
                g = FindBestGroup(member);
            }
            g.AddMember(member);
            return g;
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
    }
}
