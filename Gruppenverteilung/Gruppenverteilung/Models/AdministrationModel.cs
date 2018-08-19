using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class AdministrationModel
    {
        public List<Gruppenverteilung.Code.Group> groups;

        public AdministrationModel()
        {
            RefreshGroups();
        }

        public void RefreshGroups()
        {
            groups = GlobalVariables.sorter.groups;
        }
        
    }
}
