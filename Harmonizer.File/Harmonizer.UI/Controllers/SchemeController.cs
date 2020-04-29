using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Harmonizer.DB.Data;
using Harmonizer.Core.Model;
using Harmonizer.UI.App_Start;
using System.Data;
using Harmonizer.UI.Models;

namespace Harmonizer.UI.Controllers
{
    [SessionTimeoutFilter]
    [GeneralExceptionHandlerAttribute]
    public class SchemeController : Controller
    {
        Scheme _scheme = new Scheme();
        UserData _userData = new UserData();
        // GET: Scheme
        public ActionResult CreateScheme()
        {
            CommanUserData commanUserData = new CommanUserData();
            string BPID = Session["BPID"].ToString();
            long schemenum = _scheme.GetSchemeValue(BPID, "getschemevalue");
            ViewBag.scheme = schemenum;
            commanUserData = _userData.GetCommanData(Session["UserID"].ToString());
            return View(commanUserData);
        }

        [HttpPost]
        public ActionResult CreateNewScheme(UserScheme userScheme)
        {
            int retvalue = -1;
            retvalue=_scheme.CreateScheme(userScheme, "insert");
            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MaintainScheme()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        public ActionResult _MaintainSchemeData()
        {
            UserScheme userScheme = new UserScheme();
            DataSet ds = new DataSet();
            List<UserScheme> lst = new List<UserScheme>();
            ds=_scheme.GetAllSchemeByBPID(Session["BPID"].ToString(), "selectall");
            if (ds.Tables[0].Rows.Count > 0)
            {
                
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID =Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted =Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate =Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                    lst.Add(userScheme);
                }
            }
            return PartialView("_MaintainSchemeData", lst);
        }

        [HttpPost]
        public ActionResult UpdateSchemeCommnet(int ID, string Comment)
        {
            int retValue = -1;
            retValue = _scheme.UpdateComment(ID, Comment, "updatecommnet");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteScheme(int ID)
        {
            int retValue = -1;
            retValue = _scheme.DeleteOrArchiveScheme(ID, "delete");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ArchiveScheme(List<int> LstID)
        {
            int retValue = -1;
            //foreach(var d in LstID.ToList())
            //    retValue = _scheme.DeleteOrArchiveScheme(d, "archive");

            // LstID=SchemeNum
            foreach (var d in LstID.ToList())
                retValue = _scheme.ArchiveSchemeAndDependentFilter(d, Session["BPID"].ToString(), "archivescheme");

            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ViewScheme(int ID)
        {
            string BPID = Session["BPID"].ToString();
            SchemeDetails lstScheme = new SchemeDetails();
            DataSet ds = new DataSet();
            UserScheme userScheme = new UserScheme();
            
            ds = _scheme.GetScheme(ID, "select");

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID = Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                }
            }

            lstScheme.SchemeInfo = userScheme;
            lstScheme.SchemeDetail =GetSchemeDetailsByID(ID,BPID);

            return PartialView("_ViewScheme", lstScheme);
        }

        public List<UserScheme> GetSchemeDetailsByID(int ID, string BPID)
        {
            UserScheme userScheme = new UserScheme();
            List<UserScheme> lst = new List<UserScheme>();
            DataSet ds = _scheme.GetSchemeDetailsByID(ID, BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.FHNumber = dr["FHNumber"].ToString();
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.Sector = dr["SECID"].ToString();
                    userScheme.FilterName = dr["FilterName"].ToString();
                    userScheme.HarmonizeName = dr["HarmonizeName"].ToString();
                    userScheme.TemplateName = dr["TemplateName"].ToString();
                    lst.Add(userScheme);
                }
            }
            return lst;
        }

        [HttpPost]
        public ActionResult UpadateScheme(UserScheme userScheme)
        {
            int retvalue = -1;
            retvalue = _scheme.CreateScheme(userScheme, "updatescheme");
            return Json(retvalue, JsonRequestBehavior.AllowGet);
        }

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
        public ActionResult _ArchiveSchemeData()
        {
            UserScheme userScheme = new UserScheme();
            DataSet ds = new DataSet();
            List<UserScheme> lst = new List<UserScheme>();
            ds = _scheme.GetAllSchemeByBPID(Session["BPID"].ToString(), "getarchive");
            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID = Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                    lst.Add(userScheme);
                }
            }
            return PartialView("_ArchiveSchemeData", lst);
        }


        [HttpPost]
        public ActionResult UnArchiveScheme(int ID)
        {
            int retValue = -1;
            //retValue = _scheme.DeleteOrArchiveScheme(ID, "unarchive");
            retValue = _scheme.ArchiveSchemeAndDependentFilter(ID,Session["BPID"].ToString(), "unarchivescheme");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CheckBPIDExist(string BPID)
        {
            int retValue = -1;
            retValue = _scheme.CheckBPIDExist(BPID, "clientbpidexist");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        ////-Nitin Check for expiry of account
        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }
    }
}