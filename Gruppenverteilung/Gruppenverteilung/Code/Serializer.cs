using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static string SerializeGroupSorter()
        {
            string sorterJson = JsonConvert.SerializeObject(GlobalVariables.sorter);
            return sorterJson;
        }

        public static void SaveGroupSorter()
        {
            string sorterJson = SerializeGroupSorter();
            System.IO.File.WriteAllText("Groups.json",sorterJson,Encoding.UTF8);
        }

        public static GroupSorter DeserializeGroupSorter(string pathFrom)
        {
            GroupSorter sorter = JsonConvert.DeserializeObject<GroupSorter>(getStringFromJson(pathFrom));
            return sorter;
        }

        private static string getStringFromJson(string path)
        {
            return System.IO.File.ReadAllText(path, Encoding.UTF8);
        }
    }
}
