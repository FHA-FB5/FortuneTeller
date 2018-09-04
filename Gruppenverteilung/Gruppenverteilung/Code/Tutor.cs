using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class Tutor
    {
        public string Name { get; set; }
        public Studiengang Studiengang { get; set; }
        public bool HasGroup { get; set; }
        public Tutor()
        {

        }

        public Tutor(string name, Studiengang studiengang)
        {
            Name = name;
            Studiengang = studiengang;
        }
    }
}
