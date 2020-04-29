using Harmonizer.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Harmonizer.DB;
using Harmonizer.Core.Model;
using System.Web.Security;
using System.Data;
using System.IO;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using System.Configuration;

namespace Harmonizer.UI.Controllers
{
    [GeneralExceptionHandlerAttribute]
    public class AccountController : Controller
    {
        AdminData _admindata = new AdminData();
        Harmonizer.DB.Data.UserData _userData = new DB.Data.UserData();
        // GET: Account this is a test commit
        //https://github.com/fharmonizer/FileHarmonizer.git
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SignUp()
        {


            List<SelectListItem> lstIndustry = new List<SelectListItem>();
            DataSet ds = _userData.GetIndustry();
            lstIndustry.Add(new SelectListItem { Text = "Select Industry", Value = "0" });
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstIndustry.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["Industry"].ToString(), Value = ds.Tables[0].Rows[i]["Share"].ToString() });
                }
            }

            ViewData["lstIndustry"] = lstIndustry;

            ViewData["Countrylst"] = GetCountry();
            ViewData["Languagelst"] = GetLanguage();
            return View();
        }
        public List<LanguageTimeZone> GetLanguage()
        {
            List<LanguageTimeZone> lst = new List<LanguageTimeZone>();
            DataSet ds = _userData.GetLanguageList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LanguageTimeZone lstLang = new LanguageTimeZone();
                    lstLang.Language = ds.Tables[0].Rows[i]["Language"].ToString();
                    lstLang.LanguageDescription = ds.Tables[0].Rows[i]["Language"].ToString() + "-" + ds.Tables[0].Rows[i]["LanguageDescription"].ToString();
                    lst.Add(lstLang);
                }
            }
            lst.Insert(0, new LanguageTimeZone { Language = "Select Language", LanguageDescription = "Select Language" });
            return lst;

        }
        public ActionResult SignIn()
        {
            DataLogger.Write("Accoutn-SignIn", "Request for SignIn");
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Login(UserLogin userLogin)
        {
            string checkFolder = "";
            string msg = "Please fill correct user id and password.";
            Core.Model.LoginHistory logHist = new LoginHistory();
            Core.Model.User userData = new Core.Model.User();

            userData = _userData.GetUserDetailByUserIDAndPassword(userLogin.UserID, userLogin.Password);
            if (userData != null)
            {

                // data found

                // Redirect to Dashboard with active
                msg = "";
                // store data in session
                Session["UserID"] = userData.UserID;
                Session["Role"] = userData.Role;
                Session["Email"] = userData.EmailID;
                Session["SECID"] = userData.SECID;
                Session["BPID"] = userData.BPID;
                Session["Partner"] = userData.Partner;
                Session["BPType"] = userData.BPType;

                FormsAuthentication.SetAuthCookie(userData.UserID, true);
                logHist.UserID = userData.UserID;
                logHist.Server = "";
                _userData.LoginHistory(logHist);
                // check and create directory
                checkFolder = CreateFolder();
                // Set custom session data
                userData.UserIPAddress = Request.UserHostAddress;
                userData.UserBrowserName = Request.Browser.Browser.ToLower().Trim();
                userData.SessionToken = Guid.NewGuid().ToString();
                _userData.InsertUpdateCustomSessionData(userData, "insert");
                TempData["expiredate"] = userData.ExpireDate.Date;
                if (userData.ExpireDate.Date >= DateTime.Today.Date && userData.IsActive && userData.ValidFrom.Date <= DateTime.Today.Date)
                {
                    if (!string.IsNullOrWhiteSpace(checkFolder))
                    {
                        TempData["message"] = checkFolder;
                        return RedirectToAction("SignIn");
                    }
                    else
                    {
                        if (userData.Role == 2)
                        {
                            //return RedirectToAction("FilterTemplate", "FHFile");
                            return RedirectToAction("ManageTag", "FHFile", new { token = userData.SessionToken });
                        }
                        else
                        {
                            return RedirectToAction("Individual", "FHFile", new { token = userData.SessionToken });
                        }
                    }
                }
                else if (userData.ValidFrom.Date > DateTime.Today.Date)
                {
                    // you are not active contact to admin
                    msg = "You are not eligible to login in system. Your start date not eligible.";
                }
                else if (userData.ExpireDate.Date < DateTime.Today.Date && userData.IsActive)//Expire date as ValideTo and ValideTo naver > BPValideTo
                {
                    //msg = "You are not active user or validity date expire, please contact to admin";
                    return RedirectToAction("UserProfile", "User", new { token = userData.SessionToken });
                    // you are not active contact to admin

                }
                else if (userData.ExpireDate.Date < DateTime.Today.Date && !userData.IsActive)//Expire date as ValideTo and ValideTo naver > BPValideTo
                {
                    // you are not active contact to admin
                    msg = "You are not active user or validity date expire, please contact to admin";
                }
                else
                {
                    // your activation plan period or trail expire purchage new plan
                    msg = "You are not active user. Please contact to admin.";
                }

            }
            TempData["message"] = msg;
            TempData.Keep();
            return RedirectToAction("SignIn");//, JsonRequestBehavior.AllowGet);
        }

        public string CreateFolder()
        {
            string returnValue = "";
            try
            {
                string sPath = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString();
                string tPath = Server.MapPath("~").TrimEnd('\\') + "\\Target\\" + Session["BPID"].ToString();
                string hPath = Server.MapPath("~").TrimEnd('\\') + "\\Harmonized\\" + Session["BPID"].ToString();
                string lPath = Server.MapPath("~").TrimEnd('\\') + "\\Log\\" + Session["BPID"].ToString();

                if (!Directory.Exists(sPath))
                {
                    // create source folder  
                    Directory.CreateDirectory(sPath);
                }
                if (!Directory.Exists(tPath))
                {
                    // create target folder  
                    Directory.CreateDirectory(tPath);
                }
                if (!Directory.Exists(hPath))
                {
                    // create target folder  
                    Directory.CreateDirectory(hPath);
                }
                if (!Directory.Exists(lPath))
                {
                    // create target folder  
                    Directory.CreateDirectory(lPath);
                }
            }
            catch (Exception ex)
            {
                returnValue = ex.StackTrace + "-------" + ex.Message;

            }
            return returnValue;
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
            string token = Guid.NewGuid().ToString();
            UserLogin logInfo = new UserLogin();
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
                    if (registerUserData.User.RegistrationType == "BusinessPartner")
                        registerUserData.User.Role = 5;// Default as User
                    else
                        registerUserData.User.Role = 2;


                    registerUserData.PersonalInfo.Initials = registerUserData.PersonalInfo.FirstName != null ? registerUserData.PersonalInfo.FirstName.Substring(0, 1).ToUpper() : "";
                    registerUserData.PersonalInfo.MiddelInitials = registerUserData.PersonalInfo.MiddleName != null ? registerUserData.PersonalInfo.MiddleName.Substring(0, 1).ToUpper() : "";
                    registerUserData.PersonalInfo.UserRole = registerUserData.User.Role;
                    registerUserData.PersonalInfo.Language = registerUserData.PersonalInfo.Language.ToLower().Trim() != "Select Language".ToLower().Trim() ? registerUserData.PersonalInfo.Language.Trim() : "";
                    registerUserData.PersonalInfo.DateOfBorn = registerUserData.PersonalInfo.DateOfBorn.ToString().Trim().ToLower() != "mm/dd/yyyy" ? registerUserData.PersonalInfo.DateOfBorn : null;

                    // BPInfo
                    registerUserData.BPinfo.CompanyDate = registerUserData.BPinfo.CompanyDate.ToString().Trim().ToLower() != "mm/dd/yyyy" ? registerUserData.BPinfo.CompanyDate : null;
                    registerUserData.BPinfo.Language = registerUserData.BPinfo.Language.ToLower().Trim() != "Select Language".ToLower().Trim() ? registerUserData.BPinfo.Language.Trim() : "";

                    bool IsCreated = _userData.CreateUser(registerUserData.User, registerUserData.AddInfo, registerUserData.PersonalInfo, registerUserData.BPinfo);
                    if (IsCreated)
                    {
                        optValue = 1;
                        response = "Successfully user created!";

                        // Auto login and redirect to individual page 11/12/2018
                        logInfo = new UserLogin()
                        {
                            UserID = registerUserData.User.UserID,
                            Password = registerUserData.User.Password
                        };
                        DefaultSetSessionOnFirstLogin(logInfo, token);
                        //return RedirectToAction("Login", "Account", logInfo);
                    }

                }
                else
                {
                    if (IsUserExist)
                        response = "User already in system!";
                    else if (IsEmailIDExist)
                        response = "User email ID already in system";
                }

            }
            else
            {

            }

            return Json(new { operationValue = optValue, responseText = response, loginInfo = logInfo, roleId = Session["Role"] != null ? Session["Role"].ToString() : registerUserData.User.Role.ToString(), tokenValue = token }, JsonRequestBehavior.AllowGet);
        }

        public void DefaultSetSessionOnFirstLogin(UserLogin userLogin, string token)
        {
            Core.Model.LoginHistory logHist = new LoginHistory();
            Core.Model.User userData = new Core.Model.User();
            userData = _userData.GetUserDetailByUserIDAndPassword(userLogin.UserID, userLogin.Password);
            Session["UserID"] = userData.UserID;
            Session["Role"] = userData.Role;
            Session["Email"] = userData.EmailID;
            Session["SECID"] = userData.SECID;
            Session["BPID"] = userData.BPID;
            Session["Partner"] = userData.Partner;
            Session["BPType"] = userData.BPType;
            FormsAuthentication.SetAuthCookie(userData.UserID, true);
            logHist.UserID = userData.UserID;
            logHist.Server = "";
            _userData.LoginHistory(logHist);
            // check and create directory
            CreateFolder();
            // Custom session management
            userData.UserIPAddress = Request.UserHostAddress;
            userData.UserBrowserName = Request.Browser.Browser.ToLower().Trim();
            userData.SessionToken = token;
            TempData["expiredate"] = DateTime.Today.Date.AddMonths(1);
            _userData.InsertUpdateCustomSessionData(userData, "insert");
        }

        public ActionResult Logout()
        {
            try
            {
                string token = "";
                //for get request
                if (Request.QueryString["token"] != null)
                    token = Request.QueryString["token"];

                //for post request
                if (HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["token"] != null)
                    token = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["token"];

                // remove custom session value
                Core.Model.User userData = new Core.Model.User();
                userData.UserIPAddress = Request.UserHostAddress;
                userData.UserBrowserName = Request.Browser.Browser.ToLower().Trim();
                userData.SessionToken = token;
                _userData.InsertUpdateCustomSessionData(userData, "delete");
            }
            catch (Exception ex)
            {
                DataLogger.Write("Account-Logout", ex.Message);
            }
            // clear session
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("SignIn", "Account");
        }


        public ActionResult CommanUserData()
        {
            CommanUserData obj = new CommanUserData();
            try
            {
                obj = _userData.GetCommanData(Convert.ToString(Session["UserID"]));
            }
            catch (Exception ex)
            {
                DataLogger.Write("Account-CommanUserData", ex.Message);
            }
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        public List<Country> GetCountry()
        {
            List<Country> objCountrylst = new List<Country>();
            objCountrylst = _admindata.GetCountryList();
            objCountrylst.Insert(0, new Country { ID = 0, Alpha3 = "0", Alpha2 = "0", CountryName = "Select Country" });
            return objCountrylst;

        }
        public ActionResult GetBusinessType()
        {
            List<SelectListItem> lstBusinessType = new List<SelectListItem>();
            DataSet ds = _userData.GetBusinessTypeList();
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstBusinessType.Add(new SelectListItem { Text = Convert.ToString(ds.Tables[0].Rows[i]["Description"]), Value = Convert.ToString(ds.Tables[0].Rows[i]["BPTYPE"]) });
                }
            }

            return Json(lstBusinessType, JsonRequestBehavior.AllowGet);

        }

    }
}