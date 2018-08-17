using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class Member
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Studiengang Studiengang { get; set; }
        public Geschlecht Geschlecht { get; set; }

        public Member(string name, int age, Studiengang studiengang, Geschlecht geschlecht)
        {
            Name = name;
            Age = age;
            Studiengang = studiengang;
            Geschlecht = geschlecht;
        }
    }
}
