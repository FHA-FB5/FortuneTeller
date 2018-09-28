using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class GroupStatModel
    {
        public List<Group> Groups { get; set; }
        public Group Group { get; set; }

        //Evntl allgemeine Stats (wv. Inf, wInf etc.)
        public int ErstiSum {
            get
            {
                int sum = 0;

                if (Groups != null)
                {
                    foreach (Group g in Groups)
                    {
                        sum += g.MemberList.Count;
                    }
                }

                return sum;
            }
        }
        public GroupStatModel(Group group)
        {
            Group = group;
        }
        public GroupStatModel()
        {
        }
    }
}
