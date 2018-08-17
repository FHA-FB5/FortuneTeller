using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class Group
    {
        public int MaxMember { get; }
        public string Name { get; set; }
        public List<Member> MemberList { get; set; }
        public double AverageAge { get; set; }
        public double CurrentScore { get; set; }
        public Group()
        {
            Name = "";
            MaxMember = 100;
            MemberList = new List<Member>();
            CurrentScore = 1.0;
        }
        public Group(string name, int maxmember)
        {
            Name = name;
            MaxMember = maxmember;
            MemberList = new List<Member>();
            CurrentScore = 1.0;
        }

        public double CalculateScore(Member member)
        {
            if (MemberList.Count > 0)
            {
                ///TODO Berechne Score der Gruppe mit neuem Member!
                double ageScore = 0.0, genderScore = 0.0, stdScore = 0.0;
                foreach (Member m in MemberList)
                {
                    AverageAge += m.Age;
                }
                AverageAge /= (MemberList.Count+1);
                ageScore = member.Age - AverageAge;

                ///Get rid of the sign 
                if (ageScore != 0)
                {
                    ageScore = Math.Pow(ageScore, 2);
                    ageScore = Math.Sqrt(ageScore);
                }

                foreach (Studiengang studiengang in (Studiengang[])Enum.GetValues(typeof(Studiengang)))
                {
                    int count = 0;
                    foreach (Member m in MemberList)
                    {
                        if (m.Studiengang == studiengang)
                            count++;
                    }
                    stdScore += ((count / (double)MemberList.Count) - 1 / Enum.GetNames(typeof(Studiengang)).Length);
                }

                foreach (Geschlecht geschlecht in (Geschlecht[])Enum.GetValues(typeof(Geschlecht)))
                {
                    int count = 0;
                    foreach (Member m in MemberList)
                    {
                        if (m.Geschlecht == geschlecht)
                            count++;
                    }
                    genderScore += ((count / MemberList.Count) - 1 / Enum.GetNames(typeof(Geschlecht)).Length);
                }


                return ageScore + stdScore + genderScore;
            }
            return 0.0;
        }

        public void AddMember(string filepath)
        {
            string line;
            System.IO.StreamReader sr = new System.IO.StreamReader(filepath);
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
                MemberList.Add(member);
            }
        }
        public bool AddMember(Member member)
        {
            if (MemberList.Count < MaxMember)
            {
                MemberList.Add(member);
                return true;
            }
            else
            {
                //Test
                //Trace.TraceWarning("Maximale Member anzahl wurde überschritten, member konnte nicht hinzugefügt werden.");
                return false;
            }
        }
    }
}
