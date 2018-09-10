using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class Member
    {
        public string Name { get; set; }
        public string Vorname { get; set; }
        public int Age { get; set; }
        public Studiengang Studiengang { get; set; }
        public Geschlecht Geschlecht { get; set; }

        public Member()
        {

        }

        public Member(string name,string vorname, int age, Studiengang studiengang, Geschlecht geschlecht)
        {
            Name = name;
            Vorname = vorname;
            Age = age;
            Studiengang = studiengang;
            Geschlecht = geschlecht;
        }
        public Member(string name, int age, Studiengang studiengang, Geschlecht geschlecht)
        {
            Name = name;
            Age = age;
            Studiengang = studiengang;
            Geschlecht = geschlecht;
        }
    }
}
