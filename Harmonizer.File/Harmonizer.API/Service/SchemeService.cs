using Harmonizer.API.Models;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Service
{
  
    public class SchemeService
    {
        Scheme _scheme = new Scheme();
        UserData _userData = new UserData();

        public long GetShemeSRNo(string BPID)
        {
            return _scheme.GetSchemeValue(BPID, "getschemevalue");
        }

        public int CreateScheme(UserScheme userScheme)
        {
            return _scheme.CreateScheme(userScheme, "insert");

        }

        public List<UserScheme> GetAllSchemeData(string BPID)
        {
            UserScheme userScheme = new UserScheme();
            DataSet ds = new DataSet();
            List<UserScheme> lst = new List<UserScheme>();
            ds = _scheme.GetAllSchemeByBPID(BPID, "selectall");
            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID = Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                    lst.Add(userScheme);
                }
            }
            return lst;
        }

        public int UpdateSchemeComment(int ID, string Comment)
        {
            return _scheme.UpdateComment(ID, Comment, "updatecommnet");
        }

        public int DeleteScheme(int ID)
        {
          return  _scheme.DeleteOrArchiveScheme(ID, "delete");
        }

        public int ArchiveScheme(List<int> LstID, string BPID)
        {
            int retValue=-1;
            foreach (var d in LstID.ToList())
                retValue = _scheme.ArchiveSchemeAndDependentFilter(d, BPID, "archivescheme");

            return retValue;
        }

        public SchemeDetails ViewScheme(int ID, string BPID)
        {
            SchemeDetails lstScheme = new SchemeDetails();
            DataSet ds = new DataSet();
            UserScheme userScheme = new UserScheme();

            ds = _scheme.GetScheme(ID, "select");

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID = Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                }
            }

            lstScheme.SchemeInfo = userScheme;
            lstScheme.SchemeDetail = GetSchemeDetailsByID(ID, BPID);
            return lstScheme;
        }

        public List<UserScheme> GetSchemeDetailsByID(int ID, string BPID)
        {
            UserScheme userScheme = new UserScheme();
            List<UserScheme> lst = new List<UserScheme>();
            DataSet ds = _scheme.GetSchemeDetailsByID(ID, BPID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.FHNumber = dr["FHNumber"].ToString();
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.Sector = dr["SECID"].ToString();
                    userScheme.FilterName = dr["FilterName"].ToString();
                    userScheme.HarmonizeName = dr["HarmonizeName"].ToString();
                    userScheme.TemplateName = dr["TemplateName"].ToString();
                    lst.Add(userScheme);
                }
            }
            return lst;
        }

        public int UpadateScheme(UserScheme userScheme)
        {
            return _scheme.CreateScheme(userScheme, "updatescheme");
        }

        public List<UserScheme> ArchiveSchemeData(string BPID)
        {
            UserScheme userScheme = new UserScheme();
            DataSet ds = new DataSet();
            List<UserScheme> lst = new List<UserScheme>();
            ds = _scheme.GetAllSchemeByBPID(BPID, "getarchive");
            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    userScheme = new UserScheme();
                    userScheme.ID = Convert.ToInt32(dr["ID"]);
                    userScheme.SchemeNum = Convert.ToInt32(dr["SchemeNum"]);
                    userScheme.SchemeName = dr["SchemeName"].ToString();
                    userScheme.SchemeDescription = dr["SchemeDescription"].ToString();
                    userScheme.SchemeType = dr["SchemeType"].ToString();
                    userScheme.Client = Convert.ToInt64(dr["Client"]);
                    userScheme.Name = dr["Name"].ToString();
                    userScheme.RegistrationType = dr["RegistrationType"].ToString();
                    userScheme.ProjectType = dr["ProjectType"].ToString();
                    userScheme.ProjectName = dr["ProjectName"].ToString();
                    userScheme.ProjectDescription = dr["ProjectDescription"].ToString();
                    userScheme.SchemeComment = dr["SchemeComment"].ToString();
                    userScheme.ProjectStartDate = Convert.ToDateTime(dr["ProjectStartDate"]);
                    userScheme.ProjectEndDate = Convert.ToDateTime(dr["ProjectEndDate"]);
                    userScheme.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    userScheme.IsArchive = Convert.ToBoolean(dr["IsArchive"]);
                    userScheme.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
                    userScheme.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    userScheme.Suggestion = dr["Suggestion"].ToString();
                    lst.Add(userScheme);
                }
            }
            return lst;
        }

        public int UnArchiveScheme(int ID, string BPID)// ID Scheme No.
        {
            return _scheme.ArchiveSchemeAndDependentFilter(ID, BPID, "unarchivescheme");
        }

        public int CheckBPIDExist(string BPID)
        {
           return _scheme.CheckBPIDExist(BPID, "clientbpidexist");
        }
    }
}