using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.DB.Data
{
    public class FHManage
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();


        public DataSet GetFilterTemplateDetailsByBPID(string BPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetFilterTemplateDetailsForManage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHMnage-GetFilterTemplateDetailsByBPID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public DataSet GetFilterByBPID(string BPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetFileterByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHMnage-GetFilterByBPID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        public int UpdateHarmonizeCommnet(int ID,string  Commennt)
        {
            int retValue = -1;
            try
            {
                string sql = "Update tbl_HarminizedTemplete set Comment='" + Commennt + "', UpdatedDate=GETDATE() where ID='" + ID + "'";
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHMnage-UpdateHarmonizeCommnet", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }
        public int UpdateFilterComment(CHFilter filter)
        {
            int retValue = -1;
            try
            {
                string sql = "Update tbl_CreateFilters set FilterDesc='" + filter.FilterDesc + "',FilterText='" + filter.FilterText + "', UpdatedDate=GETDATE() where FLTRID='" + filter.FLTRID + "'";
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHMnage-UpdateFilterComment", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

        public int DeleteFilter(int FileID, string BPID, string FLTRID, string Op)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_DeleteFilter";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FLTRID", SqlDbType.NVarChar).Value = FLTRID;
                cmd.Parameters.Add("@FileID", SqlDbType.Int).Value = FileID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OP", SqlDbType.NVarChar).Value = Op;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHMnage-DeleteFilter", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }
        public int UpdateTemplateComment(Template template)
        {
            int retValue = -1;
            try
            {
                string sql = "Update tbl_Templates set TemplateDesc='" + template.TemplateDesc + "',TemplateText='" + template.TemplateText + "', UpdateDate=GETDATE(),InternalExternal='" + template.InternalExternal.Trim() + "' where FileID='" + template.FileID + "'";
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                retValue = -1;
                DataLogger.Write("FHMnage-UpdateTemplateComment", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

        public int ArchiveTemplate(int FileID, string BPID, string Op)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_DeleteFilter";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.Int).Value = FileID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OP", SqlDbType.NVarChar).Value = Op;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHMnage-ArchiveTemplate", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public int RestoreDelatedTemplateFile(List<int> lstFileID, string BPID, string Op)
        {
            int Result = 0;
            try
            {
                string lstStringFileID = changeFileIDListWithString(lstFileID);
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_RestoreTemplates";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LstFileID", SqlDbType.NVarChar).Value = lstStringFileID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OP", SqlDbType.NVarChar).Value = Op;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHMnage-RestoreDelatedTemplateFile", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public int ArchiveTemplateFileAll(List<int> lstFileID, string BPID, string Op)
        {
            int Result = 0;
            try
            {
                string lstStringFileID = changeFileIDListWithString(lstFileID);
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_ArchiveAllSelectedTemplates";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LstFileID", SqlDbType.NVarChar).Value = lstStringFileID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OP", SqlDbType.NVarChar).Value = Op;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHMnage-ArchiveTemplateFileAll", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public int MergeTempateFile(List<int> lstFileID, string BPID, string Op)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.NVarChar).Value = changeFileIDListWithString(lstFileID);
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OP", SqlDbType.NVarChar).Value = Op;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHMnage-MergeTempateFile", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string changeFileIDListWithString(List<int> lstFileID)
        {
            string listRecord = "";
            foreach (var iteam in lstFileID)
                listRecord += iteam.ToString() + ",";
            return listRecord.TrimEnd(',');

        }


        public string GetFHFileID(string FileID,string op)
        {
            string sql = "";
            string returnData = "";
            try
            {
                if (op == "F")
                    sql = "select TargetFilePath from tbl_SourceFileHistory where ID='" + FileID + "'";
                else
                    sql = "select TemplatePath from tbl_HarminizedTemplete where ID='" + FileID + "'";

                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                returnData = (string)cmd.ExecuteScalar();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                returnData = "";
                DataLogger.Write("FHMnage-GetFHFileID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return returnData;
        }


        public int UpdateAssignScheme(int schemeNum, string FLTRID,string  BPID)
        {
            int retValue = -1;
            try
            {
                string sql = "Update tbl_CreateFilters set AssignScheme='"+ schemeNum + "' where FLTRID='"+ FLTRID + "' and BPID='"+ BPID + "'";
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHMnage-UpdateHarmonizeCommnet", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

    }
}
