using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using Harmonizer.UI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using oDocP = DocumentFormat.OpenXml.Packaging;
using oDocW = DocumentFormat.OpenXml.Wordprocessing;
using oDocX = DocumentFormat.OpenXml;
using oDocS = DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Packaging;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using oDocPP = DocumentFormat.OpenXml.Presentation;
using System.Xml.Linq;
using System.Xml;
using oDocPwoerTool = OpenXmlPowerTools;
using OpenXmlPowerTools;
using System.IO;
using System.Net;
using System.Configuration;

namespace Harmonizer.UI.Controllers
{
    [GeneralExceptionHandlerAttribute]
    [SessionTimeoutFilter]
    public class FHManageController : Controller
    {
        FHManage _fhManage = new FHManage();
        FHFileData _fhFileData = new FHFileData();
        Scheme _scheme = new Scheme();
        // GET: FHManage
        public ActionResult ManageFilter()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View();
        }

        public ActionResult _FilterFileDetails()
        {

            List<SelectListItem> lstSchme = new List<SelectListItem>();
            DataSet dsScheme = _scheme.GetAllSchemeByBPID(Session["BPID"].ToString(), "selectall");
            lstSchme.Add(new SelectListItem { Text = "-Scheme-", Value = "0" });
            if (dsScheme.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsScheme.Tables[0].Rows.Count; i++)
                {
                    lstSchme.Add(new SelectListItem { Text = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString()+"-"+ dsScheme.Tables[0].Rows[i]["SchemeName"].ToString(), Value = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() });
                }
            }

            ViewData["lstSchme"] = lstSchme;


            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            List<ManageFilterTemplateModel> lstFilterTemplateNew = new List<ManageFilterTemplateModel>();
            Template _template = new Template();
            CHFilter _chFilter = new CHFilter();
            HarmonizeTemplate _harmonizeTemplate = new HarmonizeTemplate();
            FileUploadDownload _fileUploadDownload = new FileUploadDownload();
            string UserID = Session["UserID"].ToString();
            string BPID = Session["BPID"].ToString();
            DataSet ds= _fhManage.GetFilterByBPID(BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // Add Filter data
                    _chFilter = new CHFilter
                    {
                        SECID = row["SECID"].ToString(),
                        FLTRNUM = row["FLTRNUM"].ToString(),
                        FLTRID = row["FLTRID"].ToString(),
                        FilterDesc = row["FilterDesc"].ToString(),
                        FilterText = row["FilterText"].ToString(),
                        FilterName = row["FilterName"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                        IsDeleted=Convert.ToBoolean(row["IsDeleted"]),
                        FileID=Convert.ToInt32(row["FileID"]),
                        AssignScheme=Convert.ToInt32(row["AssignScheme"]),
                        IsArchive=Convert.ToBoolean(row["CFilterIsArchive"])
                    };
                   
                    // Main file to Add list of element
                    lstFilterTemplate.Add(new ManageFilterTemplateModel
                    {
                        Template = _template,
                        Filter = _chFilter,
                        FileUploadDownload = _fileUploadDownload,
                        HarmonizeTemplate = _harmonizeTemplate
                    });
                }
            }
          var datat=  lstFilterTemplate.Where(x => x.Filter.IsDeleted == false && x.Filter.IsArchive==false).ToList();

