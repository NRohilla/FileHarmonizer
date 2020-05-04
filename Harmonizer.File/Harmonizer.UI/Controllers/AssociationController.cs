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
            return View();
        }
        public ActionResult UserList()
        {
            string FHNumber = Session["FHnumber"].ToString();
            List<Association> lstassociations = _fhFileData.GetAssociation(FHNumber);
            return PartialView("_GetAssociationList", lstassociations);
        }

        public ActionResult UpdateAssociation(string AssociationToRemove)
        {
            int UpdateAssocaition = _fhFileData.RemoveAssociation(Convert.ToInt32(AssociationToRemove));
            string message = "Some issue occured";

            if (UpdateAssocaition > 0)
                message = "Removed";

            return Json(message, JsonRequestBehavior.AllowGet); ;
        }



        public ActionResult RenameTemplate(string FHnumber, string Associate, string Status, int op = 0)
        {
            Association objModel = new Association();
            objModel.FHnumber = FHnumber;
            objModel.Associate = Associate;
            objModel.AssocStatus = (Status == "True" ? true : false);
            return PartialView("_UpdateAssociation", objModel);
        }

        public ActionResult DeactivateAssociation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_DeactivateAssociation");
        }
    }
}