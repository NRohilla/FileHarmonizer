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
        Report _ReportData = new Report();
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
            List<CostOfOwnership> lstCostOfOwnership = _ReportData.GetCostOfOwnershipDetails(FHNumber);
            return PartialView("_GetCostOfOwnershipList", lstCostOfOwnership);
        }

        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }

        public ActionResult AssociationReport()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        public ActionResult UserList()
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<Association> lstassociations = _ReportData.GetAssociation(FHNumber);

            return PartialView("_GetAssociationList", lstassociations);
        }

        public ActionResult  ViewDetailsAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_ViewDetailsAssociation");
        }
         
    }
}