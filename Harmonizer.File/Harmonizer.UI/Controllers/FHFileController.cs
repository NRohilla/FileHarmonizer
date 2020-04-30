using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using Harmonizer.UI.Models;
using System.IO;
using System.Text.RegularExpressions;
using WORD = Microsoft.Office.Interop.Word;
using EXC = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;
using Harmonizer.UI.App_Start;
using System.Net;
using PPT = Microsoft.Office.Interop.PowerPoint;


// ********* Open Office
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
using System.Configuration;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace Harmonizer.UI.Controllers
{
    [GeneralExceptionHandlerAttribute]
    public class FHFileController : Controller
    {
        static iTextSharp.text.pdf.PdfStamper stamper = null;
        FHFileData _fileData = new FHFileData();
        UserData _userData = new UserData();
        FHManage _fhManage = new FHManage();
        AdminData _adminData = new AdminData();
        // GET: FHFile
        public ActionResult Index()
        {
            return View();
        }
        // Get tag list
        public ActionResult GetTagList()
        {
            List<SelectListItem> lstStandardGTag = new List<SelectListItem>();
            DataSet ds = _fileData.GetStandardGlobalTag();
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstStandardGTag.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["Tag"].ToString() + "_" + ds.Tables[0].Rows[i]["Description"].ToString(), Value = "<" + ds.Tables[0].Rows[i]["UTAGID"].ToString() + ">" });
                }
            }

            return Json(lstStandardGTag, JsonRequestBehavior.AllowGet);

        }


        [ActionName("FilterTemplate")]
        [SessionTimeoutFilter]
        public ActionResult SearchReplace()
        {
            List<SelectListItem> lstSectore = new List<SelectListItem>();

            // for sector
            DataSet dsSectore = _userData.GetSector();
            lstSectore.Add(new SelectListItem { Text = "Select Template Type", Value = "0" });
            if (dsSectore.Tables[0].Rows.Count > 0)

            {
                // Selected=(item == defaultCountry ? true : false) }
                for (int i = 0; i < dsSectore.Tables[0].Rows.Count; i++)
                {
                    lstSectore.Add(new SelectListItem { Text = dsSectore.Tables[0].Rows[i]["Description"].ToString(), Value = dsSectore.Tables[0].Rows[i]["SECID"].ToString(), Selected = dsSectore.Tables[0].Rows[i]["SECID"].ToString() == Session["SECID"].ToString() ? true : false });
                }
            }

            ViewData["lstSectore"] = lstSectore;

            ViewBag.message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View("SearchReplace");
        }


        [ValidateInput(false)]
        [SessionTimeoutFilter]
        [HttpPost]
        // public ActionResult UploadFileData(HttpPostedFileBase Fileupload, string SearchData, string Tag, string Instruction)
        public ActionResult UploadFileData()//(HttpPostedFileBase fileElementId, string title)
        {
            DataLogger.Write("FHFile-UploadFileData", "Call Upload function");
            FileUploadDownload fileUploadData = new FileUploadDownload();
            Template template = new Template();
            CHFilter filter = new CHFilter();
            CHFilter HFilter = new CHFilter();
            HttpPostedFileBase Fileupload = null;
            string msg = "File not uploaded! Please fill correct data.";
            int UploadDataInDB = -1;
            int StoreTagHistory = -1;
            int CreateTemplate = -1;
            string ReturnCFLTRID = "";
            string ReturnHFLTRID = "";
            string UnquicSECTAGCODE = "";
            string FileSucessToProcessed = "";
            List<SwordAndTagReplace> lstSearch = new List<SwordAndTagReplace>();
            try
            {
                lstSearch = RemoveDuplicateSearchText(JsonConvert.DeserializeObject<List<SwordAndTagReplace>>(Request.Form[0].ToString()));
                //RemoveDuplicateSearchText(lstSearch);

                UnquicSECTAGCODE = _fileData.GetTAGSECCode(Request.Form[2].ToString() == "0" ? "H" : Request.Form[2].ToString());
                template.TemplateType = Request.Form[1].ToString();
                // template.TemplateName = Request.Form[2].ToString();
                // template.TemplateDesc = Request.Form[3].ToString();
                template.SECID = Request.Form[2].ToString() == "0" ? "H" : Request.Form[2].ToString();
                template.SECCODE = UnquicSECTAGCODE;//_fileData.GetTAGSECCode(template.SECID);// TAG SEC ID form other table
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-UploadFileData", "First:-" + ex.Message);
                Logger.Write(Session["BPID"].ToString(), "List of search data not find:-" + ex.Message + "--" + ex.StackTrace);
            }
            try
            {

                if (Request.Files.Count > 0)
                {
                    int CreateHarmonizerRow = 0;
                    int CreateFilterRow = 0;
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        Fileupload = Request.Files[i];
                        if (Fileupload != null)
                        {

                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "csv" };
                            var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                            if (!supportedTypes.Contains(fileExt))
                            {
                                msg = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/CSV File";
                            }
                            else
                            {
                                // store source data  //Session["SECID"].ToString()  login sectorID
                                string IdentificationPath = Session["UserID"].ToString() + "_" + Request.Form[2].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                                string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                                string TargetFilePath = Server.MapPath("~").TrimEnd('\\') + "\\Target\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                                Fileupload.SaveAs(sourcePathToWrite);
                                // Store History in DB
                                fileUploadData.UserID = Session["UserID"].ToString();
                                fileUploadData.SECID = Request.Form[2].ToString();// Current selected sector ID
                                fileUploadData.BPID = Session["BPID"].ToString();
                                fileUploadData.DataDate = DateTime.Now;
                                fileUploadData.Time = DateTime.Now.ToShortTimeString();
                                fileUploadData.CreatedBy = Session["UserID"].ToString();
                                fileUploadData.CreatedDate = DateTime.Now;
                                fileUploadData.OrginalFileName = Fileupload.FileName;
                                fileUploadData.SourceFilePath = sourcePathToWrite;
                                fileUploadData.FileExt = fileExt.ToString();
                                fileUploadData.IsArchive = false;
                                fileUploadData.TargetFilePath = TargetFilePath;
                                fileUploadData.NewFileName = IdentificationPath + Fileupload.FileName;

                                // Get ID and store in other table for track history
                                UploadDataInDB = _fileData.UploadSourceFile(fileUploadData);
                                // store Tag/search data bulk
                                //StoreTagHistory = _fileData.StoreSearchReplaceTagHistory(GetOrignalCodeWithHTMLTag(lstSearch), UploadDataInDB);// comment 19-12-2018
                                StoreTagHistory = _fileData.StoreSearchReplaceTagHistory(GetOrignalCodeAfterRemoveHTMLTag(lstSearch), UploadDataInDB);
                                // Serarch and Replace data with tag ID as <10001>
                                // check file replaced or not if msg not = "" then track record 
                                FileSucessToProcessed = ReplaceDataWithList(lstSearch, TargetFilePath, fileExt, sourcePathToWrite);// comment 19-12-2018
                                //FileSucessToProcessed = ReplaceDataWithList(GetOrignalCodeAfterRemoveHTMLTag(lstSearch), TargetFilePath, fileExt, sourcePathToWrite);
                                if (FileSucessToProcessed == "" && string.IsNullOrEmpty(FileSucessToProcessed))
                                {
                                    // Create Filter
                                    if (CreateFilterRow == 0)
                                    {
                                        CreateFilterRow = 1;// for create single row
                                        filter.UserID = Session["UserID"].ToString();
                                        filter.SECID = Request.Form[2].ToString();
                                        filter.FileID = 0;
                                        filter.FilterDesc = Fileupload.FileName;// source file name
                                        filter.FilterText = "";// template.TemplateName;
                                        filter.FilterType = "C";
                                        filter.SECCODE = template.SECCODE;
                                        filter.BPID = Session["BPID"].ToString();
                                        ReturnCFLTRID = _fileData.CreateFilter(filter);
                                    }
                                    if (CreateHarmonizerRow == 0)
                                    {
                                        CreateHarmonizerRow = 1;
                                        // Create Harmonizer Filete
                                        HFilter.UserID = Session["UserID"].ToString();
                                        HFilter.BPID = Session["BPID"].ToString();
                                        HFilter.SECID = Request.Form[2].ToString();
                                        HFilter.CFLTRID = ReturnCFLTRID;// return id from create filter 
                                        HFilter.FileID = 0;
                                        HFilter.FilterDesc = "";
                                        HFilter.FilterType = "H";
                                        HFilter.SECCODE = template.SECCODE;
                                        ReturnHFLTRID = _fileData.CreateHarmonizerFilter(HFilter);
                                    }
                                    // Create Template
                                    template.FileID = UploadDataInDB;
                                    template.HFLTRID = ReturnHFLTRID;// return id from harmonizer filter
                                    template.BPID = Session["BPID"].ToString();
                                    template.DocExt = fileExt.ToString();
                                    template.UserID = Session["UserID"].ToString();
                                    template.Partner = Session["Partner"].ToString();// Patrer comes from BP Info table on BPID as BPType -- Cutomer/Indivisul/Vendor.
                                    template.FilterType = "H";// should be calculate on behalf of RoleID if RoleID as Admin then H else C (Need discuss)
                                    template.TemplateDesc = Fileupload.FileName;

                                    CreateTemplate = _fileData.CreateTemplate(template);

                                }
                                if (FileSucessToProcessed == "")
                                    msg = "File uploaded successfully";
                                else
                                    msg = FileSucessToProcessed;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-UploadFileData", "Second:-" + ex.Message);
                Logger.Write(Session["BPID"].ToString(), "File can not upload from client" + ex.Message + "--" + ex.StackTrace);
            }

            TempData["Message"] = msg;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public List<SwordAndTagReplace> RemoveDuplicateSearchText(List<SwordAndTagReplace> lstSword)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();
            //lst = lstSword.Distinct().ToList();// not working for collection as class
            lst = lstSword.Distinct(new ItemEqualityComparer()).ToList();
            return lst;
        }


        [SessionTimeoutFilter]
        public ActionResult Individual()
        {
            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }
            return View();
        }

        [NonAction]
        public string Replcacedata(string ReplaceTagName, string SearchWord, string TargetFilePath, string FileExt)
        {
            return "";
        }


        public List<SwordAndTagReplace> GetOrignalCodeWithHTMLTag(List<SwordAndTagReplace> lstSearch)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                lst.Add(new SwordAndTagReplace
                {
                    ID = item.ID,
                    SearchWord = item.SearchWord,
                    TagName = item.TagName.Replace("&lt;", "<").Replace("&gt;", ">"),
                    Instruction = item.Instruction
                });
            }

            return lst;

        }

        public List<SwordAndTagReplace> GetOrignalCodeWithHTMLTagForSearch(List<SwordAndTagReplace> lstSearch)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                lst.Add(new SwordAndTagReplace
                {
                    ID = item.ID,
                    SearchWord = item.SearchWord.Replace("&lt;", "<").Replace("&gt;", ">"),
                    TagName = string.IsNullOrEmpty(item.TagName) == true ? "..." : item.TagName,
                    Instruction = item.Instruction
                });
            }

            return lst;

        }

        public List<SwordAndTagReplace> GetTagForSearchWithWT(List<SwordAndTagReplace> lstSearch, string op = "")
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();
            if (op.ToLower() == "both".ToLower())
            {
                foreach (var item in lstSearch)
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        //<w:t>&lt;</w:t></w:r><w:r><w:rPr><w:sz w:val="24"/><w:szCs w:val="24"/></w:rPr><w:t>1000241</w:t></w:r><w:r><w:rPr><w:sz w:val="24"/><w:szCs w:val="24"/></w:rPr><w:t>&gt;</w:t>
                        ID = item.ID,
                        SearchWord = item.SearchWord.Replace("&lt;", "<w:t>").Replace("&gt;", "</w:t>"),
                        TagName = "<w:t>" + item.TagName + "</w:t>",
                        Instruction = item.Instruction
                    });
                }
            }
            else if (op.ToLower() == "greater".ToLower())
            {
                foreach (var item in lstSearch)
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        //<w:t>1000003&gt;</w:t>
                        ID = item.ID,
                        SearchWord = "<w:t>" + item.SearchWord.Replace("&lt;", "").Replace("&gt;", "") + "&gt;</w:t>",
                        TagName = "<w:t>" + item.TagName + "</w:t>",
                        Instruction = item.Instruction
                    });
                }
            }
            else if (op.ToLower() == "less".ToLower())
            {
                foreach (var item in lstSearch)
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        //<w:t>&lt;1000003</w:t>
                        ID = item.ID,
                        SearchWord = "<w:t>&lt;" + item.SearchWord.Replace("&lt;", "").Replace("&gt;", "") + "</w:t>",
                        TagName = "<w:t>" + item.TagName + "</w:t>",
                        Instruction = item.Instruction
                    });
                }
            }

            return lst;

        }



        public List<SwordAndTagReplace> GetOrignalCodeAfterRemoveHTMLTag(List<SwordAndTagReplace> lstSearch)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                lst.Add(new SwordAndTagReplace
                {
                    ID = item.ID,
                    SearchWord = item.SearchWord,
                    TagName = item.TagName.Replace("&lt;", "").Replace("&gt;", "").Replace("<", "").Replace(">", ""),
                    Instruction = item.Instruction
                });
            }

            return lst;

        }

        public List<SwordAndTagReplace> GetOrignalCodeAfterRemoveHTMLTagFormSearchWord(List<SwordAndTagReplace> lstSearch)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                lst.Add(new SwordAndTagReplace
                {
                    ID = item.ID,
                    SearchWord = item.SearchWord.Replace("&lt;", "").Replace("&gt;", "").Replace("<", "").Replace(">", ""),
                    TagName = item.TagName.Replace("&lt;", "").Replace("&gt;", "").Replace("<", "").Replace(">", ""),
                    Instruction = item.Instruction
                });
            }

            return lst;
        }

        public List<SwordAndTagReplace> RemoveSpecialCharWithEscap(List<SwordAndTagReplace> lstSearch)
        {
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                lst.Add(new SwordAndTagReplace
                {
                    ID = item.ID,
                    SearchWord = item.SearchWord.Replace("(", "\\(").Replace(")", "\\)"),
                    TagName = item.TagName,
                    Instruction = item.Instruction
                });
            }

            return lst;
        }

        [NonAction]
        public string ReplaceDataWithList(List<SwordAndTagReplace> lstSearchData, string TargetFilePath, string FileExt, string SourceFilePath, bool IsHarmonized = false)
        {

            //List<SwordAndTagReplace> lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);//22/1/2019
            List<SwordAndTagReplace> lstSearch = lstSearchData;

            int NoOfOccurence = 0;
            string msg = "";
            if (FileExt == ".txt" || FileExt == "txt") // For txt file
            {
                // add new line 22/1/2019
                if (IsHarmonized)
                    lstSearch = GetOrignalCodeWithHTMLTagForSearch(lstSearchData);
                else
                    lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);

                StreamReader reader = new StreamReader(SourceFilePath);
                string source = reader.ReadToEnd();
                reader.Close();

                foreach (var srdata in lstSearch)
                {
                    if (srdata.TagName == "" || srdata.TagName == null)
                        continue;

                    try
                    {
                        var ignorecase = (0 == MsoTriState.msoTrue) ? RegexOptions.Multiline : RegexOptions.IgnoreCase | RegexOptions.Multiline;
                        if (IsHarmonized == false)
                        {
                            source = Regex.Replace(source, @"\b" + srdata.SearchWord + "\\b", (match) =>
                                {
                                    NoOfOccurence += 1;
                                    return match.Result(srdata.TagName);
                                }, ignorecase);
                        }
                        else
                        {
                            source = Regex.Replace(source, srdata.SearchWord, (match) =>
                           {
                               NoOfOccurence += 1;
                               return match.Result(srdata.TagName);
                           }, ignorecase);
                        }
                    }
                    catch (Exception ex)
                    {
                        // logger here
                        msg = "Failed to open";
                    }
                }

                StreamWriter writer = new StreamWriter(TargetFilePath);
                writer.Write(source);
                writer.Close();


            }
            else if (FileExt == ".csv" || FileExt == "csv")// work with csv file not space only saparate with comma(,)
            {
                StreamReader reader = new StreamReader(SourceFilePath);
                // add new line 22/1/2019
                if (IsHarmonized)
                    lstSearch = GetOrignalCodeWithHTMLTagForSearch(lstSearchData);
                else
                    lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);

                string source = reader.ReadToEnd();
                reader.Close();

                foreach (var srdata in lstSearch)
                {
                    if (srdata.TagName == "" || srdata.TagName == null)
                        continue;

                    try
                    {
                        var ignorecase = (0 == MsoTriState.msoTrue) ? RegexOptions.Multiline : RegexOptions.IgnoreCase | RegexOptions.Multiline;
                        source = Regex.Replace(source, srdata.SearchWord, (match) =>
                        {
                            NoOfOccurence += 1;
                            return match.Result(srdata.TagName);
                        }, ignorecase);
                    }
                    catch (Exception ex)
                    {
                        // logger here
                        msg = "Failed to open";
                    }
                }

                StreamWriter writer = new StreamWriter(TargetFilePath);
                writer.Write(source);
                writer.Close();
            }
            else if (FileExt == ".doc" || FileExt == ".docx" || FileExt == "doc" || FileExt == "docx")// for doc file
            {

                //WORD.Application pApp = OfficeAppManager.WordApp();
                //WORD.Document pDoc = null;

                //object yes = true;
                //object no = false;
                //object missing = System.Reflection.Missing.Value;
                //try
                //{
                //    object readOnly = false;
                //    object isVisible = false;
                //    pDoc = pApp.Documents.Open(SourceFilePath, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible);
                //    pDoc.Saved = true;
                //}
                //catch (Exception ex)
                //{
                //    // logger here
                //    msg = "Failed to open doc file";
                //    Logger.Write(Session["BPID"].ToString(), msg + ":" + ex.StackTrace.ToString());
                //    return msg;
                //}


                //pDoc.Content.Select();

                //object replaceAll = WORD.WdReplace.wdReplaceAll;


                //try
                //{
                //    foreach (var srdata in lstSearch)
                //    {
                //        object matchcase = MsoTriState.msoFalse;
                //        object wholephrase = MsoTriState.msoFalse;
                //        //replace

                //        if (pApp.Selection.Find.Execute(srdata.SearchWord, ref matchcase, ref wholephrase, ref no, ref no, ref no, ref yes,
                //                                ref missing, ref missing, srdata.TagName, ref replaceAll,
                //                                ref missing, ref yes, ref missing, ref missing))
                //        {
                //            NoOfOccurence += 1;
                //        }
                //    }
                //    pDoc.SaveAs2(TargetFilePath);
                //    pDoc.Close();
                //}
                //catch (Exception ex)
                //{

                //    msg = "Failed to create filter template.";
                //    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                //}
                //finally
                //{
                //    OfficeAppManager.ReleaseObject(pDoc);
                //    OfficeAppManager.CloseWordApp();
                //    //pApp.Quit();
                //    //pApp = null;
                //}


                // ******* OpenXML for Word File
                if (System.IO.File.Exists(TargetFilePath))
                {
                    // Delete then create new file
                    System.IO.File.Delete(TargetFilePath);
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }
                else
                {
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }
                try
                {
                    if (!IsHarmonized)
                    {
                        using (oDocP.WordprocessingDocument wordDoc = oDocP.WordprocessingDocument.Open(TargetFilePath, true))
                        {
                            string docText = null;
                            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                            {
                                docText = sr.ReadToEnd();
                            }
                            foreach (var srdata in RemoveSpecialCharWithEscap(lstSearch))// Replace special charector with escape slash
                            {
                                //Regex regexText = new Regex(srdata.SearchWord);
                                //docText = regexText.Replace(docText, srdata.TagName);
                                var ignorecase = RegexOptions.IgnoreCase;
                                if (true)
                                {
                                    docText = Regex.Replace(docText, srdata.SearchWord, (match) =>
                                     {
                                         NoOfOccurence += 1;
                                         return match.Result(srdata.TagName);
                                     }, ignorecase);
                                }
                            }

                            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                            {
                                sw.Write(docText);

                            }
                        }
                    }

                    else if (IsHarmonized)
                    {
                        using (oDocP.WordprocessingDocument wordDoc = oDocP.WordprocessingDocument.Open(TargetFilePath, true))
                        {
                            foreach (var data in GetOrignalCodeWithHTMLTagForSearch(lstSearch))
                            {
                                try
                                {
                                    SearchAndReplacer.SearchAndReplace(wordDoc, data.SearchWord, data.TagName, true);
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                }


            }
            else if (FileExt == ".ppt" || FileExt == ".pptx" || FileExt == "ppt" || FileExt == "pptx")
            {

                //PPT.Application _pApp = OfficeAppManager.PowerPointApp(0);
                //PPT.Presentation _pPres = null;
                //try
                //{
                //    _pPres = _pApp.Presentations.Open(SourceFilePath, MsoTriState.msoFalse,
                //        MsoTriState.msoTrue, MsoTriState.msoFalse);
                //}
                //catch (Exception ex)
                //{
                //    // logger here
                //    msg = "Failed to open ppt file";
                //    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                //    return msg;
                //}

                //int slideCount = GetSlideCount(_pPres);


                //for (int pos = 1; pos <= slideCount; pos++)
                //{
                //    PPT.Shapes sld = GetSlide(_pPres, pos, false);

                //    // loop trough all the Shapes
                //    SearchReplaceShapes(sld, lstSearch, 0);

                //    PPT.Shapes notes = GetSlide(_pPres, pos, true);

                //    // loop trough all the Notes
                //    SearchReplaceShapes(notes, lstSearch, 0);

                //    OfficeAppManager.ReleaseObject(sld);
                //    OfficeAppManager.ReleaseObject(notes);
                //}

                //try
                //{
                //    _pPres.RemovePersonalInformation = MsoTriState.msoTrue;
                //    _pPres.RemoveDocumentInformation(PPT.PpRemoveDocInfoType.ppRDIAll);
                //}
                //catch (Exception) { }

                //PPT.PpSaveAsFileType saveastype = (".ppt" == Path.GetExtension(SourceFilePath).ToLower())
                //                          ? PPT.PpSaveAsFileType.ppSaveAsPresentation
                //                          : PPT.PpSaveAsFileType.ppSaveAsDefault;

                //var newfile = TargetFilePath;
                //_pPres.SaveAs(newfile, saveastype, MsoTriState.msoFalse);

                //_pPres.Saved = MsoTriState.msoTrue;
                //_pPres.Close();
                //OfficeAppManager.ReleaseObject(_pPres);
                //OfficeAppManager.ClosePowerpointApp();
                //_pApp = null;

                // ******* OpenXML for PPT File
                if (System.IO.File.Exists(TargetFilePath))
                {
                    // Delete then create new file
                    System.IO.File.Delete(TargetFilePath);
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }
                else
                {
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }

                try
                {
                    if (IsHarmonized)
                        lstSearch = GetOrignalCodeWithHTMLTagForSearch(lstSearchData);
                    else
                        lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);

                    using (oDocP.PresentationDocument presentationDocument = oDocP.PresentationDocument.Open(TargetFilePath, true))
                    {
                        int slideCount = GetCountOfSlides(presentationDocument);
                        for (int slidId = 0; slidId < slideCount; slidId++)
                        {
                            GetAllTextInSlide(presentationDocument, slidId, lstSearch);
                        }

                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                }

                return msg;
            }
            else if (FileExt == ".xls" || FileExt == ".xlsx" || FileExt == "xls" || FileExt == "xlsx")
            {
                //object m = Type.Missing;
                //EXC.Application excelpApp = OfficeAppManager.ExcelApp();
                //EXC.Workbook theWorkbook = null;
                //try
                //{
                //    theWorkbook = excelpApp.Workbooks.Open(SourceFilePath, 0, true, 5, string.Empty,
                //        string.Empty, true, EXC.XlPlatform.xlWindows, "\t", false, false, 0, true);
                //}
                //catch(Exception ex)
                //{
                //    // logger info here

                //    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                //}
                //if (theWorkbook == null)
                //{
                //    msg = "File not open xls";
                //    OfficeAppManager.CloseExcelApp();
                //    Logger.Write(Session["BPID"].ToString(), msg);
                //    return msg;
                //}

                //EXC.Sheets sheets = theWorkbook.Worksheets;
                //int sheetCount = sheets.Count;
                //for (int i = 0; i < sheetCount; i++)
                //{

                //    //EXC.Range uRng;
                //    EXC.Worksheet worksheet = (EXC.Worksheet)sheets.get_Item(i + 1);

                //    SearchReplaceSheet(lstSearch, worksheet);

                //    OfficeAppManager.ReleaseObject(worksheet);
                //}

                //OfficeAppManager.ReleaseObject(sheets);

                //theWorkbook.CheckCompatibility = false;
                //try
                //{
                //    theWorkbook.RemovePersonalInformation = true;
                //    theWorkbook.RemoveDocumentInformation(EXC.XlRemoveDocInfoType.xlRDIAll);
                //}
                //catch (Exception ex)
                //{
                //    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                //}

                //var newfile = TargetFilePath;
                //theWorkbook.SaveAs(newfile,
                //    theWorkbook.FileFormat,
                //    m,
                //    m,
                //    m,
                //    m,
                //    EXC.XlSaveAsAccessMode.xlNoChange,
                //    EXC.XlSaveConflictResolution.xlLocalSessionChanges,
                //    m, m, m, m);

                //theWorkbook.Close();
                //OfficeAppManager.ReleaseObject(theWorkbook);
                //OfficeAppManager.CloseExcelApp();
                //excelpApp = null;

                // ******* OpenXML for Excel File
                if (System.IO.File.Exists(TargetFilePath))
                {
                    // Delete then create new file
                    System.IO.File.Delete(TargetFilePath);
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }
                else
                {
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }

                try
                {
                    if (IsHarmonized)
                        lstSearch = GetOrignalCodeWithHTMLTagForSearch(lstSearchData);
                    else
                        lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);

                    using (FileStream fs = new FileStream(TargetFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        using (oDocP.SpreadsheetDocument doc = oDocP.SpreadsheetDocument.Open(fs, true))
                        {
                            oDocP.WorkbookPart workbookPart = doc.WorkbookPart;
                            oDocP.SharedStringTablePart sstpart = workbookPart.GetPartsOfType<oDocP.SharedStringTablePart>().First();
                            oDocS.SharedStringTable sst = sstpart.SharedStringTable;

                            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                            foreach (oDocS.SharedStringItem item in sstpart.SharedStringTable.Elements<oDocS.SharedStringItem>())
                            {

                                foreach (var srdata in lstSearch)
                                {
                                    if (item.InnerText == srdata.SearchWord)
                                    {
                                        DocumentFormat.OpenXml.Spreadsheet.Text text2 = item.Descendants<DocumentFormat.OpenXml.Spreadsheet.Text>().First();
                                        text2.Text = srdata.TagName;
                                    }
                                }
                            }
                            sstpart.SharedStringTable.Save();
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    Logger.Write(Session["BPID"].ToString(), ex.StackTrace.ToString());
                }
            }
            else if (FileExt == ".pdf" || FileExt == "pdf" || FileExt == ".PDF" || FileExt == "PDF")
            {
                if (System.IO.File.Exists(TargetFilePath))
                {
                    // Delete then create new file
                    System.IO.File.Delete(TargetFilePath);
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }
                else
                {
                    System.IO.File.Copy(SourceFilePath, TargetFilePath);
                }

                try
                {
                    if (IsHarmonized)
                        lstSearch = GetOrignalCodeWithHTMLTagForSearch(lstSearchData);
                    else
                        lstSearch = GetOrignalCodeWithHTMLTag(lstSearchData);

                    PdfReader pReader = new PdfReader(SourceFilePath);
                    stamper = new iTextSharp.text.pdf.PdfStamper(pReader, new System.IO.FileStream(TargetFilePath, System.IO.FileMode.Create));
                    PDFTextGetter(lstSearch, StringComparison.CurrentCultureIgnoreCase, TargetFilePath, pReader);
                    stamper.Close();
                    pReader.Close();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    DataLogger.Write("FHFile-PDFTextGetter_1", ex.Message);
                }
            }
            return msg;
        }

        public string PDFTextGetter(List<SwordAndTagReplace> lstSearch, StringComparison SC, string TargetFilePath, PdfReader pReaderMain)
        {
            string msg = "";
            try
            {
                iTextSharp.text.pdf.PdfContentByte cb = null;
                iTextSharp.text.pdf.PdfContentByte cb2 = null;
                iTextSharp.text.pdf.PdfWriter writer = null;
                iTextSharp.text.pdf.BaseFont bf = null;
                if (System.IO.File.Exists(TargetFilePath))
                {

                    PdfReader pReader = pReaderMain; //new PdfReader(TargetFilePath);
                    for (int page = 1; page <= pReader.NumberOfPages; page++)
                    {
                        myLocationTextExtractionStrategy strategy = new myLocationTextExtractionStrategy();
                        cb = stamper.GetOverContent(page);
                        cb2 = stamper.GetOverContent(page);
                        strategy.UndercontentCharacterSpacing = (int)cb.CharacterSpacing;
                        strategy.UndercontentHorizontalScaling = (int)cb.HorizontalScaling;
                        string currentText = PdfTextExtractor.GetTextFromPage(pReader, page, strategy);
                        foreach (var data in lstSearch)
                        {
                            List<iTextSharp.text.Rectangle> MatchesFound = strategy.GetTextLocations(data.SearchWord, SC);
                            cb.SetColorFill(iTextSharp.text.BaseColor.WHITE);
                            foreach (iTextSharp.text.Rectangle rect in MatchesFound)
                            {
                                cb.Rectangle(rect.Left, rect.Bottom, 60, rect.Height);
                                cb.Fill();
                                cb2.SetColorFill(iTextSharp.text.BaseColor.BLACK);

                                bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                                cb2.SetFontAndSize(bf, 9);
                                cb2.BeginText();
                                cb2.ShowTextAligned(0, data.TagName, rect.Left, rect.Bottom, 0);
                                cb2.EndText();
                                //cb2.Fill();
                            }
                        }
                    }
                    stamper.Close();
                    pReader.Close();
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                DataLogger.Write("FHFile-PDFTextGetter_2", ex.Message);
            }

            return msg;
        }

        public int GetCountOfSlides(oDocP.PresentationDocument presentationDocument)
        {
            int slidesCount = -1;
            // Get the presentation part of the presentation document.
            oDocP.PresentationPart presentationPart = presentationDocument.PresentationPart;
            // Verify that the presentation part and presentation exist.
            if (presentationPart != null && presentationPart.Presentation != null)
            {
                // Get the Presentation object from the presentation part.
                oDocPP.Presentation presentation = presentationPart.Presentation;
                // Verify that the slide ID list exists.
                if (presentation.SlideIdList != null)
                {
                    // Get the collection of slide IDs from the slide ID list.
                    DocumentFormat.OpenXml.OpenXmlElementList slideIds = presentation.SlideIdList.ChildElements;
                    slidesCount = slideIds.Count;
                }
            }
            return slidesCount;
        }

        public void GetAllTextInSlide(oDocP.PresentationDocument presentationDocument, int slideIndex, List<SwordAndTagReplace> lstSearch)
        {
            // Verify that the presentation document exists.
            if (presentationDocument == null)
            {
                throw new ArgumentNullException("presentationDocument");
            }

            // Verify that the slide index is not out of range.
            if (slideIndex < 0)
            {
                throw new ArgumentOutOfRangeException("slideIndex");
            }

            // Get the presentation part of the presentation document.
            oDocP.PresentationPart presentationPart = presentationDocument.PresentationPart;

            // Verify that the presentation part and presentation exist.
            if (presentationPart != null && presentationPart.Presentation != null)
            {
                // Get the Presentation object from the presentation part.
                oDocPP.Presentation presentation = presentationPart.Presentation;

                // Verify that the slide ID list exists.
                if (presentation.SlideIdList != null)
                {
                    // Get the collection of slide IDs from the slide ID list.
                    DocumentFormat.OpenXml.OpenXmlElementList slideIds =
                        presentation.SlideIdList.ChildElements;

                    // If the slide ID is in range...
                    if (slideIndex < slideIds.Count)
                    {
                        // Get the relationship ID of the slide.
                        string slidePartRelationshipId = (slideIds[slideIndex] as oDocPP.SlideId).RelationshipId;

                        // Get the specified slide part from the relationship ID.
                        oDocP.SlidePart slidePart =
                            (oDocP.SlidePart)presentationPart.GetPartById(slidePartRelationshipId);

                        // Pass the slide part to the next method, and
                        // then return the array of strings that method
                        // returns to the previous method.
                        GetAllTextInSlide(slidePart, lstSearch);
                    }
                }
            }
        }

        public void GetAllTextInSlide(oDocP.SlidePart slidePart, List<SwordAndTagReplace> lstSearch)
        {
            // Verify that the slide part exists.
            if (slidePart == null)
            {
                throw new ArgumentNullException("slidePart");
            }

            // Create a new linked list of strings.
            LinkedList<string> texts = new LinkedList<string>();

            // If the slide exists...
            if (slidePart.Slide != null)
            {
                // Iterate through all the paragraphs in the slide.
                foreach (DocumentFormat.OpenXml.Drawing.Paragraph paragraph in
                    slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Paragraph>())
                {

                    // Iterate through the lines of the paragraph.
                    foreach (DocumentFormat.OpenXml.Drawing.Text text in
                        paragraph.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                    {
                        // text.Text = text.Text.Replace("<1001>", "Binod back");
                        //Replace here
                        foreach (var sData in lstSearch)
                        {
                            text.Text = text.Text.Replace(sData.SearchWord, sData.TagName);
                        }
                    }
                }
            }
        }


        private void SearchReplaceShapes(PPT.Shapes sld, List<SwordAndTagReplace> report, int count)
        {
            try
            {
                foreach (Microsoft.Office.Interop.PowerPoint.Shape s in sld)
                {
                    DoReplace(report, s, 0);
                    OfficeAppManager.ReleaseObject(s);
                }
            }
            catch (Exception x)
            {
                if (x.Message.Contains("0x8001010A") && count < 10)
                {
                    System.Threading.Thread.Sleep(100);
                    SearchReplaceShapes(sld, report, count + 1);
                }
                else
                {
                    // logger here
                }
            }
        }
        private void DoReplace(List<SwordAndTagReplace> lstSearch, Microsoft.Office.Interop.PowerPoint.Shape s, int count)
        {
            int NumberOfChanges = 0;
            try
            {
                if (MsoTriState.msoFalse == s.HasTextFrame) return;
                if (MsoTriState.msoFalse == s.TextFrame2.HasText) return;
                if (string.IsNullOrEmpty(s.TextFrame2.TextRange.Text.Trim())) return;

                string source = s.TextFrame2.TextRange.Text;
                string newText = source;
                foreach (var srdata in lstSearch)
                {
                    if (srdata.TagName == null || !newText.Contains(srdata.SearchWord))
                        continue;

                    var ignorecase = RegexOptions.IgnoreCase | RegexOptions.Multiline;
                    newText = Regex.Replace(newText, srdata.SearchWord, (match) =>
                    {
                        NumberOfChanges += 1;
                        return match.Result(srdata.TagName);
                    }, ignorecase);

                    //OfficeAppManager.ReleaseObject(srdata);
                }

                if (newText != source)
                    s.TextFrame2.TextRange.Text = newText;
            }
            catch (Exception x)
            {
                if (x.Message.Contains("0x8001010A") && count < 10)
                {
                    System.Threading.Thread.Sleep(100);
                    DoReplace(lstSearch, s, count + 1);
                }
                else
                {
                    // logger here
                }
            }

        }
        private static PPT.Shapes GetSlide(PPT.Presentation _pPres, int pos, bool getNoteSlide)
        {
            PPT.Shapes sld = null;

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    if (!getNoteSlide)
                        sld = _pPres.Slides[pos].Shapes;
                    else
                        sld = _pPres.Slides[pos].NotesPage.Shapes;

                    break;
                }
                catch (Exception x)
                {
                    if (!x.ToString().Contains("0x8001010A"))
                    {
                        // Logger here
                        return null;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }
            return sld;
        }
        private static int GetSlideCount(PPT.Presentation _pPres)
        {
            int slideCount = 0;
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    slideCount = _pPres.Slides.Count;
                    break;
                }
                catch (Exception x)
                {
                    if (!x.ToString().Contains("0x8001010A"))
                    {
                        // logger here
                        return 0;
                    }
                    System.Threading.Thread.Sleep(100);
                }
            }

            return slideCount;
        }
        public int SearchReplaceSheet(List<SwordAndTagReplace> lstSword, EXC.Worksheet worksheet)
        {
            int NumberOfChanges = 0;
            object m = Type.Missing;
            foreach (var srdata in lstSword)
            {
                object matchcase = MsoTriState.msoFalse;
                object wholephrase = EXC.XlLookAt.xlWhole;
                EXC.Range range = null;
                try
                {
                    range = worksheet.UsedRange.Find((object)srdata.SearchWord, m, m, wholephrase, m,
                        EXC.XlSearchDirection.xlNext, matchcase);
                }
                catch (Exception)
                {
                    range = null;
                }

                if (null != range && !string.IsNullOrEmpty((string)range.Text))
                {
                    EXC.PivotTable table = null;
                    try
                    {
                        table = range.PivotTable;
                    }
                    catch (Exception)
                    {
                        table = null;
                    }

                    if (null == table)
                    {
                        try
                        {
                            worksheet.UsedRange.Replace(srdata.SearchWord, srdata.TagName, wholephrase, m, matchcase, m, m, m);
                            NumberOfChanges += 1;
                        }
                        catch (Exception)
                        {
                            range = null;
                        }
                    }
                }
            }
            return NumberOfChanges;
        }

        [SessionTimeoutFilter]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddTagDetails(Tag obj)
        {
            Tag _tag = new Tag();
            int Result = 0;
            string msg = "Data update";
            _tag.ID = obj.ID;
            _tag.Orig = obj.Orig;
            _tag.Share = obj.Share;
            _tag.TagName = obj.TagName;
            _tag.UserID = Session["UserID"].ToString();
            _tag.UTAGID = obj.UTAGID;
            _tag.GlobPri = obj.GlobPri;
            _tag.Description = obj.Description;

            Result = _fileData.AddTagNameDetails(_tag);
            if (Result != 1 && Result != 2)
                msg = "Error! Please try again";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCustomTag(Tag obj)
        {
            string Result = string.Empty;
            string msg = "Data update";
            Result = _adminData.UpdateTagDetails(obj);
            if (Result == "0")
                msg = "Data not updated.";

            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Harmonizer()
        {
            HarmonizerModel harmonizerModel = new HarmonizerModel();

            List<Tag> lstOldHarmonizerTag = new List<Tag>();
            DataSet ds = _fileData.GetStandardGlobalTag();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstOldHarmonizerTag.Add(new Tag
                    {
                        UTAGID = Convert.ToInt32(ds.Tables[0].Rows[i]["UTAGID"]),
                        UserID = ds.Tables[0].Rows[i]["UserID"].ToString(),
                        TagName = ds.Tables[0].Rows[i]["Tag"].ToString(),
                        Orig = ds.Tables[0].Rows[i]["Orig"].ToString(),
                        GlobPri = ds.Tables[0].Rows[i]["GlobPri"].ToString(),
                        Description = ds.Tables[0].Rows[i]["Description"].ToString(),
                        Share = "share value"
                    });
                }
            }
            harmonizerModel.OldHarmonizer = lstOldHarmonizerTag;
            harmonizerModel.NewHarmonizer = lstOldHarmonizerTag;

            return View(harmonizerModel);
        }

        public ActionResult GetTemplateDataForFinalTemplate()
        {
            List<CreateListTemplate> lstTemp = new List<CreateListTemplate>();
            try
            {
                DataSet ds = _fileData.GetTemplateWithSector(Convert.ToString(Session["BPID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CreateListTemplate objtemp = new CreateListTemplate();
                        objtemp.FileID = Convert.ToInt32(ds.Tables[0].Rows[i]["FileID"]);
                        objtemp.TemplateDesc = Convert.ToString(ds.Tables[0].Rows[i]["TemplateDesc"]);
                        objtemp.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        objtemp.Partner = Convert.ToString(ds.Tables[0].Rows[i]["Partner"]);
                        lstTemp.Add(objtemp);
                    }
                }
                else
                {
                    Logger.Write(Session["BPID"].ToString(), "Did not get data on that BPID");
                }

            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), ex.Message);
            }
            return PartialView("_GetTemplateDataForFinalTemplate", lstTemp);
        }

        public JsonResult GetTemplateList()
        {
            List<CreateListTemplate> lstTemp = new List<CreateListTemplate>();
            try
            {
                DataSet ds = _fileData.GetTemplateWithSector(Convert.ToString(Session["BPID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CreateListTemplate objtemp = new CreateListTemplate();
                        objtemp.FileID = Convert.ToInt32(ds.Tables[0].Rows[i]["FileID"]);
                        objtemp.TemplateDesc = Convert.ToString(ds.Tables[0].Rows[i]["TemplateDesc"]);
                        objtemp.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        objtemp.Partner = Convert.ToString(ds.Tables[0].Rows[i]["Partner"]);
                        lstTemp.Add(objtemp);
                    }
                }
                else
                {
                    Logger.Write(Session["BPID"].ToString(), "Did not get data on that BPID");
                }

            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), ex.Message);
            }
            //CreateListTemplate objtemp = new CreateListTemplate();
            //objtemp.FileID = 1;
            //objtemp.TemplateDesc = "Test";
            //objtemp.SECID = "ASR";
            //objtemp.Partner = "Customer";
            //lstTemp.Add(objtemp);
            return Json(lstTemp.Take(1), JsonRequestBehavior.AllowGet);
            //return Json("test", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTemplateDetail(string FileId)
        {
            string BPID = Convert.ToString(Session["BPID"]);
            string UserID = Convert.ToString(Session["UserID"]);
            List<Tag> lstTag = new List<Tag>();
            Tag objtag = new Tag();
            string TemplateName = "FH001 Project status Rpt";
            string FilterName = "FC001 Project status Report";
            DataSet ds = _fileData.GetTemplateDetailById(Convert.ToInt32(FileId), Convert.ToString(Session["BPID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objtag = new Tag();
                    objtag.UTAGID = Convert.ToInt32(ds.Tables[0].Rows[i]["UTAGID"]);
                    objtag.UserID = UserID;//Convert.ToString(ds.Tables[0].Rows[i]["UserID"]);// GlobalTag userid
                    objtag.TagName = Convert.ToString(ds.Tables[0].Rows[i]["Tag"]);
                    objtag.Orig = Convert.ToString(ds.Tables[0].Rows[i]["Orig"]);
                    objtag.GlobPri = Convert.ToString(ds.Tables[0].Rows[i]["GlobPri"]);
                    objtag.Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                    objtag.Share = Convert.ToString(ds.Tables[0].Rows[i]["Share"]);
                    objtag.BPID = BPID;
                    lstTag.Add(objtag);
                }

                TemplateName = ds.Tables[0].Rows[0]["TEMPID"].ToString() + " " + ds.Tables[0].Rows[0]["TemplateDesc"].ToString();
                FilterName = ds.Tables[0].Rows[0]["CFLTRID"].ToString() + " " + ds.Tables[0].Rows[0]["TemplateDesc"].ToString();
            }

            return Json(new { lst = _userData.AutoCalculateValue(lstTag), template = TemplateName, filter = FilterName }, JsonRequestBehavior.AllowGet);

        }


        [SessionTimeoutFilter]
        public ActionResult Repository()
        {
            List<Repository> lst = new List<Repository>();

            return View(lst);
        }

        public ActionResult _PartialRepository(List<Repository> lstTemplateRepository)
        {
            return PartialView(lstTemplateRepository);
        }

        public ActionResult _CreateTemplate()
        {
            DataSet dsSectore = _fileData.GetTemplateWithSector(Session["BPID"].ToString());
            List<SelectListItem> lstTemplates = new List<SelectListItem>();
            lstTemplates.Add(new SelectListItem { Text = "Select template file", Value = "0" });
            if (dsSectore.Tables[0].Rows.Count > 0)
            {
                // Selected=(item == defaultCountry ? true : false) }
                for (int i = 0; i < dsSectore.Tables[0].Rows.Count; i++)
                {
                    lstTemplates.Add(new SelectListItem { Text = dsSectore.Tables[0].Rows[i]["TemplateDesc"].ToString(), Value = dsSectore.Tables[0].Rows[i]["FileID"].ToString() });
                }
            }

            ViewData["lstTemplate"] = lstTemplates;

            return PartialView();
        }

        public ActionResult _CreateHarmonizer(List<Repository> lstTemplateRepository)
        {
            return PartialView(lstTemplateRepository);

        }

        public ActionResult GetRepositoryDataByTemplateID(int TemplateID)
        {
            Repository repo = new Repository();
            List<Repository> lstRepository = new List<Repository>();
            DataSet ds = _fileData.GetRepositoryByTemplateID(TemplateID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstRepository.Add(new Repository
                    {
                        ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]),
                        TemplateID = Convert.ToInt32(ds.Tables[0].Rows[i]["TemplateID"]),
                        BPID = ds.Tables[0].Rows[i]["BPID"].ToString(),
                        UserID = ds.Tables[0].Rows[i]["UserID"].ToString(),
                        UTAGID = ds.Tables[0].Rows[i]["UTAGID"].ToString(),
                        Tag = ds.Tables[0].Rows[i]["Tag"].ToString(),
                        Description = ds.Tables[0].Rows[i]["Description"].ToString(),
                        Org = ds.Tables[0].Rows[i]["Org"].ToString(),
                        GlobPri = ds.Tables[0].Rows[i]["GlobPri"].ToString(),
                        Share = ds.Tables[0].Rows[i]["Share"].ToString(),
                        CreatedDate = ds.Tables[0].Rows[i]["CreatedDate"] != null ? Convert.ToDateTime(ds.Tables[0].Rows[i]["CreatedDate"]) : DateTime.Now,
                        UpdateDate = ds.Tables[0].Rows[i]["UpdateDate"] != null ? Convert.ToDateTime(ds.Tables[0].Rows[i]["UpdateDate"]) : DateTime.Now
                    });
                }
            }
            return Json(lstRepository, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeoutFilter]
        public ActionResult saveRepositoryInfo(List<Repository> lstSearch)
        {
            string dFileName = "";
            string msg = "Data update!";
            DataTable dt = new DataTable();
            try
            {
                var UserID = Convert.ToString(Session["UserID"]);
                var BPID = Convert.ToString(Session["BPID"]);
                dt = ConvertToDataTableForRepostory(lstSearch, UserID, BPID);
                _fileData.UpdateTemplateShareValue(dt);// Update share as per template
                _fileData.CreateRepository(dt);// Update in repository
                _fileData.CreateUpdateShareValue(dt, BPID, "Update");// Update common share value
            }
            catch (Exception ex)
            {
                msg = "Data not updated!, Please try again.";
            }
            return Json(new { massage = msg, fileName = dFileName }, JsonRequestBehavior.AllowGet);

        }

        [SessionTimeoutFilter]
        public ActionResult CreateHarmonizerTemplate(List<Repository> lstSearch)
        {
            string ReturnFileName = "";
            string FileSucessToProcessed = "";
            string msg = "Complete Harmonizer process!";
            DataTable dt = new DataTable();
            DataSet dsFileDetails = new DataSet();
            string UniqueeNumberForFileName = DateTime.Now.ToString("MMddyyyy_HHmmss");
            List<SwordAndTagReplace> dataReplace = new List<SwordAndTagReplace>();
            int TemplateIdOrFileID = lstSearch.FirstOrDefault().TemplateID;
            dsFileDetails = _fileData.GetFileDetails(TemplateIdOrFileID);
            string hSourcePath = dsFileDetails.Tables[0].Rows[0]["targetfilepath"].ToString();
            string fileExt = dsFileDetails.Tables[0].Rows[0]["fileext"].ToString();
            string hTragetPath = Server.MapPath("~").TrimEnd('\\') + "\\Harmonized\\" + Session["BPID"].ToString() + "\\" + "HT" + UniqueeNumberForFileName + "_" + dsFileDetails.Tables[0].Rows[0]["NewFileName"].ToString();
            string TemplateName = dsFileDetails.Tables[0].Rows[0]["TemplateName"].ToString();
            string FileName = "HT" + UniqueeNumberForFileName + "_" + dsFileDetails.Tables[0].Rows[0]["NewFileName"].ToString();
            string dFileName = string.IsNullOrEmpty(dsFileDetails.Tables[0].Rows[0]["DFileName"].ToString()) == false ? dsFileDetails.Tables[0].Rows[0]["DFileName"].ToString() : TemplateName;

            var UserID = Convert.ToString(Session["UserID"]);
            var BPID = Convert.ToString(Session["BPID"]);

            try
            {
                // Create Harmonizer Template
                // dataReplace = CreateTagWithHTMLForHarmonizer(lstSearch);// 22/1/2019
                dataReplace = CreateTagWithHTMLStringForHarmonizer(lstSearch);
                //FileSucessToProcessed = ReplaceDataWithList(dataReplace, hTragetPath, fileExt, hSourcePath, true);//commented 19/12/20018
                FileSucessToProcessed = ReplaceDataWithList(dataReplace, hTragetPath, fileExt, hSourcePath, true);
                // store record in DB if get return as blank
                if (FileSucessToProcessed == "" && string.IsNullOrEmpty(FileSucessToProcessed))
                {
                    _fileData.SaveHarmonizerTeamplateInfo(TemplateIdOrFileID, BPID, UserID, TemplateName, hTragetPath);
                    ReturnFileName = BPID + "/" + FileName + "/" + dFileName;
                }

            }
            catch (Exception ex)
            {
                msg = "ERROR!, Please try again.";
            }
            finally { }


            return Json(new { massage = msg, fileName = ReturnFileName }, JsonRequestBehavior.AllowGet);
        }


        public static DataTable ConvertToDataTableForRepostory(List<Repository> data, string UserId, string BPID)
        {

            DataTable table = new DataTable();
            table.Columns.Add("BPID", typeof(string));
            table.Columns.Add("UserID", typeof(string));
            table.Columns.Add("UTAGID", typeof(string));
            table.Columns.Add("Tag", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Org", typeof(string));
            table.Columns.Add("GlobPri", typeof(string));
            table.Columns.Add("Share", typeof(string));
            table.Columns.Add("TemplateID", typeof(Int64));

            for (int i = 0; i < data.ToList().Count; i++)
                table.Rows.Add(new object[] {
                            data[i].BPID =BPID,
                            data[i].UserID =UserId,
                            data[i].UTAGID,
                            data[i].Tag,
                            data[i].Description,
                            data[i].Org,
                            data[i].GlobPri,
                            data[i].Share,
                            data[i].TemplateID

                           });

            return table;
        }

        public List<SwordAndTagReplace> CreateTagWithHTMLForHarmonizer(List<Repository> lstSearch)
        {
            // Only G value replace P value replace with blank 15 space
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                if (item.GlobPri == "G")
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "<" + item.UTAGID + ">",
                        TagName = item.Share,
                        Instruction = "0"
                    });
                }
                else if (item.GlobPri == "P")
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "<" + item.UTAGID + ">",
                        TagName = "          ",
                        Instruction = "0"
                    });
                }
                else
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "<" + item.UTAGID + ">",
                        TagName = item.Share,
                        Instruction = "0"
                    });
                }
            }

            return lst;

        }

        public List<SwordAndTagReplace> CreateTagWithHTMLStringForHarmonizer(List<Repository> lstSearch)
        {
            // Only G value replace P value replace with blank 15 space
            List<SwordAndTagReplace> lst = new List<SwordAndTagReplace>();

            foreach (var item in lstSearch)
            {
                if (item.GlobPri == "G")
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "&lt;" + item.UTAGID + "&gt;",
                        TagName = item.Share,
                        Instruction = "0"
                    });
                }
                else if (item.GlobPri == "P")
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "&lt;" + item.UTAGID + "&gt;",
                        TagName = "          ",
                        Instruction = "0"
                    });
                }
                else
                {
                    lst.Add(new SwordAndTagReplace
                    {
                        ID = item.ID,
                        SearchWord = "&lt;" + item.UTAGID + "&gt;",
                        TagName = item.Share,
                        Instruction = "0"
                    });
                }
            }

            return lst;

        }

        public FileResult DownloadFile(string FileName)
        {
            string[] SplitFileName = FileName.Split('/');
            string DownloadFileName = SplitFileName[2].ToString();
            string TemplateFilePath = SplitFileName[0].ToString() + "/" + SplitFileName[1].ToString();
            string FilePath = Server.MapPath("~/Harmonized/" + TemplateFilePath);
            string FileExt = System.IO.Path.GetExtension(FilePath);
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/Harmonized/" + TemplateFilePath));
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, DownloadFileName);
        }

        public ActionResult TestUploadFile()
        {
            //string jdata = "{\"id\":\"PAYID-LUA7CPY3YB24344CC126645A\",\"intent\":\"sale\",\"state\":\"approved\",\"cart\":\"2KC35886P30044945\",\"payer\":{\"payment_method\":\"paypal\",\"status\":\"VERIFIED\",\"payer_info\":{\"email\":\"piyushshrm-buyer@gmail.com\",\"first_name\":\"Piyush\",\"last_name\":\"Sharma\",\"payer_id\":\"JBYEK8KK58M32\",\"shipping_address\":{\"recipient_name\":\"Piyush Sharma\",\"line1\":\"1 Main St\",\"city\":\"San Jose\",\"state\":\"CA\",\"postal_code\":\"95131\",\"country_code\":\"US\"},\"country_code\":\"US\"}},\"transactions\":[{\"amount\":{\"total\":\"50.00\",\"currency\":\"USD\",\"details\":{\"subtotal\":\"40.00\",\"tax\":\"5.00\",\"shipping\":\"5.00\"}},\"payee\":{\"merchant_id\":\"CJ762SUCB87XN\",\"email\":\"fh-facilitator@fileharmonizer.com\"},\"item_list\":{\"items\":[{\"name\":\"Booking reservation\",\"description\":\"Booking reservation at ABC hotel at 24/03/2015 from 1pm to 4pm.\",\"price\":\"40.00\",\"currency\":\"USD\",\"tax\":\"5.00\",\"quantity\":1}],\"shipping_address\":{\"recipient_name\":\"Piyush Sharma\",\"line1\":\"1 Main St\",\"city\":\"San Jose\",\"state\":\"CA\",\"postal_code\":\"95131\",\"country_code\":\"US\"},\"shipping_options\":[null]},\"related_resources\":[{\"sale\":{\"id\":\"99X6863523779681T\",\"state\":\"completed\",\"amount\":{\"total\":\"50.00\",\"currency\":\"USD\",\"details\":{\"subtotal\":\"40.00\",\"tax\":\"5.00\",\"shipping\":\"5.00\"}},\"payment_mode\":\"INSTANT_TRANSFER\",\"protection_eligibility\":\"ELIGIBLE\",\"protection_eligibility_type\":\"ITEM_NOT_RECEIVED_ELIGIBLE,UNAUTHORIZED_PAYMENT_ELIGIBLE\",\"transaction_fee\":{\"value\":\"1.75\",\"currency\":\"USD\"},\"parent_payment\":\"PAYID-LUA7CPY3YB24344CC126645A\",\"create_time\":\"2019-06-13T06:54:25Z\",\"update_time\":\"2019-06-13T06:54:25Z\",\"links\":[{\"href\":\"https://api.sandbox.paypal.com/v1/payments/sale/99X6863523779681T\",\"rel\":\"self\",\"method\":\"GET\"},{\"href\":\"https://api.sandbox.paypal.com/v1/payments/sale/99X6863523779681T/refund\",\"rel\":\"refund\",\"method\":\"POST\"},{\"href\":\"https://api.sandbox.paypal.com/v1/payments/payment/PAYID-LUA7CPY3YB24344CC126645A\",\"rel\":\"parent_payment\",\"method\":\"GET\"}],\"soft_descriptor\":\"PAYPAL *TESTFACILIT\"}}]}],\"create_time\":\"2019-06-13T06:54:26Z\",\"links\":[{\"href\":\"https://api.sandbox.paypal.com/v1/payments/payment/PAYID-LUA7CPY3YB24344CC126645A\",\"rel\":\"self\",\"method\":\"GET\"}]}";
            //dynamic jsonconvertedData = JsonConvert.DeserializeObject<dynamic>(jdata);
            //var data= JsonConvert.DeserializeObject<dynamic>(jdata);// work
            //dynamic data1 = Newtonsoft.Json.Linq.JObject.Parse(jdata);
            //string d = "";
            // suceess to implemented
            //byte[] byteArray =System.IO.File.ReadAllBytes(@"D:\MVCCore\Source\New folder\Form Specification v7_1.docx");
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    memoryStream.Write(byteArray, 0, byteArray.Length);
            //    using (oDocP.WordprocessingDocument doc = oDocP.WordprocessingDocument.Open(memoryStream, true))
            //    {
            //        oDocPwoerTool.HtmlConverterSettings settings = new oDocPwoerTool.HtmlConverterSettings()
            //        {
            //            PageTitle = "My Page Title"
            //        };
            //        XElement html = oDocPwoerTool.HtmlConverter.ConvertToHtml(doc, settings);

            //        // Note: the XHTML returned by ConvertToHtmlTransform contains objects of type
            //        // XEntity. PtOpenXmlUtil.cs defines the XEntity class. See
            //        // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
            //        // for detailed explanation.
            //        //
            //        // If you further transform the XML tree returned by ConvertToHtmlTransform, you
            //        // must do it correctly, or entities do not serialize properly.

            //        System.IO.File.WriteAllText(@"d:\HtmlFolder\Test.html", html.ToStringNewLineOnAttributes());
            //    }
            //}


            /*
             oDocP.WorkbookPart workbookPart = doc.WorkbookPart;
                            oDocP.SharedStringTablePart sstpart = workbookPart.GetPartsOfType<oDocP.SharedStringTablePart>().First();
                            oDocS.SharedStringTable sst = sstpart.SharedStringTable;

                            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
                            foreach (oDocS.SharedStringItem item in sstpart.SharedStringTable.Elements<oDocS.SharedStringItem>())
                            {

                                foreach (var srdata in lstSearch)
                                {
                                    if (item.InnerText == srdata.SearchWord)
                                    {
                                        DocumentFormat.OpenXml.Spreadsheet.Text text2 = item.Descendants<DocumentFormat.OpenXml.Spreadsheet.Text>().First();
                                        text2.Text = srdata.TagName;
                                    }
                                }
                            }
                            sstpart.SharedStringTable.Save();
             */
            // For xmlx file convert to html or pdf
            //byte[] byteArray = System.IO.File.ReadAllBytes(@"D:\MVCCore\Source\New folder\7Worksheet.xlsx");
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    memoryStream.Write(byteArray, 0, byteArray.Length);
            //    using (oDocP.SpreadsheetDocument doc = oDocP.SpreadsheetDocument.Open(memoryStream, true))
            //    {
            //        oDocP.WorkbookPart workbookPart = doc.WorkbookPart;
            //        oDocP.SharedStringTablePart sstpart = workbookPart.GetPartsOfType<oDocP.SharedStringTablePart>().First();
            //        oDocS.SharedStringTable sst = sstpart.SharedStringTable;

            //        oDocPwoerTool.HtmlConverterSettings settings = new oDocPwoerTool.HtmlConverterSettings()
            //        {
            //            PageTitle = "My Page Title"
            //        };
            //        XElement html = oDocPwoerTool.HtmlConverter.ConvertToHtml(sst.SharedStringTablePart, settings);

            //        // Note: the XHTML returned by ConvertToHtmlTransform contains objects of type
            //        // XEntity. PtOpenXmlUtil.cs defines the XEntity class. See
            //        // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
            //        // for detailed explanation.
            //        //
            //        // If you further transform the XML tree returned by ConvertToHtmlTransform, you
            //        // must do it correctly, or entities do not serialize properly.

            //        System.IO.File.WriteAllText(@"d:\HtmlFolder\TestXLSX.html", html.ToStringNewLineOnAttributes());
            //    }
            //}

            //get mac address
            System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            string UniCastAddrss = string.Empty;
            foreach (System.Net.NetworkInformation.NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    System.Net.NetworkInformation.IPInterfaceProperties properties = adapter.GetIPProperties();
                    UniCastAddrss = properties.UnicastAddresses[1].Address.ToString();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            // To Get IP Address


            string IPHost = Dns.GetHostName();
            string IP = Dns.GetHostByName(IPHost).AddressList[0].ToString();

            ViewBag.MACAddress = sMacAddress;
            ViewBag.IPHost = IPHost;
            ViewBag.IP = IP;
            ViewBag.UniCastAddrss = UniCastAddrss;
            return View();
        }

        [HttpPost]
        public ActionResult TestUploadFilePost(HttpPostedFileBase file, string name)
        {
            if (file != null && file.ContentLength > 0)
            {

                var fileName = System.IO.Path.GetFileName(file.FileName);
            }

            return new EmptyResult();
        }

        [SessionTimeoutFilter]
        public ActionResult UserHeader()
        {
            CommanUserData obj = new CommanUserData();
            try
            {
                obj = _userData.GetCommanData(Convert.ToString(Session["UserID"]));
            }
            catch (Exception ex)
            {
                DataLogger.Write("Account-CommanUserData", ex.Message);
            }
            return PartialView("UserHeader", obj);
        }

        public ActionResult _FileStatusSettingInfo()
        {
            return PartialView();
        }
        public ActionResult MassUploadSearchData()
        {
            int r = 0;
            List<Tag> _lstTag = new List<Tag>();
            string result = "File not found to upload data";
            HttpPostedFileBase Fileupload = null;
            try
            {
                if (Request.Files.Count > 0)
                {
                    Fileupload = Request.Files[0];
                    var supportedTypes = new[] { "txt", "csv" };
                    var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                        string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                        Fileupload.SaveAs(sourcePathToWrite);
                        // Start process to store in DB
                        _lstTag = GetAllRowDataSearch(sourcePathToWrite);
                        if (_lstTag.Count() > 0)
                        {
                            //start process
                            result = "";
                        }
                        else
                        {
                            result = "Data format not correct user semicolon(;) ";
                        }

                        // Delete File After process
                        FileInfo file = new FileInfo(sourcePathToWrite);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    else
                    {
                        result = "Data format not correct user semicolon(;) ";
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-MassUploadSearchData", ex.Message);
            }
            return Json(new { msg = result, response = _lstTag }, JsonRequestBehavior.AllowGet);
        }

        [SessionTimeoutFilter]
        public ActionResult ViewPreview()
        {
            DataLogger.Write("FHFile-ViewPreview", "Call preview file methods");
            int r = 0;
            string fname = "";
            string path = "";
            string fullPathUrlTemplate = "";
            string host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            string rootDomain = ConfigurationManager.AppSettings["rootDomain"].ToString();
            HttpPostedFileBase Fileupload = null;
            try
            {
                if (!string.IsNullOrEmpty(port))
                {
                    fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + ":" + port + "/Source/" + Session["BPID"] + "/";
                }
                else
                {
                    fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + "/Source/" + Session["BPID"] + "/";
                }


                if (Request.Files.Count > 0)
                {
                    Fileupload = Request.Files[0];
                    var supportedTypes = new[] { "txt", "csv", "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx" };
                    var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                    if (supportedTypes.Contains(fileExt))
                    {
                        fname = System.IO.Path.GetFileName(Fileupload.FileName).Replace(" ", "_").Replace('+', '_').Replace('-', '_');
                        string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_Preview_";
                        string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + fname;
                        Fileupload.SaveAs(sourcePathToWrite);
                        path = fullPathUrlTemplate + IdentificationPath + fname;
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-ViewPreview", ex.Message);
                Logger.Write(Session["BPID"].ToString(), ex.Message);
                Logger.Write(Session["BPID"].ToString(), ex.StackTrace);
                Logger.Write(Session["BPID"].ToString(), path);
            }
            return Json(path, JsonRequestBehavior.AllowGet);
        }
        public List<Tag> GetAllRowDataSearch(string FilePath)
        {
            List<Tag> _lstTag = new List<Tag>();
            string csvData = System.IO.File.ReadAllText(FilePath);
            //Execute a loop over the rows.
            foreach (string row in csvData.Split('\n').Skip(1))
            {
                Tag _tag = new Tag();
                if (!string.IsNullOrEmpty(row))
                {
                    string[] data = row.Split(';');
                    _tag.SearchWord = data[0].ToString().Replace("\r", "").Trim();
                    _tag.TagName = "<" + data[1].ToString().Replace("\r", "").Trim() + ">";
                    _tag.Instruction = data[2].ToString().Replace("\r", "").Trim();
                    _lstTag.Add(_tag);
                }

            }
            return _lstTag;
        }

        [SessionTimeoutFilter]
        [HttpPost]
        public ActionResult HarmonizeTemplateWithShareVaule(int FileID, string PersonaID)
        {
            string ReturnFileName = "";
            string FileSucessToProcessed = "";
            string msg = "Complete Harmonizer process!";
            List<SwordAndTagReplace> dataReplace = new List<SwordAndTagReplace>();
            DataSet dsFileDetails = new DataSet();
            string UniqueeNumberForFileName = DateTime.Now.ToString("MMddyyyy_HHmmss");
            dsFileDetails = _fileData.GetFileDetails(FileID);
            string hSourcePath = dsFileDetails.Tables[0].Rows[0]["targetfilepath"].ToString();
            string fileExt = dsFileDetails.Tables[0].Rows[0]["fileext"].ToString();
            string hTragetPath = Server.MapPath("~").TrimEnd('\\') + "\\Harmonized\\" + Session["BPID"].ToString() + "\\" + "HT" + UniqueeNumberForFileName + "_" + dsFileDetails.Tables[0].Rows[0]["NewFileName"].ToString();
            string TemplateName = dsFileDetails.Tables[0].Rows[0]["TemplateName"].ToString();
            string FileName = "HT" + UniqueeNumberForFileName + "_" + dsFileDetails.Tables[0].Rows[0]["NewFileName"].ToString();
            string dFileName = string.IsNullOrEmpty(dsFileDetails.Tables[0].Rows[0]["DFileName"].ToString()) == false ? dsFileDetails.Tables[0].Rows[0]["DFileName"].ToString() : TemplateName;
            var UserID = Convert.ToString(Session["UserID"]);
            var BPID = Convert.ToString(Session["BPID"]);
            List<Repository> lstSearch = GetDataWithShareValue(BPID, FileID, PersonaID);
            try
            {
                // Create Harmonizer Template
                //dataReplace = CreateTagWithHTMLForHarmonizer(lstSearch);// 22/1/2019
                dataReplace = CreateTagWithHTMLStringForHarmonizer(lstSearch);
                //FileSucessToProcessed = ReplaceDataWithList(dataReplace, hTragetPath, fileExt, hSourcePath, true);//commented 19/12/20018
                FileSucessToProcessed = ReplaceDataWithList(dataReplace, hTragetPath, fileExt, hSourcePath, true);
                // store record in DB if get return as blank
                if (FileSucessToProcessed == "" && string.IsNullOrEmpty(FileSucessToProcessed))
                {
                    _fileData.SaveHarmonizerTeamplateInfo(FileID, BPID, UserID, TemplateName, hTragetPath, PersonaID.ToString());
                    ReturnFileName = BPID + "/" + FileName + "/" + dFileName;
                }

            }
            catch (Exception ex)
            {
                //msg = "ERROR!, Please try again.";
            }
            finally { }


            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public List<Repository> GetDataWithShareValue(string BPID, int FileID, string PersonalID, string op = "")
        {
            List<Repository> lst = new List<Repository>();
            DataSet ds = _fileData.GetTagWithShareData(BPID, FileID, PersonalID, op);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Repository repo = new Repository()
                    {
                        GlobPri = row["GlobPri"].ToString(),
                        Description = row["Description"].ToString(),
                        Share = row["Share"].ToString(),
                        TemplateID = Convert.ToInt32(row["FileID"]),
                        UTAGID = row["UTAGID"].ToString(),
                        Tag = row["Tag"].ToString(),
                        Org = row["Orig"].ToString(),
                        Instruction = row["Instruction"].ToString()
                    };
                    lst.Add(repo);
                }

            }
            return _userData.AutoFillValueForShareData(lst);
        }

        [SessionTimeoutFilter]
        public ActionResult ViewPrintFile(string FileID, string HTFileID)
        {
            //Logger.Write(Session["BPID"].ToString(), "Print File Issue: -" + FileID + "-" + HTFileID);
            string origanlPath = "";
            string ext = "";
            string FilePath = "";// @"D:\Client Projects\Joe Simmons\Code\FileHarmonizer\Harmonizer.File\Harmonizer.UI\Target\1002\binod_AGR_12192018_145834_Form Specification v7.docx";
            string PrintPath = "";
            string[] caluclatePath = { };
            int len = 0;
            string IdentificationPath = Session["UserID"].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_Print_";
            string StroragePathForPrint = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + "PrintFile.html";
            //Logger.Write(Session["BPID"].ToString(), "Print File Issue_1: -" + FileID + "-" + HTFileID);
            string calPath = "";
            string fullPathUrlTemplate = "";
            string host = Request.Url.Host;
            string port = Request.Url.Port.ToString();
            string rootDomain = ConfigurationManager.AppSettings["rootDomain"].ToString();
            try
            {
                if (!string.IsNullOrEmpty(FileID) && HTFileID == "0")
                {
                    FilePath = _fhManage.GetFHFileID(FileID, "F");
                }
                else if (!string.IsNullOrEmpty(HTFileID) && FileID == "0")
                {
                    FilePath = _fhManage.GetFHFileID(HTFileID, "H");
                }

                //Logger.Write(Session["BPID"].ToString(), "Print File Issue_2: -" + FileID + "-" + HTFileID);
            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), "Print File Error, data not found_1: -" + ex.Message + "--" + ex.StackTrace);
                PrintPath = "";
            }

            try
            {
                caluclatePath = FilePath.Split('\\');
                len = caluclatePath.Length - 1;
                calPath = caluclatePath[len - 2] + "/" + caluclatePath[len - 1] + "/" + caluclatePath[len];

                if (!string.IsNullOrEmpty(port))
                {
                    fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + ":" + port + "/Source/" + Session["BPID"] + "/";
                    origanlPath = rootDomain + host.TrimEnd('/') + ":" + port + "/" + calPath;
                }
                else
                {
                    fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + "/Source/" + Session["BPID"] + "/";
                    origanlPath = rootDomain + host.TrimEnd('/') + ":" + port + "/" + calPath;
                }
                //Logger.Write(Session["BPID"].ToString(), "Print File Issue_3: -" + FileID + "-" + HTFileID);
            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), "Print File Error, data not found_2: -" + ex.Message + "--" + ex.StackTrace);
                PrintPath = "";
            }
            // Code correct

            try
            {
                //Logger.Write(Session["BPID"].ToString(), "Print File Issue_4 Insert Try: -" + FileID + "-" + HTFileID);
                //Logger.Write(Session["BPID"].ToString(), "Print file extendsion Insert Try: -" + System.IO.Path.GetExtension(FilePath));
                if (System.IO.Path.GetExtension(FilePath) == ".docx" || System.IO.Path.GetExtension(FilePath) == "docx")
                {
                    try
                    {
                        //Logger.Write(Session["BPID"].ToString(), "Print File Issue_5 Insert Try: -" + FileID + "-" + HTFileID);
                        WebRequest req = WebRequest.Create(FilePath);
                        using (WebResponse resp = req.GetResponse())
                        {
                            //Logger.Write(Session["BPID"].ToString(), "Print File Issue_6 Insert Try: -" + FileID + "-" + HTFileID);
                            using (Stream stream = resp.GetResponseStream())
                            {
                                //Logger.Write(Session["BPID"].ToString(), "Print File Issue_8 Insert Try: -" + FileID + "-" + HTFileID);
                                string outFile = StroragePathForPrint;
                                //FileStream fs = new FileStream(outFile, FileMode.Create);
                                MemoryStream fs = new MemoryStream();
                                byte[] byteArray = new byte[resp.ContentLength];
                                stream.Read(byteArray, 0, (int)resp.ContentLength);
                                fs.Write(byteArray, 0, (int)resp.ContentLength);
                                using (oDocP.WordprocessingDocument doc = oDocP.WordprocessingDocument.Open(fs, true))
                                {
                                    oDocPwoerTool.HtmlConverterSettings settings = new oDocPwoerTool.HtmlConverterSettings()
                                    {
                                        //PageTitle = "My Page Title"
                                    };
                                    XElement html = oDocPwoerTool.HtmlConverter.ConvertToHtml(doc, settings);
                                    System.IO.File.WriteAllText(StroragePathForPrint, html.ToStringNewLineOnAttributes());
                                }
                                fs.Close();
                            }
                        }

                        PrintPath = fullPathUrlTemplate + IdentificationPath + "PrintFile.html";
                        ViewBag.Path = PrintPath;
                        //return PartialView("_PrintFile");
                    }
                    catch (Exception ex)
                    {
                        Logger.Write(Session["BPID"].ToString(), "Print File Error: -" + ex.Message + "--" + ex.StackTrace);
                        PrintPath = "";
                    }
                }

                else if (System.IO.Path.GetExtension(FilePath) == ".pdf")
                {
                    PrintPath = origanlPath;
                }
                else
                {
                    PrintPath = "";
                }
            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), "Print File Error, data not found_3: -" + ex.Message + "--" + ex.StackTrace);
                PrintPath = "";
            }
            // PrintPath = PrintFilePath(FilePath, FileID, HTFileID, fullPathUrlTemplate, StroragePathForPrint, IdentificationPath, origanlPath);
            return Json(PrintPath, JsonRequestBehavior.AllowGet);
        }


        [SessionTimeoutFilter]
        public ActionResult ManageTag()
        {
            List<SelectListItem> lstSectore = new List<SelectListItem>();
            DataSet dsSectore = _userData.GetSector();
            lstSectore.Add(new SelectListItem { Text = "Select Sector Type", Value = "0" });
            try
            {
                if (dsSectore.Tables[0].Rows.Count > 0)

                {
                    // Selected=(item == defaultCountry ? true : false) }
                    for (int i = 0; i < dsSectore.Tables[0].Rows.Count; i++)
                    {
                        lstSectore.Add(new SelectListItem { Text = dsSectore.Tables[0].Rows[i]["Description"].ToString(), Value = dsSectore.Tables[0].Rows[i]["SECID"].ToString(), Selected = dsSectore.Tables[0].Rows[i]["SECID"].ToString() == Session["SECID"].ToString() ? true : false });
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-ManageTag", ex.Message);
            }

            ViewData["lstSectore"] = lstSectore;

            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View("ManageUserTag");
        }

        [SessionTimeoutFilter]
        public ActionResult ViewAllUserTag(string SecID = "")
        {
            List<Tag> lstTag = new List<Tag>();
            Tag _tag = new Tag();

            DataSet ds = new DataSet();
            string BPID = Session["BPID"].ToString();
            string UserID = Session["UserID"].ToString();
            if (string.IsNullOrWhiteSpace(SecID))
                ds = _userData.GetPrivateTag(BPID, UserID, "selectUserTag");
            else
                ds = _userData.GetPrivateTag(BPID, UserID, "selectUserTag", SecID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    _tag = new Tag()
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        UTAGID = Convert.ToInt32(row["UTAGID"]),
                        TagName = row["Tag"].ToString(),
                        GlobPri = row["GlobPri"].ToString(),
                        Description = row["Description"].ToString(),
                        Share = row["Share"].ToString(),
                        Orig = row["Org"].ToString(),
                        UserID = UserID,
                        BPID = BPID
                    };
                    lstTag.Add(_tag);
                }
            }

            return PartialView("_ViewAllUserTag", _userData.AutoCalculateValue(lstTag));
        }



        [SessionTimeoutFilter]
        public ActionResult GetBusinessTemplateByBPIDOrFH(string BPIDOrFH, string SecID)
        {
            Association association = new Association();
            string BPID = Session["BPID"].ToString();
            string UserID = Session["UserID"].ToString();
            //String FHnumber = Session["FHnumber"].ToString();
            List<CreateListTemplate> lstTemp = new List<CreateListTemplate>();
            try
            {
                DataSet ds = _fileData.GetBusinessTemplateBPIDOrFH(BPIDOrFH, SecID, 0, "Template", BPID);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CreateListTemplate objtemp = new CreateListTemplate();
                        objtemp.FileID = Convert.ToInt32(ds.Tables[0].Rows[i]["FileID"]);
                        objtemp.TemplateDesc = Convert.ToString(ds.Tables[0].Rows[i]["TemplateDesc"]);
                        objtemp.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        objtemp.Partner = Convert.ToString(ds.Tables[0].Rows[i]["Partner"]);
                        lstTemp.Add(objtemp);
                    }
                }
                else
                {
                    Logger.Write(Session["BPID"].ToString(), "Did not get data on that BPID");
                }

            }
            catch (Exception ex)
            {
                Logger.Write(Session["BPID"].ToString(), ex.Message);
            }
            return PartialView("_GetTemplateDataForFinalTemplate", lstTemp);
        }

        [SessionTimeoutFilter]
        public ActionResult GetBusinessFilter(string BPIDOrFH, string SecID, int FileID)
        {
            List<Tag> lstTag = new List<Tag>();
            Tag _tag = new Tag();

            DataSet ds = new DataSet();
            string BPID = Session["BPID"].ToString();
            string UserID = Session["UserID"].ToString();
            ds = _fileData.GetBusinessTemplateBPIDOrFH(BPIDOrFH, SecID, FileID, "Filter", BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    _tag = new Tag()
                    {
                        ID = Convert.ToInt32(FileID),
                        UTAGID = Convert.ToInt32(row["UTAGID"]),
                        TagName = row["Tag"].ToString(),
                        GlobPri = row["GlobPri"].ToString(),
                        Description = row["Description"].ToString(),
                        Share = row["Share"].ToString(),
                        Orig = row["Org"].ToString()
                    };
                    lstTag.Add(_tag);
                }
            }
            if (!string.IsNullOrEmpty(BPIDOrFH) && lstTag.Count > 0)
                lstTag = lstTag.Where(x => x.GlobPri.ToLower().Trim() == "g").ToList();

            return PartialView("_ViewAllUserTag", lstTag);
        }


        [SessionTimeoutFilter]
        [HttpPost]
        public ActionResult CopyTemplate(int fileID, string oldName, string newName)
        {
            int retValue = -1;
            string msg = "";
            string BPID = Session["BPID"].ToString();
            string userId = Session["UserID"].ToString();
            retValue = _fileData.CopyTemplate(fileID, oldName, newName, userId, BPID);
            if (retValue >= 1)
            {
                msg = "Template copy successfully";
            }
            else
            {
                msg = "Erro! While copy template";
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        /////////////////////////// CHANGE 8/3/2019    /////////////////////////////////

        [ValidateInput(false)]
        [SessionTimeoutFilter]
        [HttpPost]
        public ActionResult UploadTemplate()
        {
            DataLogger.Write("FHFile-UploadTemplate", "Call Upload template function");
            FileUploadDownload fileUploadData = new FileUploadDownload();
            Template template = new Template();
            CHFilter filter = new CHFilter();
            CHFilter HFilter = new CHFilter();
            HttpPostedFileBase Fileupload = null;
            string msg = "File not uploaded! Please fill correct data.";
            int UploadDataInDB = -1;
            int StoreTagHistory = -1;
            int CreateTemplate = -1;
            string ReturnCFLTRID = "";
            string ReturnHFLTRID = "";
            string UnquicSECTAGCODE = "";
            string FileSucessToProcessed = "";
            List<SwordAndTagReplace> lstSearch = GetAllFiltersFromFiles();
            if (lstSearch.Count > 0)
            {
                try
                {
                    lstSearch = RemoveDuplicateSearchText(lstSearch);//JsonConvert.DeserializeObject<List<SwordAndTagReplace>>(Request.Form[0].ToString())
                    UnquicSECTAGCODE = _fileData.GetTAGSECCode(Request.Form[2].ToString() == "0" ? "H" : Request.Form[2].ToString());
                    template.TemplateType = Request.Form[1].ToString();
                    template.SECID = Request.Form[2].ToString() == "0" ? "H" : Request.Form[2].ToString();
                    template.SECCODE = UnquicSECTAGCODE;
                }
                catch (Exception ex)
                {
                    DataLogger.Write("FHFile-UploadFileData", "First:-" + ex.Message);
                    Logger.Write(Session["BPID"].ToString(), "List of search data not find:-" + ex.Message + "--" + ex.StackTrace);
                }

                try
                {

                    if (Request.Files.Count > 0)
                    {
                        int CreateHarmonizerRow = 0;
                        int CreateFilterRow = 0;
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            Fileupload = Request.Files[i];
                            if (Fileupload != null)
                            {
                                var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "csv" };
                                var fileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                                if (!supportedTypes.Contains(fileExt))
                                {
                                    msg = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/CSV File";
                                }
                                else
                                {
                                    // store source data  //Session["SECID"].ToString()  login sectorID
                                    string IdentificationPath = Session["UserID"].ToString() + "_" + Request.Form[2].ToString() + "_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                                    string sourcePathToWrite = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                                    string TargetFilePath = Server.MapPath("~").TrimEnd('\\') + "\\Target\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                                    Fileupload.SaveAs(sourcePathToWrite);
                                    Fileupload.SaveAs(TargetFilePath);
                                    // Store History in DB
                                    fileUploadData.UserID = Session["UserID"].ToString();
                                    fileUploadData.SECID = Request.Form[2].ToString();// Current selected sector ID
                                    fileUploadData.BPID = Session["BPID"].ToString();
                                    fileUploadData.DataDate = DateTime.Now;
                                    fileUploadData.Time = DateTime.Now.ToShortTimeString();
                                    fileUploadData.CreatedBy = Session["UserID"].ToString();
                                    fileUploadData.CreatedDate = DateTime.Now;
                                    fileUploadData.OrginalFileName = Fileupload.FileName;
                                    fileUploadData.SourceFilePath = sourcePathToWrite;
                                    fileUploadData.FileExt = fileExt.ToString();
                                    fileUploadData.IsArchive = false;
                                    fileUploadData.TargetFilePath = TargetFilePath;
                                    fileUploadData.NewFileName = IdentificationPath + Fileupload.FileName;

                                    // Get ID and store in other table for track history
                                    UploadDataInDB = _fileData.UploadSourceFile(fileUploadData);
                                    // store Tag/search data bulk
                                    //StoreTagHistory = _fileData.StoreSearchReplaceTagHistory(GetOrignalCodeWithHTMLTag(lstSearch), UploadDataInDB);// comment 19-12-2018
                                    StoreTagHistory = _fileData.StoreSearchReplaceTagHistory(GetOrignalCodeAfterRemoveHTMLTag(lstSearch), UploadDataInDB);
                                    // Serarch and Replace data with tag ID as <10001>
                                    // check file replaced or not if msg not = "" then track record 
                                    FileSucessToProcessed = ""; //ReplaceDataWithList(lstSearch, TargetFilePath, fileExt, sourcePathToWrite);
                                                                //FileSucessToProcessed = ReplaceDataWithList(GetOrignalCodeAfterRemoveHTMLTag(lstSearch), TargetFilePath, fileExt, sourcePathToWrite);
                                    if (FileSucessToProcessed == "" && string.IsNullOrEmpty(FileSucessToProcessed))
                                    {
                                        // Create Filter
                                        if (CreateFilterRow == 0)
                                        {
                                            CreateFilterRow = 1;// for create single row
                                            filter.UserID = Session["UserID"].ToString();
                                            filter.SECID = Request.Form[2].ToString();
                                            filter.FileID = 0;
                                            filter.FilterDesc = Fileupload.FileName;// source file name
                                            filter.FilterText = "";// template.TemplateName;
                                            filter.FilterType = "C";
                                            filter.SECCODE = template.SECCODE;
                                            filter.BPID = Session["BPID"].ToString();
                                            ReturnCFLTRID = _fileData.CreateFilter(filter);
                                        }
                                        if (CreateHarmonizerRow == 0)
                                        {
                                            CreateHarmonizerRow = 1;
                                            // Create Harmonizer Filete
                                            HFilter.UserID = Session["UserID"].ToString();
                                            HFilter.BPID = Session["BPID"].ToString();
                                            HFilter.SECID = Request.Form[2].ToString();
                                            HFilter.CFLTRID = ReturnCFLTRID;// return id from create filter 
                                            HFilter.FileID = 0;
                                            HFilter.FilterDesc = "";
                                            HFilter.FilterType = "H";
                                            HFilter.SECCODE = template.SECCODE;
                                            ReturnHFLTRID = _fileData.CreateHarmonizerFilter(HFilter);
                                        }
                                        // Create Template
                                        template.FileID = UploadDataInDB;
                                        template.HFLTRID = ReturnHFLTRID;// return id from harmonizer filter
                                        template.BPID = Session["BPID"].ToString();
                                        template.DocExt = fileExt.ToString();
                                        template.UserID = Session["UserID"].ToString();
                                        template.Partner = Session["Partner"].ToString();// Patrer comes from BP Info table on BPID as BPType -- Cutomer/Indivisul/Vendor.
                                        template.FilterType = "H";// should be calculate on behalf of RoleID if RoleID as Admin then H else C (Need discuss)
                                        template.TemplateDesc = Fileupload.FileName;

                                        CreateTemplate = _fileData.CreateTemplate(template);

                                    }
                                    if (FileSucessToProcessed == "")
                                        msg = "File uploaded successfully";
                                    else
                                        msg = FileSucessToProcessed;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DataLogger.Write("FHFile-UploadFileData", "Second:-" + ex.Message);
                    Logger.Write(Session["BPID"].ToString(), "File can not upload from client" + ex.Message + "--" + ex.StackTrace);
                }
            }
            else
            {
                msg = "Template Tag not Found";
            }
            TempData["Message"] = msg;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        private List<SwordAndTagReplace> GetAllFiltersFromFiles()
        {
            List<SwordAndTagReplace> listOfFilters = new List<SwordAndTagReplace>();
            HttpPostedFileBase Fileupload = null;
            string msg = "File not uploaded! Please fill correct data.";
            string SourceFilePath = string.Empty;
            string IdentificationPath = string.Empty;
            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        Fileupload = Request.Files[i];
                        if (Fileupload != null)
                        {
                            var supportedTypes = new[] { "txt", "doc", "docx", "pdf", "xls", "xlsx", "ppt", "pptx", "csv" };
                            var FileExt = System.IO.Path.GetExtension(Fileupload.FileName).Substring(1);
                            if (!supportedTypes.Contains(FileExt))
                            {
                                msg = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT/CSV File";
                            }
                            else
                            {
                                IdentificationPath = Session["UserID"].ToString() + "_" + Request.Form[2].ToString() + "_Temp_" + DateTime.Now.ToString("MMddyyyy_HHmmss") + "_";
                                SourceFilePath = Server.MapPath("~").TrimEnd('\\') + "\\Source\\" + Session["BPID"].ToString() + "\\" + IdentificationPath + System.IO.Path.GetFileName(Fileupload.FileName);
                                Fileupload.SaveAs(SourceFilePath);
                                if (FileExt == ".doc" || FileExt == ".docx" || FileExt == "doc" || FileExt == "docx")
                                {
                                    // List of <TAG> from .docx file
                                    using (FileStream fs = new FileStream(SourceFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                                    {
                                        using (oDocP.WordprocessingDocument doc = oDocP.WordprocessingDocument.Open(fs, true))
                                        {
                                            var document = doc.MainDocumentPart.Document;
                                            var set = new HashSet<char> { '<', '>' };

                                            int[] angularBracketStart = document.InnerText.AllIndexesOf("<").ToArray();
                                            int[] angularBracketClose = document.InnerText.AllIndexesOf(">").ToArray();

                                            for (int j = 0; j < angularBracketStart.Length; j++)
                                            {
                                                string filter = document.InnerText.Slice(angularBracketStart[j], angularBracketClose[j] + 1);
                                                listOfFilters.Add(new SwordAndTagReplace
                                                {
                                                    TagName = filter,
                                                    SearchWord = filter,
                                                    Instruction = "0"
                                                });
                                            }

                                            // For Header part
                                            foreach (var hPart in doc.MainDocumentPart.HeaderParts)
                                            {
                                                var hData = SearchAndReplacer.GetXmlDocument(hPart);
                                                int[] hDataAngularBracketStart = hData.InnerText.AllIndexesOf("<").ToArray();
                                                int[] hDataAngularBracketClose = hData.InnerText.AllIndexesOf(">").ToArray();

                                                for (int j = 0; j < hDataAngularBracketStart.Length; j++)
                                                {
                                                    string filter = hData.InnerText.Slice(hDataAngularBracketStart[j], hDataAngularBracketClose[j] + 1);
                                                    listOfFilters.Add(new SwordAndTagReplace
                                                    {
                                                        TagName = filter,
                                                        SearchWord = filter,
                                                        Instruction = "0"
                                                    });
                                                }
                                            }

                                            // For Footer part
                                            foreach (var fPart in doc.MainDocumentPart.FooterParts)
                                            {
                                                var fData = SearchAndReplacer.GetXmlDocument(fPart);
                                                int[] fDataAngularBracketStart = fData.InnerText.AllIndexesOf("<").ToArray();
                                                int[] fDataAngularBracketClose = fData.InnerText.AllIndexesOf(">").ToArray();

                                                for (int j = 0; j < fDataAngularBracketStart.Length; j++)
                                                {
                                                    string filter = fData.InnerText.Slice(fDataAngularBracketStart[j], fDataAngularBracketClose[j] + 1);
                                                    listOfFilters.Add(new SwordAndTagReplace
                                                    {
                                                        TagName = filter,
                                                        SearchWord = filter,
                                                        Instruction = "0"
                                                    });
                                                }
                                            }

                                            // EndNote
                                            if (doc.MainDocumentPart.EndnotesPart != null)
                                            {
                                                var endNoteData = SearchAndReplacer.GetXmlDocument(doc.MainDocumentPart.EndnotesPart);
                                                int[] endNoteDataAngularBracketStart = endNoteData.InnerText.AllIndexesOf("<").ToArray();
                                                int[] endNoteDataAngularBracketClose = endNoteData.InnerText.AllIndexesOf(">").ToArray();

                                                for (int j = 0; j < endNoteDataAngularBracketStart.Length; j++)
                                                {
                                                    string filter = endNoteData.InnerText.Slice(endNoteDataAngularBracketStart[j], endNoteDataAngularBracketClose[j] + 1);
                                                    listOfFilters.Add(new SwordAndTagReplace
                                                    {
                                                        TagName = filter,
                                                        SearchWord = filter,
                                                        Instruction = "0"
                                                    });
                                                }


                                            }
                                            // FootNote
                                            if (doc.MainDocumentPart.FootnotesPart != null)
                                            {
                                                var footNoteData = SearchAndReplacer.GetXmlDocument(doc.MainDocumentPart.FootnotesPart);
                                                int[] footNoteDataAngularBracketStart = footNoteData.InnerText.AllIndexesOf("<").ToArray();
                                                int[] footNoteDataAngularBracketClose = footNoteData.InnerText.AllIndexesOf(">").ToArray();

                                                for (int j = 0; j < footNoteDataAngularBracketStart.Length; j++)
                                                {
                                                    string filter = footNoteData.InnerText.Slice(footNoteDataAngularBracketStart[j], footNoteDataAngularBracketClose[j] + 1);
                                                    listOfFilters.Add(new SwordAndTagReplace
                                                    {
                                                        TagName = filter,
                                                        SearchWord = filter,
                                                        Instruction = "0"
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (FileExt == ".ppt" || FileExt == ".pptx" || FileExt == "ppt" || FileExt == "pptx")
                                {
                                    // List of <TAG> from .xlsx file
                                    using (FileStream fs = new FileStream(SourceFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                                    {
                                        using (oDocP.SpreadsheetDocument sheet = oDocP.SpreadsheetDocument.Open(fs, true))
                                        {
                                            oDocP.WorkbookPart workbookPart = sheet.WorkbookPart;
                                            oDocP.SharedStringTablePart sstpart = workbookPart.GetPartsOfType<oDocP.SharedStringTablePart>().First();

                                            foreach (oDocS.SharedStringItem item in sstpart.SharedStringTable.Elements<oDocS.SharedStringItem>())
                                            {
                                                if (item.InnerText.Contains('<') && item.InnerText.Contains('>') && !item.InnerText.Contains(" "))
                                                {
                                                    string filter = item.InnerText.Substring(item.InnerText.IndexOf('<'), item.InnerText.IndexOf('>'));
                                                    listOfFilters.Add(new SwordAndTagReplace
                                                    {
                                                        TagName = filter,
                                                        SearchWord = filter,
                                                        Instruction = "0"
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (FileExt == ".xls" || FileExt == ".xlsx" || FileExt == "xls" || FileExt == "xlsx")
                                {
                                    // List of <TAG> from .xlsx file
                                    using (oDocP.PresentationDocument presentationDocument = oDocP.PresentationDocument.Open(SourceFilePath, true))
                                    {
                                        int slideCount = GetCountOfSlides(presentationDocument);
                                        for (int slidId = 0; slidId < slideCount; slidId++)
                                        {
                                            var filters = GetAllTextInSlideForTags(presentationDocument, slidId);
                                            foreach (var filter in filters)
                                            {
                                                string tag = filter.Substring(filter.IndexOf('<'), filter.IndexOf('>'));
                                                listOfFilters.Add(new SwordAndTagReplace
                                                {
                                                    TagName = tag,
                                                    SearchWord = tag,
                                                    Instruction = "0"
                                                });
                                            }
                                        }
                                    }
                                }
                                else if (FileExt == ".csv" || FileExt == "csv")
                                {
                                    var set = new HashSet<char> { '<', '>' };
                                    StreamReader reader = new StreamReader(SourceFilePath);
                                    string source = reader.ReadToEnd();
                                    reader.Close();

                                    int[] angularBracketStart = source.AllIndexesOf("<").ToArray();
                                    int[] angularBracketClose = source.AllIndexesOf(">").ToArray();

                                    for (int j = 0; j < angularBracketStart.Length; j++)
                                    {
                                        string filter = source.Slice(angularBracketStart[j], angularBracketClose[j] + 1);
                                        listOfFilters.Add(new SwordAndTagReplace
                                        {
                                            TagName = filter,
                                            SearchWord = filter,
                                            Instruction = "0"
                                        });
                                    }
                                }
                                else if (FileExt == ".txt" || FileExt == "txt")
                                {
                                    var set = new HashSet<char> { '<', '>' };
                                    StreamReader reader = new StreamReader(SourceFilePath);
                                    string source = reader.ReadToEnd();
                                    reader.Close();

                                    int[] angularBracketStart = source.AllIndexesOf("<").ToArray();
                                    int[] angularBracketClose = source.AllIndexesOf(">").ToArray();

                                    for (int j = 0; j < angularBracketStart.Length; j++)
                                    {
                                        string filter = source.Slice(angularBracketStart[j], angularBracketClose[j] + 1);
                                        listOfFilters.Add(new SwordAndTagReplace
                                        {
                                            TagName = filter,
                                            SearchWord = filter,
                                            Instruction = "0"
                                        });
                                    }
                                }
                                else if (FileExt == ".pdf" || FileExt == "pdf" || FileExt == ".PDF" || FileExt == "PDF")
                                {
                                    using (PdfReader reader = new PdfReader(SourceFilePath))
                                    {
                                        StringBuilder text = new StringBuilder();
                                        ITextExtractionStrategy Strategy = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();

                                        for (int j = 1; j <= reader.NumberOfPages; j++)
                                        {
                                            string source = PdfTextExtractor.GetTextFromPage(reader, j, Strategy);

                                            int[] angularBracketStart = source.AllIndexesOf("<").ToArray();
                                            int[] angularBracketClose = source.AllIndexesOf(">").ToArray();

                                            for (int k = 0; k < angularBracketStart.Length; k++)
                                            {
                                                string filter = source.Slice(angularBracketStart[k], angularBracketClose[k] + 1);
                                                listOfFilters.Add(new SwordAndTagReplace
                                                {
                                                    TagName = filter,
                                                    SearchWord = filter,
                                                    Instruction = "0"
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                            // Remove uploded file
                            if (System.IO.File.Exists(SourceFilePath))
                            {
                                System.IO.File.Delete(SourceFilePath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("FHFile-UploadFileData", "Second:-" + ex.Message);
                Logger.Write(Session["BPID"].ToString(), "File can not upload from client" + ex.Message + "--" + ex.StackTrace);
            }
            return listOfFilters.Where(x => !string.IsNullOrEmpty(x.TagName)).ToList();
        }

        public List<string> GetAllTextInSlideForTags(oDocP.PresentationDocument presentationDocument, int slideIndex)
        {
            // Verify that the presentation document exists.
            if (presentationDocument == null)
            {
                throw new ArgumentNullException("presentationDocument");
            }

            // Verify that the slide index is not out of range.
            if (slideIndex < 0)
            {
                throw new ArgumentOutOfRangeException("slideIndex");
            }

            // Get the presentation part of the presentation document.
            oDocP.PresentationPart presentationPart = presentationDocument.PresentationPart;

            // Verify that the presentation part and presentation exist.
            if (presentationPart != null && presentationPart.Presentation != null)
            {
                // Get the Presentation object from the presentation part.
                oDocPP.Presentation presentation = presentationPart.Presentation;

                // Verify that the slide ID list exists.
                if (presentation.SlideIdList != null)
                {
                    // Get the collection of slide IDs from the slide ID list.
                    DocumentFormat.OpenXml.OpenXmlElementList slideIds =
                        presentation.SlideIdList.ChildElements;

                    // If the slide ID is in range...
                    if (slideIndex < slideIds.Count)
                    {
                        // Get the relationship ID of the slide.
                        string slidePartRelationshipId = (slideIds[slideIndex] as oDocPP.SlideId).RelationshipId;

                        // Get the specified slide part from the relationship ID.
                        oDocP.SlidePart slidePart =
                            (oDocP.SlidePart)presentationPart.GetPartById(slidePartRelationshipId);

                        // Pass the slide part to the next method, and
                        // then return the array of strings that method
                        // returns to the previous method.
                        return GetAllTextInSlideForTags(slidePart);
                    }
                }
            }
            return null;
        }

        public List<string> GetAllTextInSlideForTags(oDocP.SlidePart slidePart)
        {
            // Verify that the slide part exists.
            if (slidePart == null)
            {
                throw new ArgumentNullException("slidePart");
            }

            // Create a new linked list of strings.
            List<string> texts = new List<string>();

            // If the slide exists...
            if (slidePart.Slide != null)
            {
                // Iterate through all the paragraphs in the slide.
                foreach (DocumentFormat.OpenXml.Drawing.Paragraph paragraph in
                    slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Paragraph>())
                {

                    // Iterate through the lines of the paragraph.
                    foreach (DocumentFormat.OpenXml.Drawing.Text text in
                        paragraph.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                    {

                        if (text.InnerText.Contains('<') && text.InnerText.Contains('>') && !text.InnerText.Contains(" "))
                        {
                            texts.Add(text.InnerText);
                        }

                        //Replace here
                        //foreach (var sData in lstSearch)
                        //{
                        //    text.Text = text.Text.Replace(sData.SearchWord, sData.TagName);
                        //}
                    }
                }
            }
            return texts;
        }

        [SessionTimeoutFilter]
        public ActionResult QRCode()
        {
            CommanUserData commanUserData = new CommanUserData();
            commanUserData = _userData.GetCommanData(Session["UserID"].ToString());
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();

                // var  qrCode = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(commanUserData.HarmonizerValue, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    ViewBag.QRCodeImage = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            ////-Nitin Check for expiry of account
            if (TempData["expiredate"] != null)
            {
                ViewBag.ExpireDate = Convert.ToDateTime(TempData["expiredate"]).ToShortDateString();
                TempData.Keep();
            }

            return View();
        }


        ////-Nitin Check for expiry of account
        public ActionResult ExipreActivation()
        {
            ViewBag.token = Request.QueryString["token"];
            return PartialView("_UserActivationMessage");
        }
    }

}