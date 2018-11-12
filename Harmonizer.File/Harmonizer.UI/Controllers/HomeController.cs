using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Harmonizer.Core;


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
    }
}