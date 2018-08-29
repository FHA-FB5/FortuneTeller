using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class Group
    {
        #region propbs...
        public string Name { get; set; }
        public List<Member> MemberList { get; set; }
        public List<Tutor> TutorList { get; set; }
        public List<KeyValuePair<Studiengang, double>> CourseRates { get; set; }
        public List<KeyValuePair<Geschlecht, double>> GenderRates { get; set; }
        public double AverageAge { get; set; }
        public string Room { get; set; }
        #endregion

        #region Constructors...
        public Group()
        {
            Name = "";
            MemberList = new List<Member>();
            TutorList = new List<Tutor>();
            CourseRates = new List<KeyValuePair<Studiengang, double>>();
            foreach (Studiengang studiengang in (Studiengang[])Enum.GetValues(typeof(Studiengang)))
            {
                CourseRates.Add(new KeyValuePair<Studiengang, double>(studiengang, 0.0));
            }
            GenderRates = new List<KeyValuePair<Geschlecht, double>>();
            foreach (Geschlecht geschlecht in (Geschlecht[])Enum.GetValues(typeof(Geschlecht)))
            {
                GenderRates.Add(new KeyValuePair<Geschlecht, double>(geschlecht, 0.0));
            }
        }
        public Group(string name)
        {
            Name = name;
            MemberList = new List<Member>();
            TutorList = new List<Tutor>();
            CourseRates = new List<KeyValuePair<Studiengang, double>>();
            foreach (Studiengang studiengang in (Studiengang[])Enum.GetValues(typeof(Studiengang)))
            {
                CourseRates.Add(new KeyValuePair<Studiengang, double>(studiengang, 0.0));
            }
            GenderRates = new List<KeyValuePair<Geschlecht, double>>();
            foreach (Geschlecht geschlecht in (Geschlecht[])Enum.GetValues(typeof(Geschlecht)))
            {
                GenderRates.Add(new KeyValuePair<Geschlecht, double>(geschlecht, 0.0));
            }
        }
        #endregion

        #region Methods...
        public void UpdateCourseRates()
        {
            List<int> courseCounts = new List<int>();
            courseCounts.Add(0); // mcd
            courseCounts.Add(0); // inf
            courseCounts.Add(0); // etech
            courseCounts.Add(0); // winf
            
            // Courses member count
            foreach (Member member in MemberList)
            {
                if (member.Studiengang == Studiengang.MCD)
                    courseCounts[0]++;
                else if (member.Studiengang == Studiengang.Informatik)
                    courseCounts[1]++;
                else if (member.Studiengang == Studiengang.Elektrotechnik)
                    courseCounts[2]++;
                else if (member.Studiengang == Studiengang.Wirtschaftsinformatik)
                    courseCounts[3]++;
            }

            //Calculate new CourseRates
            List<KeyValuePair<Studiengang, double>> UpdatedCourseRates = new List<KeyValuePair<Studiengang, double>>();
            for(int i = 0; i < CourseRates.Count; i++) 
            {
                KeyValuePair<Studiengang, double> pair = CourseRates[i];

                KeyValuePair<Studiengang, double> newPair = new KeyValuePair<Studiengang, double>(pair.Key, courseCounts[i] / (double)MemberList.Count);
                KeyValuePair<Studiengang, double> oldPair = CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.MCD); 

                if(i == 1)
                    oldPair = CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Informatik);
                else if(i == 2)
                    oldPair = CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Elektrotechnik);
                else if(i == 3)
                    oldPair = CourseRates.FirstOrDefault(kvp => kvp.Key == Studiengang.Wirtschaftsinformatik);
                
                UpdatedCourseRates.Add(newPair);
            }
            //Update old Rates.
            CourseRates = UpdatedCourseRates;
        }
        public void UpdateGenderRates()
        {
            List<int> GenerCounts = new List<int>();
            GenerCounts.Add(0); // m
            GenerCounts.Add(0); // w

            // Courses member count
            foreach (Member member in MemberList)
            {
                if (member.Geschlecht == Geschlecht.Maennlich)
                    GenerCounts[0]++;
                else if (member.Geschlecht == Geschlecht.Weiblich)
                    GenerCounts[1]++;
            }

            //Calculate new CourseRates
            List<KeyValuePair<Geschlecht, double>> UpdatedGenderRates = new List<KeyValuePair<Geschlecht, double>>();
            for (int i = 0; i < GenerCounts.Count; i++)
            {
                KeyValuePair<Geschlecht, double> pair = GenderRates[i];

                KeyValuePair<Geschlecht, double> newPair = new KeyValuePair<Geschlecht, double>(pair.Key, GenerCounts[i] / (double)MemberList.Count);
                KeyValuePair<Geschlecht, double> oldPair = GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Maennlich);

                if (i == 1)
                    oldPair = GenderRates.FirstOrDefault(kvp => kvp.Key == Geschlecht.Weiblich);

                UpdatedGenderRates.Add(newPair);
            }
            //Update old Rates.
            GenderRates = UpdatedGenderRates;

        }
        //public double CalculateScore(Member member)
        //{
        //    if (MemberList.Count > 0)
        //    {
        //        ///TODO Berechne Score der Gruppe mit neuem Member!
        //        double ageScore = 0.0, genderScore = 0.0, stdScore = 0.0;
        //        foreach (Member m in MemberList)
        //        {
        //            AverageAge += m.Age;
        //        }
        //        AverageAge /= (MemberList.Count);
        //        ageScore = member.Age - AverageAge;

        //        ///Get rid of the sign 
        //        if (ageScore != 0)
        //        {
        //            ageScore = Math.Pow(ageScore, 2);
        //            ageScore = Math.Sqrt(ageScore);
        //        }

        //        foreach (Studiengang studiengang in (Studiengang[])Enum.GetValues(typeof(Studiengang)))
        //        {
        //            int count = 0;
        //            foreach (Member m in MemberList)
        //            {
        //                if (m.Studiengang == studiengang)
        //                    count++;
        //            }
        //            stdScore += ((count / (double)MemberList.Count) - 1 / Enum.GetNames(typeof(Studiengang)).Length);
        //        }

        //        foreach (Geschlecht geschlecht in (Geschlecht[])Enum.GetValues(typeof(Geschlecht)))
        //        {
        //            int count = 0;
        //            foreach (Member m in MemberList)
        //            {
        //                if (m.Geschlecht == geschlecht)
        //                    count++;
        //            }
        //            genderScore += ((count / MemberList.Count) - 1 / Enum.GetNames(typeof(Geschlecht)).Length);
        //        }


        //        return ageScore + stdScore + genderScore;
        //    }
        //    return 0.0;
        //}
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
            MemberList.Add(member);
            UpdateCourseRates();
            UpdateGenderRates();
            return true;
        }

        public void AddTutor(Tutor tutor)
        {
            TutorList.Add(tutor);
        }

        public void RemoveTutor(Tutor tutor)
        {
            TutorList.Remove(tutor);
        }
        #endregion
    }
}
