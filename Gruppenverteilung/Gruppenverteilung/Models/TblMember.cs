using System;
using System.Collections.Generic;

namespace Gruppenverteilung.Models
{
    public partial class TblMember
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Course { get; set; }
    }
}
