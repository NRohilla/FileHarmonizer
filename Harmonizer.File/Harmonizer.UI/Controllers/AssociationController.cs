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

        public ActionResult RenameTemplate(string FHnumber, string Associate, bool Status, int op = 0)
        {
           
            return PartialView("_UpdateAssociation");
        }
    }
}