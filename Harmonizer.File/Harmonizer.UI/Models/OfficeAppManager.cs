using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using EXC = Microsoft.Office.Interop.Excel;
using PPT = Microsoft.Office.Interop.PowerPoint;
using WORD = Microsoft.Office.Interop.Word;

using System.Reflection;
using System.Diagnostics;
namespace Harmonizer.UI.Models
{
    public static class OfficeAppManager
    {
        static EXC.Application excel = null;
        static WORD.Application word = null;
        static PPT.Application ppt = null;

        public static void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception)
            {
                obj = null;
            }
        }

        public static EXC.Application ExcelApp()
        {
            if (excel != null)
                CloseExcelApp();

            excel = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application")) as EXC.Application;
            excel.DisplayAlerts = false;

            return excel;
        }

        public static PPT.Application PowerPointApp(int count)
        {
            if (ppt != null)
                ClosePowerpointApp();

            try
            {
                ppt = Activator.CreateInstance(Type.GetTypeFromProgID("PowerPoint.Application")) as PPT.Application;
                ppt.DisplayAlerts = PPT.PpAlertLevel.ppAlertsNone;
            }
            catch (Exception x)
            {
                if ((x.ToString().Contains("0x800706B5") || x.ToString().Contains("0x800706BE")) && count < 5)
                    PowerPointApp(count + 1);
                //else
                //    Log.Instance.Write("OfficeAppManager.PowerPointApp() - " + x.ToString(), Log.Type.Error);
            }

            return ppt;
        }

        public static WORD.Application WordApp()
        {
            if (word != null)
                CloseWordApp();

            word = Activator.CreateInstance(Type.GetTypeFromProgID("Word.Application")) as WORD.Application;
            word.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            return word;
        }

        public static void ClosePowerpointApp()
        {
            if (ppt != null)
            {
                try
                {
                    ppt.Quit();
                    ReleaseObject(ppt);
                }
                catch (Exception x)
                {
                    //if (x.ToString().Contains("0x800706BA"))
                    //    Log.Instance.Write("OfficeAppManager.ClosePowerpointApp() - " + x.ToString(), Log.Type.Normal);
                    //else
                    //    Log.Instance.Write("OfficeAppManager.ClosePowerpointApp() - " + x.ToString(), Log.Type.Error);
                }
            }
            ppt = null;
        }

        public static void CloseWordApp()
        {
            if (word != null)
            {
                try
                {
                    word.Quit();
                    ReleaseObject(word);
                }
                catch (Exception x)
                {
                    //Log.Instance.Write("OfficeAppManager.CloseWordApp() - " + x.ToString(), Log.Type.Error);
                }
            }
            word = null;
        }

        public static void CloseExcelApp()
        {
            if (excel != null)
            {
                try
                {
                    excel.Quit();
                    ReleaseObject(excel);
                }
                catch (Exception x)
                {
                    // if (x.ToString().Contains("0x800AC472"))
                    //Log.Instance.Write("OfficeAppManager.CloseExcelApp() - " + x.ToString(), Log.Type.Normal);
                    //else
                    //Log.Instance.Write("OfficeAppManager.CloseExcelApp() - " + x.ToString(), Log.Type.Error);
                }
            }
            excel = null;
        }

        public static void CloseAllOfficeApps()
        {
            ClosePowerpointApp();
            CloseWordApp();
            CloseExcelApp();
        }

        public static void ForceCloseOfficeApps()
        {
            Process[] list = Process.GetProcesses();

            try
            {
                foreach (Process p in list)
                {
                    if (p.ProcessName.Contains("POWERPNT") || p.ProcessName.Contains("WINWORD") || p.ProcessName.Contains("EXCEL"))
                    {
                        if (!p.HasExited)
                            p.Kill();
                    }
                }
            }
            catch (Exception x)
            {
                //Log.Instance.Write("OfficeAppManager.ForceCloseOfficeApps() - " + x.ToString(), Log.Type.Error);
            }
        }
    }
}