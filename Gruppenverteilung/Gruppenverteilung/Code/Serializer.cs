using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public static class Serializer
    {
        public static string SerializeGroup(Group group)
        {
            string output = JsonConvert.SerializeObject(group);
            return output;
        }

        public static Group DeserializeGroup(string json)
        {
            Group group = JsonConvert.DeserializeObject<Group>(json);
            return group;
        }
    }
}
