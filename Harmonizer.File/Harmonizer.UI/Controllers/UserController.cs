using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using Harmonizer.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Harmonizer.UI.Controllers
{
    [GeneralExceptionHandlerAttribute]
    public class UserController : Controller
    {
        AdminData _admindata = new AdminData();
        UserData _userData = new UserData();

        [SessionTimeoutFilter]
        // GET: User
        public ActionResult Dashboard()
        {
            return View();
        }

        [SessionTimeoutFilter]
        public ActionResult StartMyFreeMonth()
        {
            List<PlanDetails> lst = new List<PlanDetails>();
            DataSet ds = new DataSet();
            ds = _admindata.GetAllPlan("select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PlanDetails planDetails = new PlanDetails();
                    planDetails.ID = Convert.ToInt32(row["ID"]);
                    planDetails.Title = row["Title"].ToString();
                    planDetails.Description = row["Description"].ToString();
                    planDetails.FreeInfo = row["FreeInfo"].ToString();
                    planDetails.Cost = Convert.ToDecimal(row["Cost"]);
                    planDetails.Validity = Convert.ToInt32(row["Validity"]);
                    planDetails.Year = Convert.ToInt32(row["Year"]);
                    planDetails.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    planDetails.UpdateDate = Convert.ToDateTime(row["UpdateDate"]);
                    planDetails.OrderDisplay = Convert.ToInt32(row["OrderDisplay"]);
                    planDetails.IsActive = Convert.ToBoolean(row["IsActive"]);
                    planDetails.Group = row["GroupOrder"].ToString();
                    lst.Add(planDetails);
                }
            }
            if(Session["BPType"]!=null)
                lst = lst.Where(x => x.Group.ToLower().Trim() == Session["BPType"].ToString().ToLower().Trim() || x.Cost==0).ToList();

            return View(lst);
        }
      
        public ActionResult Reactivate()
        {
            return View();
        }

        [SessionTimeoutFilter]
        public ActionResult UserProfile()
        {
            RegisterUser registerUser = new RegisterUser();
            User user = new User();
            PersonalInformation personalInformation = new PersonalInformation();
            AddressIinformation addressIinformation = new AddressIinformation();
            BPInfo bPInfo = new BPInfo();
            string BPID = Session["BPID"].ToString();
            string UserID = Session["UserID"].ToString();
            int RollID = Convert.ToInt32(Session["Role"]);

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

            ViewData["Countrylst"] =new AccountController().GetCountry();
            ViewData["Languagelst"] = new AccountController().GetLanguage();

            DataSet dsProfile = _userData.GetProfileDetails(BPID,UserID,RollID,"GetData");
            if (dsProfile.Tables[0].Rows.Count > 0)
            {
                addressIinformation.AddressID = Convert.ToInt32(dsProfile.Tables[0].Rows[0]["AddressID"]);
                addressIinformation.CompName = dsProfile.Tables[0].Rows[0]["CompName"].ToString();
                addressIinformation.CompName2 = dsProfile.Tables[0].Rows[0]["CompName2"].ToString();
                addressIinformation.Department = dsProfile.Tables[0].Rows[0]["Department"].ToString();
                addressIinformation.Address1 = dsProfile.Tables[0].Rows[0]["Address1"].ToString();
                addressIinformation.Address2 = dsProfile.Tables[0].Rows[0]["Address2"].ToString();
                addressIinformation.POBox = dsProfile.Tables[0].Rows[0]["POBox"].ToString();
                addressIinformation.City = dsProfile.Tables[0].Rows[0]["City"].ToString();
                addressIinformation.State = dsProfile.Tables[0].Rows[0]["State"].ToString();
                addressIinformation.Zip = dsProfile.Tables[0].Rows[0]["Zip"].ToString();
                addressIinformation.CountryKey = dsProfile.Tables[0].Rows[0]["CountryKey"].ToString();
                addressIinformation.Country = dsProfile.Tables[0].Rows[0]["Country"].ToString();
                addressIinformation.Language = dsProfile.Tables[0].Rows[0]["Language"].ToString();
                addressIinformation.Phone = dsProfile.Tables[0].Rows[0]["Phone"].ToString();
                addressIinformation.WorkPhone = dsProfile.Tables[0].Rows[0]["WorkPhone"].ToString();
                addressIinformation.MobilePhone = dsProfile.Tables[0].Rows[0]["MobilePhone"].ToString();
                addressIinformation.Fax = dsProfile.Tables[0].Rows[0]["Fax"].ToString();
                addressIinformation.TimeZone = dsProfile.Tables[0].Rows[0]["TimeZone"].ToString();
                addressIinformation.Location = dsProfile.Tables[0].Rows[0]["Location"].ToString();
                addressIinformation.DistrictPostalCode = dsProfile.Tables[0].Rows[0]["DistrictPostalCode"].ToString();
                addressIinformation.county = dsProfile.Tables[0].Rows[0]["County"].ToString();


                if (RollID == 2 || RollID == 6) {
                    personalInformation.PersonalID =Convert.ToInt32(dsProfile.Tables[0].Rows[0]["PersonalID"]);
                    personalInformation.BirthCountry = dsProfile.Tables[0].Rows[0]["BirthCountry"].ToString();
                    personalInformation.Country= dsProfile.Tables[0].Rows[0]["pCountry"].ToString();
                    personalInformation.DateOfBorn= Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["DateOfBorn"]);
                    personalInformation.Email= dsProfile.Tables[0].Rows[0]["Email"].ToString();
                    personalInformation.FirstName= dsProfile.Tables[0].Rows[0]["FirstName"].ToString();
                    personalInformation.LastName= dsProfile.Tables[0].Rows[0]["LastName"].ToString();
                    personalInformation.AKA= dsProfile.Tables[0].Rows[0]["AKA"].ToString();
                    personalInformation.Language = dsProfile.Tables[0].Rows[0]["pLanguage"].ToString();
                    personalInformation.LastName2 = dsProfile.Tables[0].Rows[0]["LastName2"].ToString();
                    personalInformation.Website = dsProfile.Tables[0].Rows[0]["Website"].ToString();
                    personalInformation.Profession = dsProfile.Tables[0].Rows[0]["Profession"].ToString();
                    personalInformation.Title = dsProfile.Tables[0].Rows[0]["Title"].ToString().Trim();
                    personalInformation.MiddleName = dsProfile.Tables[0].Rows[0]["MiddleName"].ToString();
                    personalInformation.Name2 = dsProfile.Tables[0].Rows[0]["Name2"].ToString();
                    personalInformation.Gender = dsProfile.Tables[0].Rows[0]["Gender"].ToString().Trim();
                }
                else if (RollID == 5)
                {
                   
                    bPInfo.AddressID= Convert.ToInt32(dsProfile.Tables[0].Rows[0]["AddressID"]);
                    bPInfo.Country = dsProfile.Tables[0].Rows[0]["bpCountry"].ToString();
                    bPInfo.ContactNameFirst = dsProfile.Tables[0].Rows[0]["bpName"].ToString();
                    bPInfo.City = dsProfile.Tables[0].Rows[0]["bpCity"].ToString();
                    bPInfo.State = dsProfile.Tables[0].Rows[0]["bpState"].ToString();
                    bPInfo.Fax = dsProfile.Tables[0].Rows[0]["bpFax"].ToString();
                    bPInfo.Zip = dsProfile.Tables[0].Rows[0]["bpZip"].ToString();
                    bPInfo.Partner = dsProfile.Tables[0].Rows[0]["Partner"].ToString();
                    bPInfo.Language = dsProfile.Tables[0].Rows[0]["bpLanguage"].ToString();
                    bPInfo.TollFreePhone = dsProfile.Tables[0].Rows[0]["TollFreeNo"].ToString();
                    bPInfo.ContactNameLast = dsProfile.Tables[0].Rows[0]["bpLastName"].ToString();
                    bPInfo.CompName = dsProfile.Tables[0].Rows[0]["bpCompName"].ToString();
                    bPInfo.CompanyEIN = dsProfile.Tables[0].Rows[0]["CompanyEIN"].ToString();
                    bPInfo.CompanyDate = Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["CompanyDate"]);
                    bPInfo.Email = dsProfile.Tables[0].Rows[0]["bpEmail"].ToString();
                    bPInfo.Department = dsProfile.Tables[0].Rows[0]["bpDepartment"].ToString();
                    bPInfo.Website = dsProfile.Tables[0].Rows[0]["bpWebsite"].ToString();
                }
            }
            registerUser.User = user;
            registerUser.PersonalInfo = personalInformation;
            registerUser.AddInfo = addressIinformation;
            registerUser.BPinfo = bPInfo;

            if (TempData["expiredate"] == null)
                TempData["expiredate"] = Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["ExpireDate"]);
            else if (Convert.ToDateTime(TempData["expiredate"]) != Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["ExpireDate"]))
                TempData["expiredate"] = Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["ExpireDate"]);



            ViewBag.ExpireDate =Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();

            TempData.Keep();
            return View(registerUser);
        }
        [SessionTimeoutFilter]
        public ActionResult Archive()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        [SessionTimeoutFilter]
        public ActionResult FHAccountSetting()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        [SessionTimeoutFilter]
        public ActionResult ManageUser()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        public ActionResult _CreateTeamUser()
        {
            RegisterUser registerUser = new RegisterUser();
            ViewData["Countrylst"] =new AccountController().GetCountry();
            ViewData["Languagelst"] = new AccountController().GetLanguage();
            return PartialView("_CreateTeamUser", registerUser);
        }
        public ActionResult ViewAllTeamMember()
        {
            List<RegisterUser> _userList = new List<RegisterUser>();
            _userList = GetTeamMemebrs();
            return PartialView("_ViewAllTeamMember", _userList);
        }

        public List<RegisterUser> GetTeamMemebrs()
        {
            List<RegisterUser> _userList = new List<RegisterUser>();
            RegisterUser _registerUser = new RegisterUser();
            User user = new User();
            PersonalInformation personalInfo = new PersonalInformation();
            AddressIinformation addInfo = new AddressIinformation();
            BPInfo bPinfo = new BPInfo();
            DataSet ds = _userData.GetAllTeamMember(Session["BPID"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    user = new User
                    {
                        UserID = row["UserID"].ToString(),
                        ActiveDate=Convert.ToDateTime(row["ActiveDate"]),
                        ExpireDate = Convert.ToDateTime(row["ExpireDate"]),
                        EmailID = row["RegisterEmal"].ToString(),
                        IsActive=Convert.ToBoolean(row["IsActive"]),
                    };
                    personalInfo = new PersonalInformation
                    {
                        FirstName=row["FirstName"].ToString(),
                        AKA= row["AKA"].ToString(),
                        TeamMemberRole = row["TeamMemberRole"].ToString(),
                        Email= row["personalemail"].ToString(),
                        LastName=row["LastName"].ToString()
                    };
                    _registerUser = new RegisterUser();
                    _registerUser.User = user;
                    _registerUser.PersonalInfo = personalInfo;
                    _userList.Add(_registerUser);
                }
            }
            return _userList;
        }

        [SessionTimeoutFilter]
        public ActionResult RegisterTeamUser(RegisterUser registerUserData)
        {
            string BPID = Session["BPID"].ToString();
            int optValue = 0;
            string response = "";
            bool IsCreated = false;
            if (registerUserData.User.UserID != "" && registerUserData.User.Password != "" && registerUserData.User.ConfirmPassword != "" && registerUserData.User.EmailID != "")
            {
                // Concate Team user with BPID
                if(registerUserData.User.RegistrationType.ToLower()== "TeamMember".ToLower())
                       registerUserData.User.UserID = BPID + registerUserData.User.UserID;

                bool IsUserExist = _userData.CheckUserByUserID(registerUserData.User.UserID);
                bool IsEmailIDExist = _userData.CheckUserByEmailID(registerUserData.User.EmailID);
                if (!IsUserExist && !IsEmailIDExist)
                {
                    registerUserData.User.IsActive = true;
                    registerUserData.User.ActiveDate = DateTime.Now;
                    registerUserData.User.ExpireDate = DateTime.Now.AddMonths(1);
                    registerUserData.User.Role = registerUserData.User.Role;
                    registerUserData.User.BPID = Session["BPID"].ToString();// send BPID

                    registerUserData.PersonalInfo.Initials = registerUserData.PersonalInfo.FirstName != null ? registerUserData.PersonalInfo.FirstName.Substring(0, 1).ToUpper() : "";
                    registerUserData.PersonalInfo.MiddelInitials = registerUserData.PersonalInfo.MiddleName != null ? registerUserData.PersonalInfo.MiddleName.Substring(0, 1).ToUpper() : "";
                    registerUserData.PersonalInfo.UserRole = registerUserData.User.Role;
                    registerUserData.PersonalInfo.Language = registerUserData.PersonalInfo.Language.ToLower().Trim() != "Select Language".ToLower().Trim() ? registerUserData.PersonalInfo.Language.Trim() : "";
                    registerUserData.PersonalInfo.DateOfBorn = registerUserData.PersonalInfo.DateOfBorn.ToString().Trim().ToLower() != "mm/dd/yyyy" ? registerUserData.PersonalInfo.DateOfBorn : null;

                    IsCreated= _userData.CreateTeamUser(registerUserData.User, registerUserData.AddInfo, registerUserData.PersonalInfo, registerUserData.BPinfo);
                }
                else
                {
                    optValue = -1;
                    if (IsUserExist)
                        response = "User already in system!";
                    else if (IsEmailIDExist)
                        response = "User email ID already in system";
                }
            }
            return Json(new{ operationValue = optValue, responseText = response }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveTeamUser(RegisterUser registerUser)
        {
            int result = 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _FolderSetting()
        {
            FolderLocation folderLocation = new FolderLocation();
            DataSet ds = _userData.GetFolderLocation(Session["BPID"].ToString(), "select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.source = ds.Tables[0].Rows[0]["Source"].ToString();
                ViewBag.target = ds.Tables[0].Rows[0]["Target"].ToString();
                ViewBag.convertion = ds.Tables[0].Rows[0]["Conversion"].ToString();
                ViewBag.filter = ds.Tables[0].Rows[0]["Filter"].ToString();
                ViewBag.archive = ds.Tables[0].Rows[0]["Archive"].ToString();
                ViewBag.template = ds.Tables[0].Rows[0]["Template"].ToString();
                ViewBag.report = ds.Tables[0].Rows[0]["Report"].ToString();
                ViewBag.changelog = ds.Tables[0].Rows[0]["Changelog"].ToString();
                ViewBag.errorlog = ds.Tables[0].Rows[0]["Errorlog"].ToString();
            }
            else
            {
                ViewBag.source = DefaultFolderLocation.Source;
                ViewBag.target = DefaultFolderLocation.Target;
                ViewBag.convertion = DefaultFolderLocation.Conversion;
                ViewBag.filter = DefaultFolderLocation.Filter;
                ViewBag.archive = DefaultFolderLocation.Archive;
                ViewBag.template = DefaultFolderLocation.Template;
                ViewBag.report = DefaultFolderLocation.Report;
                ViewBag.changelog = DefaultFolderLocation.Changelog;
                ViewBag.errorlog = DefaultFolderLocation.Errorlog;
            }
            return PartialView("_FolderSetting");
        }

        public ActionResult UpadateRestoreFolderLoc(string op, FolderLocation folderLocation)
        {
            int rValue = 0;
            FolderLocation restoreFolder = new FolderLocation();
            string data = "Update data";
            string BPID = Session["BPID"].ToString();
            if (op == "restore")
            {
                // Restore from constant
                restoreFolder.BPID = BPID;
                restoreFolder.Source = DefaultFolderLocation.Source;
                restoreFolder.Target = DefaultFolderLocation.Target;
                restoreFolder.Conversion = DefaultFolderLocation.Conversion;
                restoreFolder.Filter = DefaultFolderLocation.Filter;
                restoreFolder.Archive = DefaultFolderLocation.Archive;
                restoreFolder.Template = DefaultFolderLocation.Template;
                restoreFolder.Report = DefaultFolderLocation.Report;
                restoreFolder.Changelog = DefaultFolderLocation.Changelog;
                restoreFolder.Errorlog = DefaultFolderLocation.Errorlog;
            }
            else
            {
                // Update with new one
                restoreFolder.BPID = BPID;
                restoreFolder.Source = folderLocation.Source;
                restoreFolder.Target = folderLocation.Target;
                restoreFolder.Conversion = folderLocation.Conversion;
                restoreFolder.Filter = folderLocation.Filter;
                restoreFolder.Archive = folderLocation.Archive;
                restoreFolder.Template = folderLocation.Template;
                restoreFolder.Report = folderLocation.Report;
                restoreFolder.Changelog = folderLocation.Changelog;
                restoreFolder.Errorlog = folderLocation.Errorlog;
            }
            rValue = _userData.UpdateFolderLocation(restoreFolder, "insert");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpadateRestoreNaming(string op, string prostfix, string prefix)
        {
            string data = "Update data";
            int rValue = 0;
            string BPID = Session["BPID"].ToString();
            if (op == "restore")
            {
                // Restore from constant
               
            }
            else
            {
                // Update with new one
            }
            rValue = _userData.UpdateNaming(BPID, prostfix, prefix, "insert");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GeneralSetting()
        {
            return PartialView("_GeneralSetting");
        }
        public ActionResult _NamingSetting()
        {
            string BPID = Session["BPID"].ToString();
            DataSet ds= _userData.GetNaming(BPID,"select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewBag.Postfix = ds.Tables[0].Rows[0]["Postfix"].ToString();
                ViewBag.Prefix = ds.Tables[0].Rows[0]["Prefix"].ToString();
            }
            else
            {
                ViewBag.Postfix = "";
                ViewBag.Prefix = "";
            }
            return PartialView("_NamingSetting");
        }
        public ActionResult _RegistrationSetting()
        {
            CommanUserData commanUserData = new CommanUserData();
            commanUserData = _userData.GetCommanData(Session["UserID"].ToString());
            ViewBag.FHnumber = commanUserData.HarmonizerValue;
            ViewBag.Activedate = string.Format("{0:MM/dd/yyyy}", commanUserData.ActiveDate);
            return PartialView("_RegistrationSetting");
        }
        public ActionResult _ViewLog()
        {
            string fullPath = "";
            string host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            string rootDomain = ConfigurationManager.AppSettings["rootDomain"].ToString();
            if (!string.IsNullOrEmpty(port))
            {
                fullPath = rootDomain + host.TrimEnd('/') + ":" + port + "/Log/" + Session["BPID"] + "/Error.txt";
               
            }
            else
            {
                fullPath = rootDomain + host.TrimEnd('/') + "/Log/" + Session["BPID"] + "/Error.txt";
              
            }
            ViewBag.logpath = fullPath;
            return PartialView("_ViewLog");
        }

        public ActionResult UpdateStatus(Boolean status,string userId)
        {
            int result = 0;
            result = _userData.UpdateUserStatus(userId, Session["BPID"].ToString(), status, DateTime.Now, "UpdateStatus");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUserExpireDate(string date, string userId)
        {
            int result = 0;
            string msg = "";
            DateTime dateValue;
            if (DateTime.TryParseExact(date, "M/d/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
            {
                result = _userData.UpdateUserStatus(userId, Session["BPID"].ToString(), false, dateValue, "UpdateExpiredate");
            }
            else
            {
                msg = "Date format not correct! Please use M/d/yyyy";
                result = -2;
            }
            
            return Json(new { resultValue = result, msgValue = msg }, JsonRequestBehavior.AllowGet);
        }

        // Update User Information
        [SessionTimeoutFilter]
        public ActionResult UpdateUserAddress(AddressIinformation Address)
        {
            string ReturnValue = "";
            int RoleD = Convert.ToInt32(Session["Role"]);
            Address.UserID = Session["UserID"].ToString();
            int rValue = _userData.UpdateUserAddress(Address,RoleD);
            return Json(ReturnValue, JsonRequestBehavior.AllowGet);
        }
        [SessionTimeoutFilter]
        public ActionResult UpdateUserPersonalInfo(PersonalInformation personalInformation)
        {
            string ReturnValue = "";
            personalInformation.UserID = Session["UserID"].ToString();
            int rValue = _userData.UpdateUserPersonalInfo(personalInformation);
            return Json(ReturnValue, JsonRequestBehavior.AllowGet);
        }
        [SessionTimeoutFilter]
        public ActionResult UpdateUserBPInfo(BPInfo bPInfo)
        {
            string ReturnValue = "";
            string BPID = Session["BPID"].ToString();
            bPInfo.UserID= Session["UserID"].ToString();
            int rValue = _userData.UpdateUserBPInfo(bPInfo,BPID);
            return Json(ReturnValue, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeoutFilter]
        public ActionResult ManageShareTag()
        {
            return View("ManageTag");
        }

        [SessionTimeoutFilter]
        public ActionResult MaintainCustomTag()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        [SessionTimeoutFilter]
        public ActionResult viewCustomTag()
        {
            List<Tag> lst = new List<Tag>();
            lst = _userData.GetCustomeTag(Session["BPID"].ToString(),Session["UserID"].ToString());
            return PartialView("_viewCustomTag", lst);
        }


        [SessionTimeoutFilter]
        public ActionResult ViewPrivateTag()
        {
            List<Tag> lstTag = new List<Tag>();
            Tag _tag = new Tag();

            DataSet ds = new DataSet();
            string BPID = Session["BPID"].ToString();
            string UserID = Session["UserID"].ToString();
            ds = _userData.GetPrivateTag(BPID, UserID,"select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    _tag = new Tag()
                    {
                        ID=Convert.ToInt32(row["ID"]),
                        UTAGID= Convert.ToInt32(row["UTAGID"]),
                        TagName=row["Tag"].ToString(),
                        GlobPri=row["GlobPri"].ToString(),
                        Description=row["Description"].ToString(),
                        Share=row["Share"].ToString()
                    };
                    lstTag.Add(_tag);
                }
            }
            return PartialView("_ViewPrivateTag", lstTag);
        }

        [SessionTimeoutFilter]
        public ActionResult UpdateShareValue(int ID,string shareValue,string globalPriValue, int utagId)
        {
            
            int retValue = 0;
            string BPID = Session["BPID"].ToString();
            retValue = _userData.UpdateShareVaue(ID,  shareValue,  globalPriValue,  utagId, BPID, "update");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeoutFilter]
        public ActionResult DeleteShareValue(int ID, string shareValue, string globalPriValue, int utagId)
        {

            int retValue = 0;
            string BPID = Session["BPID"].ToString();
            retValue = _userData.UpdateShareVaue(ID, shareValue, globalPriValue, utagId, BPID, "delete");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }

        public ActionResult CallForRate(string FName,string LName, string EmailId,string ContactNo,string Description)
        {
            string data = "";
            data = SendEmail.CallForRate(FName, LName, EmailId, ContactNo, Description);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}