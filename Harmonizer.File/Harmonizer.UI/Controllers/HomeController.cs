using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Harmonizer.Core;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using Ionic.Zip;

namespace Harmonizer.UI.Controllers
{
    public class HomeController : Controller
    {
        UserData _userData = new UserData();
        public ActionResult Index()
        {
            if (Request.QueryString["token"] != null && Session["UserID"]==null)
            {
                CreateSessions();
            }
                // Home
                return View();
        }

        
        public ActionResult About()
        {
            // About
            if (Request.QueryString["token"] != null && Session["UserID"] == null)
            {
                CreateSessions();
            }
            ViewBag.Message = "Your application description page.";

            return View();
        }

       
        public ActionResult Contact()
        {
            //Contact
            if (Request.QueryString["token"] != null && Session["UserID"] == null)
            {
                CreateSessions();
            }
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

        public void CreateSessions()
        {
                var token = Request.QueryString["token"];
                string UserIPAddrss = Request.UserHostAddress;
                string UserBrowserName = Request.Browser.Browser.ToLower().Trim();
                DataSet ds = _userData.GetCustomSessionData(UserIPAddrss, UserBrowserName, token, "select");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["UserID"] = ds.Tables[0].Rows[0]["UserID"];
                    Session["Role"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Role"]);
                    Session["Email"] = ds.Tables[0].Rows[0]["EmailID"];
                    Session["SECID"] = ds.Tables[0].Rows[0]["SECID"];
                    Session["BPID"] = ds.Tables[0].Rows[0]["BPID"];
                    Session["Partner"] = ds.Tables[0].Rows[0]["Partner"];
                    Session["BPType"] = ds.Tables[0].Rows[0]["BPType"];
                    Session["expiredate"] = _userData.GetExpiredate(ds.Tables[0].Rows[0]["UserID"].ToString());

                }
            }




    }
}