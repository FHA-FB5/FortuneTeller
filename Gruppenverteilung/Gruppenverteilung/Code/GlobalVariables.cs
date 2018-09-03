using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public static class GlobalVariables
    {
        public static GroupSorter Sorter = new GroupSorter();
        public static Logger TestLogger = new Logger("LogFile_Test.txt");
        public static byte[] passwordbyte_hashed = { 255, 135, 179, 126, 206, 136, 142, 185, 43, 179, 194, 75, 120, 229, 220, 177, 210, 237, 148, 52 };
    }
}
