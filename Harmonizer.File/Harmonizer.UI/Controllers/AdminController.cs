using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using Harmonizer.UI.Models;

namespace Harmonizer.UI.Controllers
{
    [SessionTimeoutFilter]
    public class AdminController : Controller
    {
        AdminData _admindata = new AdminData();
        UserData _userData = new UserData();
        public ActionResult Country()
        {
            return View();
        }

        public ActionResult Sector()
        {
            return View();
        }

        public ActionResult Industry()
        {

            return View();

        }

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult StandardGlobalTag()
        {
            return View();
        }

        #region Country CRUD
        public ActionResult GetCountry()
        {
            List<Country> objCountrylst = new List<Country>();
            objCountrylst = _admindata.GetCountryList();
            return Json(objCountrylst,JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCountry(Country ObjCountry)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteCountry(ObjCountry);

            if (Result == "1")
            {
                Result = "Country deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCountryDetails(Country ObjCountry)
        {
            string Result = string.Empty;
            Result = _admindata.InsertCountryDetails(ObjCountry);

            if (Result == "1")
            {
                Result = "Country created successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCountryDetails(Country ObjCountry)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateCountryDetails(ObjCountry);

            if (Result == "1")
            {
                Result = "Country created successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Sector CRUD

        public ActionResult GetSectorList()
        {
            List<Sector> objSectorlst = new List<Sector>();
            objSectorlst = _admindata.GetSectorList();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSector()
        {
            List<Sector> objSectorlst = new List<Sector>();
            objSectorlst = _admindata.GetSector();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSector(Sector objSector)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteSector(objSector);

            if (Result == "2")
            {
                Result = "Sector deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSectorDetails(Sector objSector)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateSectorDetails(objSector);

            if (Result == "2")
            {
                Result = "Sector updated successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSectorDetails(Sector objSector)
        {
            string Result = string.Empty;
            Result = _admindata.InsertSectorDetails(objSector);

            if (Result == "2")
            {
                Result = "Sector created successfully";
            }


            return Json(Result, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Industry CRUD

        public ActionResult GetIndustryList()
        {
            List<Industry> objSectorlst = new List<Industry>();
            objSectorlst = _admindata.GetIndustryList();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteIndustry(Industry objIndustry)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteIndustry(objIndustry);

            if (Result == "1")
            {
                Result = "Industry deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddIndustryDetails(Industry ObjIndustry)
        {
            string Result = string.Empty;
            Result = _admindata.InsertIndustryDetails(ObjIndustry);

            if (Result == "1")
            {
                Result = "Industry created successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateIndustryDetails(Industry ObjIndustry)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateIndustryDetails(ObjIndustry);

            if (Result == "1")
            {
                Result = "Industry updated successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Category CRUD

        public ActionResult GetCategoryList()
        {
            List<Category> objSectorlst = new List<Category>();
            objSectorlst = _admindata.GetCategoryList();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCategory(Category objCategory)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteCategory(objCategory);

            if (Result == "1")
            {
                Result = "Category deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCategoryDetails(Category objCategory)
        {
            string Result = string.Empty;
            Result = _admindata.InsertCategoryDetails(objCategory);

            if (Result == "1")
            {
                Result = "Category created successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCategoryDetails(Category objCategory)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateCategoryDetails(objCategory);

            if (Result == "1")
            {
                Result = "Category updated successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSectorFromCategory()
        {
            List<Sector> objSectorlst = new List<Sector>();
            objSectorlst = _admindata.GetSectorFromCategory();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIndustry()
        {
            List<Industry> objSectorlst = new List<Industry>();
            objSectorlst = _admindata.GetIndustry();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Standard Tag CRUD

        public ActionResult GetTagList()
        {
            List<Tag> objSectorlst = new List<Tag>();
            objSectorlst = _admindata.GetTagList();
            return Json(objSectorlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTag(Tag objTag)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteTag(objTag);

            if (Result == "1")
            {
                Result = "Standard Global Tag deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTagDetails(Tag objTag)
        {
            string Result = string.Empty;
            Result = _admindata.InsertTagDetails(objTag);

            if (Result == "1")
            {
                Result = "Standard Global Tag created successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateTagDetails(Tag objTag)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateTagDetails(objTag);

            if (Result == "1")
            {
                Result = "Standard Global Tag updated successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult ManageAdminUser()
        {
            return View();
        }
        public ActionResult ManageAllUser()
        {
            return View();
        }

        public ActionResult UserList(string RoleID)
        {
            List<RegisterUser> _userList = new List<RegisterUser>();
            _userList = GetUserList(RoleID);
            return PartialView("_ViewAllUserForAdmin", _userList);
        }
        public ActionResult UpdateUserExpireDateByAdmin(string date, string userId)
        {
            int result = 0;
            string msg = "";
            DateTime dateValue;
            if (DateTime.TryParseExact(date, "M/d/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out dateValue))
            {
                result = _userData.UpdateUserStatus(userId, Session["BPID"].ToString(), false, dateValue, "UpdateExpiredateByAdmin");
            }
            else
            {
                msg = "Date format not correct! Please use M/d/yyyy";
                result = -2;
            }

            return Json(new { resultValue = result, msgValue = msg }, JsonRequestBehavior.AllowGet);
        }
        public List<RegisterUser> GetUserList(string RoleID)
        {
            List<RegisterUser> _userList = new List<RegisterUser>();
            RegisterUser _registerUser = new RegisterUser();
            User user = new User();
            PersonalInformation personalInfo = new PersonalInformation();
            AddressIinformation addInfo = new AddressIinformation();
            BPInfo bPinfo = new BPInfo();
            DataSet ds = _userData.GetUserByRoleID(RoleID,"ByRoleID");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    user = new User
                    {
                        UserID = row["UserID"].ToString(),
                        ActiveDate = Convert.ToDateTime(row["ActiveDate"]),
                        ExpireDate = Convert.ToDateTime(row["ExpireDate"]),
                        EmailID = row["RegisterEmal"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"]),
                    };
                    personalInfo = new PersonalInformation
                    {
                        FirstName = row["FirstName"].ToString(),
                        AKA = row["AKA"].ToString(),
                        TeamMemberRole = row["TeamMemberRole"].ToString(),
                        Email = row["personalemail"].ToString(),
                        LastName = row["LastName"].ToString()
                    };
                    _registerUser = new RegisterUser();
                    _registerUser.User = user;
                    _registerUser.PersonalInfo = personalInfo;
                    _userList.Add(_registerUser);
                }
            }
            return _userList;
        }


        [HttpPost]
        public ActionResult MassUploadDataStandardTag()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                         r=_admindata.UploadStandardTag(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MassUploadDataSector()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadSector(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MassUploadDataCategory()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadCategory(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MassUploadDataIndustry()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadIndustry(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MassUploadDataCountry()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadCountry(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteMassData(string op)
        {
            string result = "Data delete";
            _admindata.DeleteMassData(op);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public DataTable GetAllRowDataFoMassUplod(string FilePath)
        {
            //Create a DataTable.
            DataTable dt = new DataTable();
          
            //Read the contents of CSV file.
            string csvData =System.IO.File.ReadAllText(FilePath);

            //Execute a loop over the rows.
            int count = 0;
            try
            {
                foreach (string row in csvData.Split('\n'))
                {

                    if (!string.IsNullOrEmpty(row))
                    {
                        string[] data = row.Split(';');

                        if (count == 0)
                        {
                            dt.Columns.Add("ID", typeof(Int32));
                            for (var iloop = 0; iloop < data.Length; iloop++)
                            {
                                dt.Columns.Add(data[iloop].ToString().Replace("\\", "").Replace(" ", "").Replace("\r", "").Trim());
                            }
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            dr[0] = 0;
                            for (var iloop = 0; iloop < data.Length; iloop++)
                            {
                                var incLoop = iloop + 1;
                                dr[incLoop] = data[iloop].ToString().Replace("\r", "").Trim();
                            }

                            dt.Rows.Add(dr);
                        }

                        count++;
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("AdminController-GetAllRowDataFoMassUplod", ex.Message);
            }
            return dt;
        }

        public ActionResult _CreateAdminUser()
        {
            RegisterUser registerUser = new RegisterUser();
            ViewData["Countrylst"] = new AccountController().GetCountry();
            ViewData["Languagelst"] = new AccountController().GetLanguage();
            return PartialView("_CreateAdminUser",registerUser);
        }

        public ActionResult GetAllPlan()
        {
            List<PlanDetails> lst = new List<PlanDetails>();
            DataSet ds = new DataSet();
            ds = _admindata.GetAllPlan("select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
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
                    lst.Add(planDetails);
                }
            }
            return View(lst);
        }


        public ActionResult CreatePlan(string title, string description, decimal cost,string group, string planfor)
        {
            int retValue = -1;
            string msg = "Error! while create plan.";
            retValue = _admindata.CreatePlan(title, description, "1 month free trial", cost,1, group, planfor, "insert");

            if (retValue >= 1)
                msg = "Plan create successfully";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDelete(string title="", string description="", decimal cost=0, int ID=0, bool status=false,int order=0,string op="")
        {
            int retValue = -1;
            string msg = "Error! while create plan.";
            retValue=_admindata.UpateDeletePlan(title, description, cost, ID, status, order,op);

            if (retValue >= 1)
                msg = "Plan updated successfully";
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Language()
        {
            return View();
        }
        public ActionResult AddLanguageDetails(LanguageTimeZone LanguageTimeZone)
        {
            string msg = "Data created successfully.";
            int result = -1;
            result = _admindata.CreateLanguage(LanguageTimeZone, "insert");
            if (result <= 0)
                msg = "Error in data or already in system.";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateLanguageDetails(LanguageTimeZone LanguageTimeZone)
        {
            string msg = "Data created successfully.";
            int result = -1;
            result = _admindata.CreateLanguage(LanguageTimeZone, "update");
            if (result <= 0)
                msg = "Error in data or already in system.";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLanguageList()
        {
            LanguageTimeZone languageTimeZone = new LanguageTimeZone();
            List<LanguageTimeZone> lst = new List<LanguageTimeZone>();
            DataSet ds = _admindata.GetLanguageList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    languageTimeZone = new LanguageTimeZone();
                    languageTimeZone.ID = Convert.ToInt32(row["ID"]);
                    languageTimeZone.Language = row["Language"].ToString();
                    languageTimeZone.LanguageDescription = row["LanguageDescription"].ToString();
                    languageTimeZone.Country = row["Country"].ToString();
                    languageTimeZone.Code = row["Code"].ToString();
                    lst.Add(languageTimeZone);
                }
            }
            return PartialView("_GetLanguage", lst);

        }

        public ActionResult DeleteLanguage(int ID)
        {
            string msg = "Data deleted successfully.";
            int result = -1;
            result = _admindata.DeleteLanguage(ID, "delete");
            if (result <= 0)
                msg = "Data not deleted!";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MassUploadDataLanguage()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadLanguage(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPaymentDetails()
        {

            List<PaymentModel> obj = new List<PaymentModel>();
            try
            {
                obj = _admindata.GetPaymentDetails("","all");
            }
            catch (Exception ex)
            {
                DataLogger.Write("Admin-PaymentModel", ex.Message);
            }
            //return Json(obj, JsonRequestBehavior.AllowGet);
            return View("GetPaymentDetails", obj);

        }
        public ActionResult GetAllTierPlan()
        {
            return View();
        }

        public ActionResult GetTierPlanList()
        {
            List<TierDetails> objTierlst = new List<TierDetails>();
            objTierlst = _admindata.GetTierList();
            return Json(objTierlst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTier(TierDetails objTier)
        {
            string Result = string.Empty;
            Result = _admindata.DeleteTier(objTier);

            if (Result == "2")
            {
                Result = "Tier plan deleted successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTierDetails(TierDetails objTier)
        {
            string Result = string.Empty;
            Result = _admindata.InsertTierDetails(objTier);

            if (Result == "2")
            {
                Result = "Sector created successfully";
            }


            return Json(Result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult UpdateTierDetails(TierDetails objTier)
        {
            string Result = string.Empty;
            Result = _admindata.UpdateTierDetails(objTier);

            if (Result == "2")
            {
                Result = "Sector updated successfully";
            }

            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MassUploadDataTier()
        {
            int r = 0;
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            if (Request.Files.Count > 0)
            {
                Fileupload = Request.Files[0];
                var supportedTypes = new[] { "txt", "csv" };
                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                if (supportedTypes.Contains(fileExt))
                {
                    string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + Path.GetFileName(Fileupload.FileName);
                    Fileupload.SaveAs(sourcePathToWrite);
                    // Start process to store in DB
                    DataTable dt = GetAllRowDataFoMassUplod(sourcePathToWrite);
                    if (dt.Rows.Count > 0)
                    {
                        //start to save in DB
                        r = _admindata.UploadTier(dt);
                        if (r > 0)
                            result = "Data Uploaded!";
                        else
                            result = "Data not uploaded, Something wrong with your data.";
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }

                    // Delete File After process
                    FileInfo file = new FileInfo(sourcePathToWrite);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                else
                {
                    result = "Data format not correct user semicolon(;) ";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}