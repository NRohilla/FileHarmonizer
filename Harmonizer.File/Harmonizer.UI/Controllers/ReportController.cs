using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using Harmonizer.UI.Models;
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
        List<string> lstUsers = new List<string>();

        // GET: Report
        public ActionResult Index()
        {
            ////-Nitin Check for expiry of account
            if (Session["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(Session["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        public ActionResult CostOfOwnershipList()
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<CostOfOwnership> lstCostOfOwnership = new List<CostOfOwnership>();
            if (FHNumber != "None")
            {
                lstCostOfOwnership = _ReportData.GetCostOfOwnershipDetails(FHNumber).Where(p => p.FHnumber == FHNumber).ToList();

            }
            else
            {
                lstCostOfOwnership = _ReportData.GetCostOfOwnershipDetails(FHNumber);

            }
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
            if (Session["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(Session["expiredate"]).ToShortDateString();
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
                if (!lstUsers.Contains(item.FHName))
                    lstUsers.Add(item.FHName);
                else if (!lstUsers.Contains(item.AssociateName))
                    lstUsers.Add(item.AssociateName);
            }
            ViewData["user"] = lstUsers;
            return PartialView("_GetAssociationList", lstassociations);
        }

        [HttpPost]
        public ActionResult UserList(FilterModel model)
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<Association> lstassociations = new List<Association>();
            if (FHNumber != "None")
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
            List<Association> filtredListassociations = new List<Association>();
            if (model.filterBy== "AssociateName" && model.filterKeyword!=null && model.filterKeyword!="")
            {
                foreach (var item in lstassociations)
                {
                    if (item.AssociateName.Contains(model.filterKeyword))
                    {
                        filtredListassociations.Add(item);
                    }
                }
               
            }
            else if(model.filterKeyword != null && model.filterKeyword != "")
            {
                foreach (var item in lstassociations)
                {
                    if (item.FHName.Contains(model.filterKeyword))
                    {
                        filtredListassociations.Add(item);
                    }
                }
            }
          //  return filtredListassociations;
            return PartialView("_GetAssociationList", filtredListassociations);
        }

        public ActionResult  ViewDetailsAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_ViewDetailsAssociation");
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            List<string> filtredUsers = new List<string>();
            var users = _ReportData.GetUsers();
           
            if (prefix != "")
            {

                foreach (var user in users){
                    if (user.StartsWith(prefix))
                        filtredUsers.Add(user);
                }
                return Json(filtredUsers);
                //   foreach (var item in lstassociations)
                //{

                //    if (item.StartsWith(prefix) )
                //        associateNames.Add(item);
                //}






            }
            else
            {
                return Json(users);
            }
          
        }
    }
}