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
        // GET: Association
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserList()
        {
            Association lstassociations1 = new Association()
            {
                FHnumber=  "1428321551",
                Associate = "1428321551",
                AssocStatus = true,
                OriginalDateofAssoc=Convert.ToDateTime("01-Apr-2020"),
                AssocCanceledDate= Convert.ToDateTime("01-Apr-2020"),
                AssocCanceledBy="Smit"

            };
            List<Association> lstassociations = new List<Association>();
            lstassociations.Add(lstassociations1);

            //lstassociations.Add(new Association("1428321551", "1428321551", 1,"01-Apr-2020", "01-Apr-2020", "Smit"));
            return PartialView("_GetAssociationList", lstassociations);
        }
    }
}