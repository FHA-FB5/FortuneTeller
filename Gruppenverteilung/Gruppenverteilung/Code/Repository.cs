using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gruppenverteilung.Models;

namespace Gruppenverteilung.Code
{
    public class Repository
    {
        public StudentDistributorDbContext context { get; set; }

        public Repository()
        {
            context = new StudentDistributorDbContext();
        }

        public bool EmailIsAlreadyUsed(string email)
        {
            TblMember member = context.TblMember.SingleOrDefault(m => m.Email == email);



            return (member == null) ? false : true;
        }
    }
}
