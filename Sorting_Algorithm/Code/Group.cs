using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
namespace Sorting_Algorithm.Code
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
            ///TODO Berechne Score der Gruppe mit neuem Member!
            double ageScore = 0.0, genderScore = 0.0, stdScore = 0.0;

            ageScore = member.Age - AverageAge;
            ///Get rid of the sign 
            if (ageScore != 0)
            {
                ageScore = Math.Pow(ageScore, 2);            
                ageScore = Math.Sqrt(ageScore);
            }

            foreach (Studiengang studiengang in (Studiengang[]) Enum.GetValues(typeof(Studiengang)))
            {
                int count = 0;
                foreach (Member m in MemberList)
                {
                    if (m.Studiengang == studiengang)
                        count++;
                }
                stdScore += ((count / MemberList.Count) - 1 / Enum.GetNames(typeof(Studiengang)).Length);
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
                Trace.TraceWarning("Maximale Member anzahl wurde überschritten, member konnte nicht hinzugefügt werden.");
                return false;
            }
        }
    }
}