            // lst = lstSword.Distinct(new ItemEqualityComparer()).ToList();
            lstFilterTemplateNew = datat.Distinct(new NoDuplicateFilter()).ToList(); ;
            return PartialView(lstFilterTemplateNew);
        }

        public ActionResult ExportFilter()
        {
            return View();
        }

        public  ActionResult HarmonizeTemplate()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View();
        }

        public ActionResult _ManageHarmonizeTemplate(string FHNumber = "")
        {
			//added smit
			Session["Associate"] = FHNumber;
            string UserID = Session["UserID"].ToString();
            List<SelectListItem> lstSchme = new List<SelectListItem>();
            DataSet dsScheme = _scheme.GetAllSchemeByBPID(Session["BPID"].ToString(), "selectall");
            lstSchme.Add(new SelectListItem { Text = "-Scheme-", Value = "0" });
            if (dsScheme.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsScheme.Tables[0].Rows.Count; i++)
                {
                    lstSchme.Add(new SelectListItem { Text = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() + "-" + dsScheme.Tables[0].Rows[i]["SchemeName"].ToString(), Value = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() });
                }
            }

            ViewData["lstSchme"] = lstSchme;

            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            string BPID = Session["BPID"].ToString();
            if (FHNumber == "")
            {
                lstFilterTemplate = GetFiletTemplateManageData(BPID).Where(x => x.HarmonizeTemplate.TemplatePath != "" && !string.IsNullOrWhiteSpace(x.HarmonizeTemplate.TemplatePath) && x.HarmonizeTemplate.ID != 0 && x.HarmonizeTemplate.IsArchive==false).ToList();
            }
            else
            {
                lstFilterTemplate = GetFiletTemplateManageData(BPID).Where(x => x.HarmonizeTemplate.TemplatePath != "" && !string.IsNullOrWhiteSpace(x.HarmonizeTemplate.TemplatePath) && x.HarmonizeTemplate.ID != 0).ToList();
                lstFilterTemplate = lstFilterTemplate.Where(x => x.HarmonizeTemplate.HTFHNumber == FHNumber && x.HarmonizeTemplate.IsArchive == false).ToList();
            }
            return PartialView(lstFilterTemplate);
        }


        public ActionResult ManageTemplate()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View();
        }

        public ActionResult ViewFile(string FilePath)
        {
            ViewBag.FilePath = FilePath;
            return PartialView("_ViewFile");
        }

       

        public ActionResult RenameTemplate(int FileID, string TemplateText, string Description, string HFLTRID, string IE, int op = 0)
        {
            int retValue = 0;
            Template _template = new Template();
            _template.FileID=FileID;
            _template.TemplateDesc = Description;
            _template.TemplateText = TemplateText;
            _template.HFLTRID = HFLTRID;
            _template.InternalExternal = IE;
            if (op == 1)
            {
                // Update Template
                retValue = _fhManage.UpdateTemplateComment(_template);
                retValue = 1;
                return Json(retValue, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_RenameTemplate", _template);
            }
        }


        public ActionResult _TemplateFileDetails()
        {
            List<SelectListItem> lstSchme = new List<SelectListItem>();
            DataSet dsScheme = _scheme.GetAllSchemeByBPID(Session["BPID"].ToString(), "selectall");
            lstSchme.Add(new SelectListItem { Text = "-Scheme-", Value = "0" });
            if (dsScheme.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsScheme.Tables[0].Rows.Count; i++)
                {
                    lstSchme.Add(new SelectListItem { Text = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() + "-" + dsScheme.Tables[0].Rows[i]["SchemeName"].ToString(), Value = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() });
                }
            }

            ViewData["lstSchme"] = lstSchme;

            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            string UserID = Session["UserID"].ToString();
            string BPID = Session["BPID"].ToString();
            var lstFilterTemplateNew = GetFiletTemplateManageData(BPID).Where(x=>x.Template.IsDeleted==false && x.Template.IsArchive==false).ToList();
            lstFilterTemplate = lstFilterTemplateNew.Distinct(new NoDuplicateTemplate()).ToList(); ;
            return PartialView(lstFilterTemplate);
        }

        [HttpPost]
        public ActionResult DeleteTemplate(int FileID)
        {
            string BPID = Session["BPID"].ToString();
            int retValue = 0;
            string FLTRID = "";
            retValue = _fhManage.DeleteFilter(FileID, BPID, FLTRID,"Template");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportTemplate()
        {
            return View();
        }
        public ActionResult ConvertFileFormat()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetOrignalFileteTagDetails(int FileID)
        {
            string BPID = Session["BPID"].ToString();

            List<Repository> lstTag = new List<Repository>();
            DataSet ds = _fhFileData.GetTemplateDetailById(FileID, BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    lstTag.Add(new Repository
                    {
                        
                        UserID = row["UserID"].ToString(),
                        UTAGID = row["UTAGID"].ToString(),
                        Tag = row["Tag"].ToString(),
                        Org = row["Orig"].ToString(),
                        GlobPri = row["GlobPri"].ToString(),
                        Description = row["Description"].ToString(),
                        SearchWord= row["OrgSearchWord"].ToString(),
                        Share = row["Share"].ToString(),
                        TemplateID= Convert.ToInt32(row["FileID"])
                    });
                }
                
            }
            return PartialView("_GetOrignalFileteTagDetails",lstTag);
        }

        public ActionResult RenameFilter(int FileID, string Comment,string Description,string FLTRID, int op=0)
        {
            int retValue = -1;
            CHFilter _chFilter = new CHFilter();
            _chFilter.FileID = FileID;
            _chFilter.FilterDesc = Description;
            _chFilter.FilterText = Comment;
            _chFilter.FLTRID = FLTRID;
            if (op == 1)
            {
                // Update Filter
                retValue = _fhManage.UpdateFilterComment(_chFilter);
                return Json(retValue, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return PartialView("_RenameFilter", _chFilter);
            }
        }

        public ActionResult GetOrignalFileteTagDetailsToPrint(List<int> FileIDList)
        {
            string BPID = Session["BPID"].ToString();

            List<Repository> lstTag = new List<Repository>();
            for (int i = 0; i < FileIDList.Count; i++)
            {
                DataSet ds = _fhFileData.GetTemplateDetailById(FileIDList[i], BPID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstTag.Add(new Repository
                        {
                            UserID = row["UserID"].ToString(),
                            UTAGID = row["UTAGID"].ToString(),
                            Tag = row["Tag"].ToString(),
                            Org = row["Orig"].ToString(),
                            GlobPri = row["GlobPri"].ToString(),
                            Description = row["Description"].ToString(),
                            SearchWord = row["OrgSearchWord"].ToString(),
                            Share = row["Share"].ToString(),
                            TemplateID = Convert.ToInt32(row["FileID"])
                        });
                    }

                }
            }
            return PartialView("_GetOrignalFileteTagDetails", lstTag);
        }


        public ActionResult _ArchiveTemplate()
        {
            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            string UserID = Session["UserID"].ToString();
            string BPID = Session["BPID"].ToString();
            lstFilterTemplate = GetFiletTemplateManageData(BPID).Where(x => x.Template.IsDeleted == false && x.Template.IsArchive==true).ToList();
            return PartialView("_ArchiveTemplate", lstFilterTemplate);
        }

        [HttpPost]
        public ActionResult ArchiveTemplate(List<int> lstFileID)
        {
            int retValue = 0;
            string BPID = Session["BPID"].ToString(); 
            string Op = "ArchiveTemplate";
            retValue = _fhManage.ArchiveTemplateFileAll(lstFileID, BPID, Op);
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RestoreDelatedTemplateFile(List<int> lstFileID)
        {
            int retValue = 0;
            string BPID = Session["BPID"].ToString(); 
            string Op = "RestoreTemplate";
            retValue = _fhManage.RestoreDelatedTemplateFile(lstFileID, BPID, Op);
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MergeTempateFile(List<int> lstFileID)
        {
            int retValue = 0;
            string BPID = Session["BPID"].ToString(); 
            string Op = "";
            retValue = _fhManage.MergeTempateFile(lstFileID, BPID, Op);
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteFileter(int FileID, string FLTRID="")
        {
            string BPID = Session["BPID"].ToString();
            int retValue = 0;
            retValue = _fhManage.DeleteFilter(FileID, BPID, FLTRID, "Filter");
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public List<ManageFilterTemplateModel> GetFiletTemplateManageData(string BPID)
        {
			//added Smit userid,Fhnumb,associate
            string UserID = Session["UserID"].ToString();
            string FHnumb = Session["FHnumber"].ToString();
            string Associate = Session["Associate"].ToString();
            string fullPathUrlTemplate = "";
            string fullPathUrlHarmonized = "";
            string host = Request.Url.Host;
            string port =Request.Url.Port.ToString();
            string rootDomain = ConfigurationManager.AppSettings["rootDomain"].ToString();
            if (!string.IsNullOrEmpty(port))
            {
                fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + ":" + port + "/Target/" + Session["BPID"] + "/";
                fullPathUrlHarmonized= rootDomain + host.TrimEnd('/') + ":" + port + "/Harmonized/" + Session["BPID"] + "/";
            }
            else
            {
                fullPathUrlTemplate = rootDomain + host.TrimEnd('/') +"/Target/" + Session["BPID"] + "/";
                fullPathUrlHarmonized = rootDomain + host.TrimEnd('/') + "/Harmonized/" + Session["BPID"] + "/";
            }
			//added Smit
			Association association = new Association();
            Template _template = new Template();
            CHFilter _chFilter = new CHFilter();
            HarmonizeTemplate _harmonizeTemplate = new HarmonizeTemplate();
            FileUploadDownload _fileUploadDownload = new FileUploadDownload();
            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            DataSet ds =_fhManage.GetFilterTemplateDetailsByBPID(BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
				//added smit 
				association.FHnumber = FHnumb;
                association.Associate = Associate;
                association.AssocStatus = true;
                association.AssocCanceledBy = UserID;
                int createAssociation = _fhFileData.CreateAssociation(association);
                string recordId1 = _fhFileData.GetAssociationInActiveId(FHnumb, Associate);
                if (recordId1 != "")
                {
                    Session["RecordId"] = recordId1;
                }
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    // Add template data
                    _template = new Template
                    {
                        BPID=row["BPID"].ToString(),
                        HFLTRID=row["HFLTRID"].ToString(),
                        TEMPID=row["TEMPID"].ToString(),
                        Partner=row["Partner"].ToString(),
                        TemplateType=row["TemplateType"].ToString(),
                        TemplateDesc=row["TemplateDesc"].ToString(),
                        TemplateName=row["TemplateName"].ToString(),
                        DocExt=row["DocExt"].ToString(),
                        CreatedDate=Convert.ToDateTime(row["TempCDate"]),
                        UpdatedDate = Convert.ToDateTime(row["TempUDate"]),
                        FileID=Convert.ToInt32(row["FileID"]),
                        SECCODE=row["TempSECCode"].ToString(),
                        TemplateText=row["TemplateText"].ToString(),
                        IsDeleted=Convert.ToBoolean( row["TempDeleted"]),
                        IsArchive = Convert.ToBoolean(row["TempArchive"]),
                        InternalExternal= row["InternalExternal"].ToString()
                    };
                    // Add Filter data
                    _chFilter = new CHFilter
                    {
                        SECID=row["CSECID"].ToString(),
                        FLTRNUM=row["CFILTERNUM"].ToString(),
                        FLTRID=row["CFLTERID"].ToString(),
                        FilterDesc=row["FilterDesc"].ToString(),
                        FilterText=row["FilterText"].ToString(),
                        FilterName=row["FilterName"].ToString(),
                        CreatedDate=Convert.ToDateTime(row["CFCDate"]),
                        UpdatedDate = Convert.ToDateTime(row["CFUDate"]),
                        AssignScheme=Convert.ToInt32(row["AssignScheme"])
                    };
                    // Add File history data
                    _fileUploadDownload = new FileUploadDownload
                    {
                        OrginalFileName = row["OrignalFileName"].ToString(),
                        SourceFilePath = row["SourceFilePath"].ToString(),
                        TargetFilePath = row["TargetFilePath"].ToString(),
                        IsDeleted = Convert.ToBoolean(row["FileDelete"]),
                        IsArchive = Convert.ToBoolean(row["FileArchive"]),
                        TemplateDownloadPath = fullPathUrlTemplate + row["NewFileName"].ToString()

                    };
                    _harmonizeTemplate = new HarmonizeTemplate
                    {
                        ID = Convert.ToInt32(row["HTID"]),
                        TemplateName = row["TemplateName"].ToString(),
                        TemplateID = Convert.ToInt32(row["HTTemplateID"]),
                        NoOfDownloads = Convert.ToInt32(row["NoOfDownloads"]),
                        CreatedDate = Convert.ToDateTime(row["HTCreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(row["HTUpdateDate"]),
                        TemplatePath = fullPathUrlHarmonized + row["TemplatePath"].ToString(),
                        Comment = row["HTComment"].ToString(),
                        HTFHNumber = row["HTFHNumber"].ToString(),
                        IsArchive = Convert.ToBoolean(row["HTIsArchive"])

                    };
                    // Main file to Add list of element
                    lstFilterTemplate.Add(new ManageFilterTemplateModel
                    {
                       Template= _template,
                       Filter= _chFilter,
                       FileUploadDownload=_fileUploadDownload,
                       HarmonizeTemplate=_harmonizeTemplate
                    });
                }
            }
            return lstFilterTemplate;
        }

        public ActionResult UpdateHarmonizeCommnet(int ID,string Comment)
        {
            int retValue = -1;
            retValue = _fhManage.UpdateHarmonizeCommnet(ID, Comment);
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateAssignScheme(int schemenum, string FLTRID)
        {
            int retValue = -1;
            retValue = _fhManage.UpdateAssignScheme(schemenum, FLTRID,Session["BPID"].ToString());
            return Json(retValue, JsonRequestBehavior.AllowGet);
        }

        ////-Nitin Check for expiry of account
        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }
    }
}