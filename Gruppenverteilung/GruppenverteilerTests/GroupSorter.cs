using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Gruppenverteilung.Code;

namespace GruppenverteilerTests
{
    [TestClass]
    public class GroupSorter
    {
        [TestMethod]
        public void MemberSortingTest()
        {
            // arrange
            List<Group> groups = new List<Group>();
            groups.Add(new Group("grp1"));
            groups.Add(new Group("grp2"));
            groups.Add(new Group("grp3"));
            groups.Add(new Group("grp4"));
            groups.Add(new Group("grp5"));

            Member member1 = new Member("Test1", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member member2 = new Member("Test2", 25, Studiengang.Wirtschaftsinformatik, Geschlecht.Maennlich);
            Member member3 = new Member("Test3", 25, Studiengang.Elektrotechnik, Geschlecht.Maennlich);
            Member member4 = new Member("Test4", 25, Studiengang.MCD, Geschlecht.Maennlich);
            Member member5 = new Member("Test5", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member member6 = new Member("Test6", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member member7 = new Member("Test7", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member member8 = new Member("Test8", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member member9 = new Member("Test9", 25, Studiengang.Informatik, Geschlecht.Maennlich);


            List<double> scores = new List<double>();
            scores.Add(100);
            scores.Add(100);
            scores.Add(100);
            scores.Add(100);
            scores.Add(100);

            Gruppenverteilung.Code.GroupSorter sorter = new Gruppenverteilung.Code.GroupSorter();
            sorter.groups = groups;
            sorter.groupscores = scores;
            // act
            Group grp1 = sorter.SortMemberIntoGroup(member1);
            Group grp2 = sorter.SortMemberIntoGroup(member2);
            Group grp3 = sorter.SortMemberIntoGroup(member3);
            Group grp4 = sorter.SortMemberIntoGroup(member4);
            Group grp5 = sorter.SortMemberIntoGroup(member5);
            Group grp6 = sorter.SortMemberIntoGroup(member6);
            Group grp7 = sorter.SortMemberIntoGroup(member7);

            // assert
            Assert.AreEqual(grp1.Name, "grp1");
            Assert.AreEqual(grp2.Name, "grp2");
            Assert.AreEqual(grp3.Name, "grp3");
            Assert.AreEqual(grp4.Name, "grp4");
            Assert.AreEqual(grp5.Name, "grp5");
            //Assert.AreEqual(grp6.Name, "grp1");
        }

        [TestMethod]
        public void CourseRateUpdateTest()
        {
            // arrange
            Group TestGroup = new Group("TestGroup");
            Member TMember_1 = new Member("Test1", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member TMember_2 = new Member("Test2", 25, Studiengang.Wirtschaftsinformatik, Geschlecht.Maennlich);
            Member TMember_3 = new Member("Test2", 25, Studiengang.Elektrotechnik, Geschlecht.Maennlich);
            Member TMember_4 = new Member("Test4", 25, Studiengang.MCD, Geschlecht.Maennlich);
            List<Member> member = new List<Member>();
            member.Add(TMember_1);
            member.Add(TMember_2);
            member.Add(TMember_3);
            member.Add(TMember_4);
            TestGroup.MemberList = member;

            //act
            TestGroup.UpdateCourseRates();

            //assert
            KeyValuePair<Studiengang, double> pair = TestGroup.CourseRates.Find(kvp => kvp.Key == Studiengang.Informatik);
            double infrate = pair.Value;
            pair = TestGroup.CourseRates.Find(kvp => kvp.Key == Studiengang.Wirtschaftsinformatik);
            double winfrate = pair.Value;
            pair = TestGroup.CourseRates.Find(kvp => kvp.Key == Studiengang.MCD);
            double mcdrate = pair.Value;
            pair = TestGroup.CourseRates.Find(kvp => kvp.Key == Studiengang.Elektrotechnik);
            double etechrate = pair.Value;

            Assert.AreEqual(infrate, 0.25);
            Assert.AreEqual(winfrate, 0.25);
            Assert.AreEqual(mcdrate, 0.25);
            Assert.AreEqual(etechrate, 0.25);
        }

        [TestMethod]
        public void GenderRateUpdateTest()
        {
            // arrange
            Group TestGroup = new Group("TestGroup");
            Member TMember_1 = new Member("Test1", 25, Studiengang.Informatik, Geschlecht.Weiblich);
            Member TMember_2 = new Member("Test2", 25, Studiengang.Wirtschaftsinformatik, Geschlecht.Maennlich);
            Member TMember_3 = new Member("Test2", 25, Studiengang.Elektrotechnik, Geschlecht.Weiblich);
            Member TMember_4 = new Member("Test4", 25, Studiengang.MCD, Geschlecht.Weiblich);
            Member TMember_5 = new Member("Test5", 25, Studiengang.Informatik, Geschlecht.Weiblich);

            List<Member> member = new List<Member>();
            member.Add(TMember_1);
            member.Add(TMember_2);
            member.Add(TMember_3);
            member.Add(TMember_4);
            member.Add(TMember_5);
            TestGroup.MemberList = member;

            //act
            TestGroup.UpdateGenderRates();

            //assert
            KeyValuePair<Geschlecht, double> pair = TestGroup.GenderRates.Find(kvp => kvp.Key == Geschlecht.Maennlich);
            double mrate = pair.Value;
            pair = TestGroup.GenderRates.Find(kvp => kvp.Key == Geschlecht.Weiblich);
            double wrate = pair.Value;

            Assert.AreEqual(mrate, 0.2);
            Assert.AreEqual(wrate, 0.8);
        }

        [TestMethod]
        public void FindBestGroupTest()
        {
            // arrange
            List<Group> groups = new List<Group>();
            groups.Add(new Group("grp1"));
            groups.Add(new Group("grp2"));
            groups.Add(new Group("grp3"));
            groups.Add(new Group("grp4"));
            groups.Add(new Group("grp5"));

            Member InfMember = new Member("Test1", 25, Studiengang.Informatik, Geschlecht.Maennlich);
            Member WinfMember = new Member("Test2", 25, Studiengang.Wirtschaftsinformatik, Geschlecht.Maennlich);
            Member EtechMember = new Member("Test3", 25, Studiengang.Elektrotechnik, Geschlecht.Maennlich);
            Member MCDMember = new Member("Test4", 25, Studiengang.MCD, Geschlecht.Maennlich);

            Gruppenverteilung.Code.GroupSorter sorter = new Gruppenverteilung.Code.GroupSorter();
            sorter.groups = groups;

            //act
            Group grp1 = sorter.FindBestGroup(InfMember);
            Group grp2 = sorter.FindBestGroup(WinfMember);
            Group grp3 = sorter.FindBestGroup(EtechMember);
            Group grp4 = sorter.FindBestGroup(MCDMember);
            Group grp5 = sorter.FindBestGroup(InfMember);
            Group grp6 = sorter.FindBestGroup(InfMember);
            Group grp7 = sorter.FindBestGroup(InfMember);
            Group grp8 = sorter.FindBestGroup(InfMember);
            Group grp9 = sorter.FindBestGroup(InfMember);
            Group grp10 = sorter.FindBestGroup(MCDMember);
            Group grp11 = sorter.FindBestGroup(MCDMember);

            //assert
            Assert.AreEqual(grp1.Name, "grp1");
            Assert.AreEqual(grp2.Name, "grp2");
            Assert.AreEqual(grp3.Name, "grp3");
            Assert.AreEqual(grp4.Name, "grp4");
            Assert.AreEqual(grp5.Name, "grp5");
            Assert.AreEqual(grp6.Name, "grp2");
            Assert.AreEqual(grp7.Name, "grp3");
            Assert.AreEqual(grp8.Name, "grp4");
            Assert.AreEqual(grp9.Name, "grp2");
            Assert.AreEqual(grp10.Name, "grp1");
            Assert.AreEqual(grp11.Name, "grp2");
        }
    }
}
