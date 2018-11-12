using Harmonizer.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Harmonizer.DB;
using Harmonizer.Core.Model;
using System.Web.Security;

namespace Harmonizer.UI.Controllers
{
    public class AccountController : Controller
    {
        Harmonizer.DB.Data.UserData _userData = new DB.Data.UserData();
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
            string msg = "Please fill correct user id and password";
            Core.Model.LoginHistory logHist = new LoginHistory();
            Core.Model.User userData = new Core.Model.User();
            userData = _userData.GetUserDetailByUserIDAndPassword(userLogin.UserID, userLogin.Password);
            if (userData != null)
            {
                // data found
                if (userData.ExpireDate.Date >= DateTime.Today.Date && userData.IsActive)
                {
                    // Redirect to Dashboard with active
                    msg = "";
                    // store data in session
                    Session["UserID"] = userData.UserID;
                    Session["Role"] = userData.Role;
                    FormsAuthentication.SetAuthCookie(userData.UserID, false);
                    logHist.UserID = userData.UserID;
                    logHist.Server = "";
                    _userData.LoginHistory(logHist);
                    return RedirectToAction("dashboard", "User");
                }
                else if (userData.ExpireDate.Date >= DateTime.Today.Date && !userData.IsActive)
                {
                    // you are not active contact to admin
                    msg = "You are not active user, please contact to admin";
                }
                else
                {
                    // your activation plan period or trail expire purchage new plan
                    msg = "Your business plan has been expire. Please purchage new plan.";
                }

            }
            TempData["message"] = msg;
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public ActionResult RestPassword(ForgotPassword forgotPassword)
        {
            string msg = "";
            int resultVlaue = -1;
            User user = new User();
            user.UserID = forgotPassword.UserID;
            user.EmailID = forgotPassword.EmailID;
            user.Password = forgotPassword.NewPassword;
            resultVlaue = _userData.ResetPassword(user);
            if (resultVlaue == 1)
                msg = "Password reset sucessfully!";
            else
                msg = "Password not updated, please fill correct user id and email id";

            TempData["message"] = msg;

            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public ActionResult RegisterUser(RegisterUser registerUserData)
        {

            int optValue = -1;
            string response = "Failed to register user. Pleae try again.";
            if (registerUserData.User.UserID != "" && registerUserData.User.Password != "" && registerUserData.User.ConfirmPassword != "" && registerUserData.User.EmailID != "")
            {
                // Check data in DB with user name and email Id exist or not 
                bool IsUserExist = _userData.CheckUserByUserID(registerUserData.User.UserID);
                bool IsEmailIDExist = _userData.CheckUserByEmailID(registerUserData.User.EmailID);
                if (!IsUserExist && !IsEmailIDExist)
                {

                    //registerUserData.User.PasswordHasValue = DB.Data.Utility.EncryptPassword(registerUserData.User.Password).ToString();
                    //registerUserData.User.PasswordHash = DB.Data.Utility.EncryptPassword(registerUserData.User.Password).ToString();
                    // registerUserData.User.OriganalPass = DB.Data.Utility.EncryptPassword(registerUserData.User.Password);// match next time while change
                    registerUserData.User.IsActive = true;
                    registerUserData.User.ActiveDate = DateTime.Now;
                    registerUserData.User.ExpireDate = DateTime.Now.AddMonths(1);// for trail
                    registerUserData.User.Role = 2;// Default as User


                    registerUserData.PersonalInfo.Initials = registerUserData.PersonalInfo.FirstName.Substring(0, 1).ToUpper();
                    registerUserData.PersonalInfo.UserRole = 2;// default;


                    bool IsCreated = _userData.CreateUser(registerUserData.User, registerUserData.AddInfo, registerUserData.PersonalInfo);
                    if (IsCreated)
                    {
                        optValue = 1;
                        response = "Successfully user created!";
                    }

                }
                else
                {
                    if (!IsUserExist)
                        response = "User already in system!";
                    else if (IsEmailIDExist)
                        response = "User email ID already in system";
                }

            }
            else
            {

            }

            return Json(new{ operationValue = optValue, responseText = response }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}