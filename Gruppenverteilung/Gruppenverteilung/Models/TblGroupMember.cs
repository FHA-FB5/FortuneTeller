using System;
using System.Collections.Generic;

namespace Gruppenverteilung.Models
{
    public partial class TblGroupMember
    {
        public int GroupMemberId { get; set; }
        public int MemberId { get; set; }
        public int GroupId { get; set; }
    }
}
