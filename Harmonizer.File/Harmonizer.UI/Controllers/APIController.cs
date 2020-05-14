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
            return View();
        }

        [HttpGet]
        public JsonResult GetAPIkey()
        {
            APIModel _apiModel = new APIModel();
            _apiModel.API_Key = Guid.NewGuid().ToString();
            string UserId = Session["UserID"].ToString();
            int i = _apiData.InsertAPIKey(UserId, _apiModel.API_Key);
            string APIkey = _apiData.GetAPIKey(UserId);
            return Json(APIkey,JsonRequestBehavior.AllowGet);
        }
    }
}