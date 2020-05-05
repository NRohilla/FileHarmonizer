using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Harmonizer.UI.Controllers
{
    public class ReportController : Controller
    {
        FHFileData _fhFileData = new FHFileData();
        // GET: Report
        public ActionResult Index()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        public ActionResult CostOfOwnershipList()
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<CostOfOwnership> lstCostOfOwnership = _fhFileData.GetCostOfOwnershipDetails(FHNumber);
            return PartialView("_GetCostOfOwnershipList", lstCostOfOwnership);
        }

        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }
    }
}