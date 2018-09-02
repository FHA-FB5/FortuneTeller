using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gruppenverteilung.Models
{
    public class AdministrationLoginModel
    {
        public const string SessionKeyName = "Username";
        public const string SessionKeyPassword = "Password";

        public string SessionInfo_Username { get; set; }
        public string SessionInfo_Password { get; set; }

        public AdministrationLoginModel() { }

    }
}
