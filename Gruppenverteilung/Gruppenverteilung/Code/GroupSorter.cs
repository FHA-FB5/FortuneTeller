using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Code;

namespace Gruppenverteilung.Code
{
    public class GroupSorter
    {
        private List<Group> groups;

        public GroupSorter() { }

        //Fill group list
        ///TODO Database based
        public void ReadGroups() { }

        private Group FindBestGroup()
        {
            ///TODO Verwende CalculateScore um besten Score zu fnden
            ///TODO Gebe die Gruppe mit dem besten Score zurück
            return new Group();
        }

        public Group SortMemberIntoGroup(Member member)
        {
            ///TODO Finde beste Gruppe
            ///TODO Schreibe member in die Gruppe ein (DB!)
            ///TODO Übergebe Gruppenname an model für Outputview

            return new Group();
        }
    }
}
