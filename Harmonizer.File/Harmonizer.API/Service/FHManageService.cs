using Harmonizer.API.Models;
using Harmonizer.API.Utility;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Service
{
    public class FHManageService
    {
        FHManage _fhManage = new FHManage();
        public List<ManageFilterTemplateModel> TemplateFileDetails(string UserID,string BPID,string HostName,string ProtNo)
        {
            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            var lstFilterTemplateNew = GetFiletTemplateManageData(BPID, HostName, ProtNo).Where(x => x.Template.IsDeleted == false && x.Template.IsArchive == false).ToList();
            lstFilterTemplate = lstFilterTemplateNew.Distinct(new NoDuplicateTemplate()).ToList(); ;
            return lstFilterTemplate;
        }

        public List<ManageFilterTemplateModel> ManageHarmonizeTemplateDetails(string UserID, string BPID, string HostName, string ProtNo,string FHNumber)
        {
            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            var lstFilterTemplateNew = GetFiletTemplateManageData(BPID, HostName, ProtNo);
            if (FHNumber == "")
            {
                lstFilterTemplate = lstFilterTemplateNew.Where(x => x.HarmonizeTemplate.TemplatePath != "" && !string.IsNullOrWhiteSpace(x.HarmonizeTemplate.TemplatePath) && x.HarmonizeTemplate.ID != 0 && x.HarmonizeTemplate.IsArchive == false).ToList();
            }
            else
            {
                lstFilterTemplate = lstFilterTemplateNew.Where(x => x.HarmonizeTemplate.TemplatePath != "" && !string.IsNullOrWhiteSpace(x.HarmonizeTemplate.TemplatePath) && x.HarmonizeTemplate.ID != 0).ToList();
                lstFilterTemplate = lstFilterTemplate.Where(x => x.HarmonizeTemplate.HTFHNumber == FHNumber && x.HarmonizeTemplate.IsArchive == false).ToList();
            }
            return lstFilterTemplate;
        }

        public List<ManageFilterTemplateModel> GetFiletTemplateManageData(string BPID, string HostName, string ProtNo)
        {
            string fullPathUrlTemplate = "";
            string fullPathUrlHarmonized = "";
            string host = HostName;
            string port = ProtNo;
            string rootDomain = ConfigurationManager.AppSettings["rootDomain"].ToString();
            if (!string.IsNullOrEmpty(port))
            {
                fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + ":" + port + "/Target/" + BPID + "/";
                fullPathUrlHarmonized = rootDomain + host.TrimEnd('/') + ":" + port + "/Harmonized/" +BPID + "/";
            }
            else
            {
                fullPathUrlTemplate = rootDomain + host.TrimEnd('/') + "/Target/" + BPID + "/";
                fullPathUrlHarmonized = rootDomain + host.TrimEnd('/') + "/Harmonized/" + BPID + "/";
            }
            Template _template = new Template();
            CHFilter _chFilter = new CHFilter();
            HarmonizeTemplate _harmonizeTemplate = new HarmonizeTemplate();
            FileUploadDownload _fileUploadDownload = new FileUploadDownload();
            List<ManageFilterTemplateModel> lstFilterTemplate = new List<ManageFilterTemplateModel>();
            DataSet ds = _fhManage.GetFilterTemplateDetailsByBPID(BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // Add template data
                    _template = new Template
                    {
                        BPID = row["BPID"].ToString(),
                        HFLTRID = row["HFLTRID"].ToString(),
                        TEMPID = row["TEMPID"].ToString(),
                        Partner = row["Partner"].ToString(),
                        TemplateType = row["TemplateType"].ToString(),
                        TemplateDesc = row["TemplateDesc"].ToString(),
                        TemplateName = row["TemplateName"].ToString(),
                        DocExt = row["DocExt"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["TempCDate"]),
                        UpdatedDate = Convert.ToDateTime(row["TempUDate"]),
                        FileID = Convert.ToInt32(row["FileID"]),
                        SECCODE = row["TempSECCode"].ToString(),
                        TemplateText = row["TemplateText"].ToString(),
                        IsDeleted = Convert.ToBoolean(row["TempDeleted"]),
                        IsArchive = Convert.ToBoolean(row["TempArchive"]),
                        InternalExternal = row["InternalExternal"].ToString()
                    };
                    // Add Filter data
                    _chFilter = new CHFilter
                    {
                        SECID = row["CSECID"].ToString(),
                        FLTRNUM = row["CFILTERNUM"].ToString(),
                        FLTRID = row["CFLTERID"].ToString(),
                        FilterDesc = row["FilterDesc"].ToString(),
                        FilterText = row["FilterText"].ToString(),
                        FilterName = row["FilterName"].ToString(),
                        CreatedDate = Convert.ToDateTime(row["CFCDate"]),
                        UpdatedDate = Convert.ToDateTime(row["CFUDate"]),
                        AssignScheme = Convert.ToInt32(row["AssignScheme"])
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
                        Template = _template,
                        Filter = _chFilter,
                        FileUploadDownload = _fileUploadDownload,
                        HarmonizeTemplate = _harmonizeTemplate
                    });
                }
            }
            return lstFilterTemplate;
        }

        public int ArchiveTemplate(List<int> lstFileID,string BPID)
        {
            int retValue = 0;
            string Op = "ArchiveTemplate";
            retValue = _fhManage.ArchiveTemplateFileAll(lstFileID, BPID, Op);
            return retValue;
        }
        public int UpdateAssignScheme(int schemenum, string FLTRID, string BPID)
        {
            int retValue = -1;
            retValue = _fhManage.UpdateAssignScheme(schemenum, FLTRID, BPID);
            return retValue;
        }

        public int DeleteTemplate(int FileID,string BPID,string FLTRID)
        {
          return  _fhManage.DeleteFilter(FileID, BPID, FLTRID, "Template");
        }

        public int RenameTemplate(int FileID, string TemplateText, string Description, string HFLTRID, string IE, int op = 0)
        {
            int retValue = 0;
            Template _template = new Template();
            _template.FileID = FileID;
            _template.TemplateDesc = Description;
            _template.TemplateText = TemplateText;
            _template.HFLTRID = HFLTRID;
            _template.InternalExternal = IE;
            if (op == 1)
            {
                // Update Template
                retValue = _fhManage.UpdateTemplateComment(_template);
                return retValue;
            }
            else
            {
                return retValue;
            }
        }

        public int UpdateHarmonizeCommnet(int ID, string Comment)
        {
            int retValue = -1;
            try
            {
                retValue = _fhManage.UpdateHarmonizeCommnet(ID, Comment);
            }
            catch
            {
                retValue = -1;
            }
            return retValue;
        }


    }
}