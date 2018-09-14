using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public static class GlobalVariables
    {

        public static GroupSorter sorter = Serializer.DeserializeGroupSorter("Groups.json");
        public static Logger TestLogger = new Logger("LogFile_Test.txt");
        public static byte[] passwordbyte_hashed = { 255, 135, 179, 126, 206, 136, 142, 185, 43, 179, 194, 75, 120, 229, 220, 177, 210, 237, 148, 52 };
        public static DbConnection DatabaseConnection = new DbConnection();

        //HACK: WEil ich zu schlecht bin das mit dem Mdel im Controller zu managen -.-
        public static Group CurrentSelectedGroupInTutorAssignView = new Group();
        public static List<Tutor> ToAssignTutors_ForAssignView = new List<Tutor>();
    }
}
