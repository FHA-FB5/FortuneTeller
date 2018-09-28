using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Models
{
    public class EditModel
    {
        public List<Group> Groups { get; set; }
        public Group CurrentGroup { get; set; }
        public string SaveMassage { get; private set; }
        public EditModel()
        {
            SaveMassage = "Änderungen gespeichert!";
        }
    }
}
