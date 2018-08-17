using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sorting_Algorithm.Code
{
    public class Sorter
    {
        private List<Group> groups;

        public Sorter() { }

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
            ///TODO Finde beste Grupper 
            ///TODO Schreibe member in die Gruppe ein (DB!)
            ///TODO Übergebe Gruppenname an model für Outputview
            
            return new Group();
        }

    }
}