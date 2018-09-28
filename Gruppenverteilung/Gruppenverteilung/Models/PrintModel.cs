using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class PrintModel
    {
        public List<Group> Groups { get; set; }
        public AdministrationAllErstiesModel erstimodel { get; set; }

        public PrintModel()
        {
            erstimodel = new AdministrationAllErstiesModel();
        }
    }
}
