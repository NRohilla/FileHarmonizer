using Harmonizer.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Harmonizer.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account this is a test commit
        //https://github.com/fharmonizer/FileHarmonizer.git
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult SignIn()
        {
            return View();
        }


        public  ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin userLogin)
        {

            return null;
        }

        [HttpPost]
        public ActionResult RestPassword(ForgotPassword forgotPassword)
        {
            return null;
        }
    }
}