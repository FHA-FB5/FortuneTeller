using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sorting_Algorithm.Code;

namespace Sorting_Algorithm.Models
{
    public class DataInputModel
    {
        public string Name { get; set; }
        public int Alter { get; set; }
        public Studiengang Studiengang { get; set; }
        public Geschlecht Geschlecht { get; set;}

        public DataInputModel()
        {

        }
    }
}