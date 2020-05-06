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
            List<Association> lstassociations = new List<Association>();
            if (FHNumber!= "None" )
            {
                lstassociations = _ReportData.GetAssociation(FHNumber).Where(p => p.FHnumber == FHNumber).ToList(); 

            }
            else
            {
                 lstassociations = _ReportData.GetAssociation(FHNumber);

            }


            foreach (var item in lstassociations)
            {
                item.FHName = _fhFileData.GetUserid(item.FHnumber);
                item.AssociateName = _fhFileData.GetUserid(item.Associate);
            }
            return PartialView("_GetAssociationList", lstassociations);
        }

        public ActionResult  ViewDetailsAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_ViewDetailsAssociation");
        }
         
    }
}