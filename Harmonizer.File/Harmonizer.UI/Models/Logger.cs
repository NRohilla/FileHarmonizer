using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public static class Logger
    {
        public static void Write(string BPID,string msg)
        {
            try
            {
                string rootPath = HttpContext.Current.Server.MapPath("~").TrimEnd('\\') + "\\Log\\";
                string lPath = rootPath + BPID + "\\Error.txt";
                using (StreamWriter sw = File.AppendText(lPath))
                {
                    sw.WriteLine(DateTime.Now + " : BP ID : " + BPID + " :- " + msg);
                }
            }
            catch(Exception ex)
            {
                // get error in logger to write in file
            }
        }
    }
}