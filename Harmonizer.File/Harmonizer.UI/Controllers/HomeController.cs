using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Harmonizer.Core;
using Ionic.Zip;

namespace Harmonizer.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           
            // Home
            return View();
        }

        public ActionResult About()
        {
            // About
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //Contact
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult VerifyUser()
        {
            return View();
        }
        public ActionResult ReActive()
        {
            return View();
        }

        public ActionResult IndustrySector()
        {
            return View();
        }

        public ActionResult TermAndCondition()
        {
            return View();

        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        
       
        public FileResult Help()
        {
            string fileType = "application/octet-stream";
            var outputStream = new MemoryStream();
            string FilePath = Server.MapPath("~/Help/FHGHelp.zip");
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddFile(FilePath,"Help");
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AppendHeader("content-disposition", "attachment; filename=FGHHelp.zip");
                zipFile.Save(outputStream);
            }
            outputStream.Position = 0;
            return new FileStreamResult(outputStream, fileType);
        }

        public ActionResult Disclaimer()
        {
            return View();
        }

        public ActionResult ReturnPolicy()
        {
            return View();
        }



    }
}