using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Gruppenverteilung.Code
{
    public class Logger
    {
        #region Probs...
        public string FileName { get; private set; }
        public StreamWriter Writer { get; private set; }
        #endregion

        #region Constructors...
        public Logger(string filename)
        {
            FileName = filename;
        }
        #endregion

        #region Public methods...
        public void LogLine(string value)
        {
            ///new StreaWriter(filename, append)
            Writer = new StreamWriter(FileName, true);
            ///LogFormat: " [Date, Time]: Linevalue  "
            Writer.WriteLine(String.Format("[{0}, {1}]: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), value));
            Writer.Close();
        }
        #endregion

        #region Private methods..

        #endregion
    }
}
