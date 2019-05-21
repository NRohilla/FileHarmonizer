using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
namespace Harmonizer.API.Service
{
    public class FHFileService
    {
        UserData _userData = new UserData();
        FHFileData _fileData = new FHFileData();
        public List<Tag> ViewAllTag(string BPID,string UserID)
        {
            List<Tag> lstTag = new List<Tag>();
            Tag _tag = new Tag();
            DataSet ds = new DataSet();
            ds = _userData.GetPrivateTag(BPID, UserID, "selectUserTag");
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
            lstTag= _userData.AutoCalculateValue(lstTag);

            //return Task.FromResult(lstTag);// when use Tast at top as Tas<List<Tag>>
            return lstTag;
        }

        public List<CreateListTemplate> GetAllExternalTemplate(string BPIDOrFH,string SecID, string BPID)
        {
            List<CreateListTemplate> lstTemp = new List<CreateListTemplate>();
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
            return lstTemp;
        }

        public List<Tag> GetBusinessFilter(string BPIDOrFH, string SecID, int FileID,string BPID)
        {
            DataSet ds = new DataSet();
            Tag _tag = new Tag();
            List<Tag> lstTag = new List<Tag>();
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

            return lstTag;
        }

        public void SaveRepositoryInfo(DataTable dt,string BPID)
        {
            _fileData.UpdateTemplateShareValue(dt);// Update share as per template
            _fileData.CreateRepository(dt);// Update in repository
            _fileData.CreateUpdateShareValue(dt, BPID, "Update");// Update common share value
        }

        public List<CreateListTemplate> GetTemplateDataForFinalTemplate(string BPID)
        {
            List<CreateListTemplate> lstTemp = new List<CreateListTemplate>();
            DataSet ds = _fileData.GetTemplateWithSector(BPID);
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
            return lstTemp;
        }

        public List<Tag> GetTemplateDetail(int FileID,string BPID, string UserID)
        {
            List<Tag> lstTag = new List<Tag>();
            Tag objtag = new Tag();
            DataSet ds = _fileData.GetTemplateDetailById(FileID, BPID);
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

                //TemplateName = ds.Tables[0].Rows[0]["TEMPID"].ToString() + " " + ds.Tables[0].Rows[0]["TemplateDesc"].ToString();
                //FilterName = ds.Tables[0].Rows[0]["CFLTRID"].ToString() + " " + ds.Tables[0].Rows[0]["TemplateDesc"].ToString();
            }
            return lstTag = _userData.AutoCalculateValue(lstTag);
        }
    }
}