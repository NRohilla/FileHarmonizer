using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Harmonizer.UI.Controllers
{
    public class APIController : Controller
    {
        APIData _apiData = new APIData();
        // GET: API
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Signup()
        {
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetAPIkey()
        {
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            string APIkey = "";
            APIModel _apiModel = new APIModel();
            _apiModel.APIKey = Guid.NewGuid().ToString();
            string UserId = Session["UserID"].ToString();
            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string ExpireDate = ViewBag.ExpireDate;
            if (DateTime.Parse(ExpireDate) < DateTime.Parse(CurrentDate))
            {
                int i = _apiData.UpdateAPIKey(UserId, false, DateTime.Parse(ExpireDate));
                APIkey = _apiData.GetInActiveAPIKey(UserId);
            }
            else
            {
                int i = _apiData.InsertAPIKey(UserId, _apiModel.APIKey, true, DateTime.Parse(ExpireDate));
                APIkey = _apiData.GetActiveAPIKey(UserId);
            }
            return Json(APIkey, JsonRequestBehavior.AllowGet);
        }


    }
}