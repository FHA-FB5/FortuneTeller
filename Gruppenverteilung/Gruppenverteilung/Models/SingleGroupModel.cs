using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class SingleGroupModel
    {
        public Group Group { get; set; }

        public SingleGroupModel()
        {

        }

        public SingleGroupModel(Group group)
        {
            Group = group;
        }
    }
}
