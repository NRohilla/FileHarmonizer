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
    public class AssociationController : Controller
    {
        FHFileData _fhFileData = new FHFileData();
        // GET: Association
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

        public ActionResult UserList()
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<Association> lstassociations = new List<Association>();
            if (FHNumber != "None")
            {
               lstassociations =
                 _fhFileData.GetAssociation(FHNumber)
                 .Where(p => p.FHnumber == FHNumber).ToList();

            }
            else
            {
                 lstassociations =
                 _fhFileData.GetAssociation(FHNumber);
                

            }
          
            return PartialView("_GetAssociationList", lstassociations);
        }

        public ActionResult UpdateAssociation(string AssociationToRemove)
        {
            int UpdateAssocaition = _fhFileData.RemoveAssociation(Convert.ToInt32(AssociationToRemove));
            string message = "Some issue occured";

            if (UpdateAssocaition > 0)
                message = "Removed";

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateInActiveAssociation(string RecordId)
        {
            int UpdateAssocaition = _fhFileData.UpdateAssociation(Convert.ToInt32(RecordId));
            string message = "Some issue occured";

            if (UpdateAssocaition > 0)
                message = "Removed";
            Session.Remove("RecordId");
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ActiveAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UpdateAssociation");
        }

        public ActionResult DeactivateAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_DeactivateAssociation");
        }

        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }
    }
}