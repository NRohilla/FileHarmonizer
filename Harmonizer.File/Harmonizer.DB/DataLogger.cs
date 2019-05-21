using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Harmonizer.DB.Data
{
   public static class DataLogger
    {
        public static void Write(string MethodName, string msg)
        {
            try
            {
                string rootPath = HttpContext.Current.Server.MapPath("~").TrimEnd('\\') + "\\DataError\\";
                string lPath = rootPath + "\\Error.txt";
                using (StreamWriter sw = File.AppendText(lPath))
                {
                    sw.WriteLine(DateTime.Now + " : Method Name : " + MethodName + " :- " + msg);
                }
            }
            catch (Exception ex)
            {
                // get error in logger to write in file
            }
        }
    }
}
