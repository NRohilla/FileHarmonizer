using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Harmonizer.Core.Model;
using System.ComponentModel;


namespace Harmonizer.DB.Data
{
    public class FHFileData
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();

        public int UploadSourceFile(FileUploadDownload File)
        {
            int retValue = -1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = "sp_CreateFileHistory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = File.UserID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = File.SECID;
                cmd.Parameters.Add("@DataDate", SqlDbType.DateTime).Value = File.DataDate;
                cmd.Parameters.Add("@Time", SqlDbType.NVarChar).Value = File.Time;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = File.CreatedBy;
                cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = File.CreatedDate;
                cmd.Parameters.Add("@OrignalFileName", SqlDbType.NVarChar).Value = File.OrginalFileName;
                cmd.Parameters.Add("@SourceFilePath", SqlDbType.NVarChar).Value = File.SourceFilePath;
                cmd.Parameters.Add("@FileExt", SqlDbType.NVarChar).Value = File.FileExt;
                cmd.Parameters.Add("@IsArchive", SqlDbType.Bit).Value = File.IsArchive;
                cmd.Parameters.Add("@TargetFilePath", SqlDbType.NVarChar).Value = File.TargetFilePath;
                cmd.Parameters.Add("@NewFileName", SqlDbType.NVarChar).Value = File.NewFileName;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = File.BPID;
                cmd.Parameters.Add("@Output", SqlDbType.Int);
                cmd.Parameters["@Output"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                retValue = (int)cmd.Parameters["@Output"].Value;
            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = -1;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-UploadSourceFile", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = -1;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-UploadSourceFile", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;

        }
        public List<FileUploadDownload> GetSourceFile(FileUploadDownload File)
        {
            List<FileUploadDownload> lst = new List<FileUploadDownload>();

            return lst;
        }

        public DataSet GetStandardGlobalTag()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "select ID,UTAGID,UserID,Tag,Orig,GlobPri,Description from tbl_StandardGlobalTag order by ID desc";
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetStandardGlobalTag", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        public DataSet GetRepository()
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand("sp_GetRepository", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetRepository", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public DataSet GetTemplateWithSector(string BPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetTemplateWithSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetTemplateWithSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public DataSet GetBusinessTemplateBPIDOrFH(string BPIDOrFH, string SECID, int FileId, string op, string CurrentBPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetBusinessTemplateBPIDFH";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPIDOrFH", SqlDbType.NVarChar).Value = BPIDOrFH;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = SECID;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileId;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                cmd.Parameters.Add("@CurrentBPID", SqlDbType.NVarChar).Value = CurrentBPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetBusinessTemplateBPIDOrFH", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        public DataSet GetTemplateDetailById(Int32 FileId, string BPId)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetTagNameById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileId;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetTemplateDetailById", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;

        }

        public int StoreSearchReplaceTagHistory(List<SwordAndTagReplace> data, int LastId)
        {
            try
            {
                con = ConnectionClass.getConnection();
                DataTable table = new DataTable();
                table = ConvertToDataTable(data, LastId);
                using (System.Data.SqlClient.SqlBulkCopy bulkCopy = new System.Data.SqlClient.SqlBulkCopy(con))
                {

                    bulkCopy.DestinationTableName = "tbl_SourceFileHistoryTag";
                    bulkCopy.ColumnMappings.Add("FileID", "FileID");
                    bulkCopy.ColumnMappings.Add("SearchWord", "SearchWord");
                    bulkCopy.ColumnMappings.Add("TagName", "TagName");
                    bulkCopy.ColumnMappings.Add("Instruction", "Instruction");
                    bulkCopy.ColumnMappings.Add("FilterType", "FilterType");
                    bulkCopy.WriteToServer(table);

                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-StoreSearchReplaceTagHistory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return -1;
        }


        public static DataTable ConvertToDataTable(List<SwordAndTagReplace> data, int LastId)
        {

            DataTable table = new DataTable();
            try
            {
                table.Columns.Add("FileID", typeof(int));
                table.Columns.Add("SearchWord", typeof(string));
                table.Columns.Add("TagName", typeof(string));
                table.Columns.Add("Instruction", typeof(string));
                table.Columns.Add("FilterType", typeof(string));

                for (int i = 0; i < data.ToList().Count; i++)
                    table.Rows.Add(new object[] {
                            data[i].FileID=LastId,
                            data[i].SearchWord,
                            data[i].TagName,
                            data[i].Instruction,
                            data[i].FilterType="C",
                           });
            }
            catch (Exception ex)
            {
                // close 
                DataLogger.Write("FHFile-ConvertToDataTable", ex.Message);
            }
            return table;
        }

        public int AddTagNameDetails(Tag Objtag)
        {

            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_CreateTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UTAGID", SqlDbType.Int).Value = Result;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = Objtag.UserID;
                cmd.Parameters.Add("@Tag", SqlDbType.NVarChar).Value = Objtag.TagName;
                cmd.Parameters.Add("@Orig", SqlDbType.VarChar).Value = Objtag.Orig;
                cmd.Parameters.Add("@GlobPri", SqlDbType.VarChar).Value = Objtag.GlobPri;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = Objtag.Description;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-AddTagNameDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }
        public int CreateTemplate(Template Template)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteTemplate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = Template.ID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = Template.UserID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = Template.SECID;
                cmd.Parameters.Add("@SECCODE", SqlDbType.NVarChar).Value = Template.SECCODE;
                cmd.Parameters.Add("@FilterType", SqlDbType.Char).Value = Template.FilterType;
                cmd.Parameters.Add("@HFLTRID", SqlDbType.NVarChar).Value = Template.HFLTRID;
                cmd.Parameters.Add("@TEMPNUM", SqlDbType.NVarChar).Value = Template.TEMPNUM;
                cmd.Parameters.Add("@TEMPID", SqlDbType.NVarChar).Value = Template.TEMPID;
                cmd.Parameters.Add("@Partner", SqlDbType.NVarChar).Value = Template.Partner;
                cmd.Parameters.Add("@DocExt", SqlDbType.NVarChar).Value = Template.DocExt;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = Template.BPID;
                cmd.Parameters.Add("@FileID", SqlDbType.NVarChar).Value = Template.FileID;
                cmd.Parameters.Add("@TemplateType", SqlDbType.NVarChar).Value = Template.TemplateType;
                cmd.Parameters.Add("@TemplateDesc", SqlDbType.NVarChar).Value = Template.TemplateDesc;
                cmd.Parameters.Add("@TemplateName", SqlDbType.NVarChar).Value = Template.TemplateName;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Insert";
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-CreateTemplate", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public string CreateFilter(CHFilter Filter)
        {
            string Result = "";
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCreateFilter";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = Filter.ID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = Filter.UserID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = Filter.SECID;
                cmd.Parameters.Add("@SECCODE", SqlDbType.NVarChar).Value = Filter.SECCODE;
                cmd.Parameters.Add("@FilterType", SqlDbType.Char).Value = Filter.FilterType;
                cmd.Parameters.Add("@BPID", SqlDbType.Char).Value = Filter.BPID;
                cmd.Parameters.Add("@FLTRSEC", SqlDbType.NVarChar).Value = Filter.SECCODE;
                cmd.Parameters.Add("@FLTRID", SqlDbType.NVarChar).Value = Filter.FLTRID;
                cmd.Parameters.Add("@FilterDesc", SqlDbType.NVarChar).Value = Filter.FilterDesc;
                cmd.Parameters.Add("@FilterText", SqlDbType.NVarChar).Value = Filter.FilterText;
                cmd.Parameters.Add("@FilterName", SqlDbType.NVarChar).Value = Filter.FilterName;
                cmd.Parameters.Add("@FLTRNUM", SqlDbType.NVarChar).Value = Filter.FLTRNUM;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = Filter.INDID;
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = Filter.CATID;
                cmd.Parameters.Add("@FileID", SqlDbType.NVarChar).Value = Filter.FileID;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Insert";
                cmd.Parameters.Add("@Output", SqlDbType.NVarChar, 50);
                cmd.Parameters["@Output"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Result = (string)cmd.Parameters["@Output"].Value;
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = "";
                DataLogger.Write("FHFile-CreateFilter", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public string CreateHarmonizerFilter(CHFilter HFilter)
        {
            string Result = "";
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteHarmonizerFilter";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = HFilter.ID;
                cmd.Parameters.Add("@BPID", SqlDbType.Char).Value = HFilter.BPID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = HFilter.UserID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = HFilter.SECID;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = HFilter.INDID;
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = HFilter.CATID;
                cmd.Parameters.Add("@CFLTRID", SqlDbType.NVarChar).Value = HFilter.CFLTRID;
                cmd.Parameters.Add("@FLTRSEC", SqlDbType.NVarChar).Value = HFilter.SECCODE;
                cmd.Parameters.Add("@FilterType", SqlDbType.Char).Value = HFilter.FilterType;
                cmd.Parameters.Add("@HFLTRID", SqlDbType.NVarChar).Value = HFilter.HFLTRID;
                cmd.Parameters.Add("@FilterDesc", SqlDbType.NVarChar).Value = HFilter.FilterDesc;
                cmd.Parameters.Add("@FileID", SqlDbType.NVarChar).Value = HFilter.FileID;
                cmd.Parameters.Add("@SECCODE", SqlDbType.NVarChar).Value = HFilter.SECCODE;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Insert";
                cmd.Parameters.Add("@Output", SqlDbType.NVarChar, 50);
                cmd.Parameters["@Output"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Result = (string)cmd.Parameters["@Output"].Value;
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = "";
                DataLogger.Write("FHFile-CreateHarmonizerFilter", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public string GetTAGSECCode(string SecId)
        {
            string TAGSECCode = "";
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT convert(nvarchar, Convert(bigint,TAGSEC)) FROM tbl_TAGSectorID where SECID='" + SecId + "'";
                cmd.CommandType = CommandType.Text;
                TAGSECCode = (string)cmd.ExecuteScalar();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                TAGSECCode = "";
                DataLogger.Write("FHFile-GetTAGSECCode", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return TAGSECCode;
        }
        public DataSet GetTemplateWithSector()
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetTemplateWithSector";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetTemplateWithSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public void UpdateTemplateShareValue(DataTable TemplateShareValue)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_UpdateTemplateShareValue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblRepository", TemplateShareValue);
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-UpdateTemplateShareValue", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
        }


        public void CreateRepository(DataTable repoTable)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_UpdateRepository";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblRepository", repoTable);
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-CreateRepository", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

        }

        public void CreateUpdateShareValue(DataTable repoTable, string BPID, string op)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateBPShareValue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblRepository", repoTable);
                cmd.Parameters.AddWithValue("@BPID", BPID);
                cmd.Parameters.AddWithValue("@op", op);
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-CreateUpdateShareValue", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
        }


        public DataSet GetRepositoryByTemplateID(int TemplateID)
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "SELECT ID,TemplateID,BPID,UserID,UTAGID,Tag ,Description,Org,GlobPri,Share,CreatedDate,UpdateDate FROM tbl_Repository where TemplateID=" + TemplateID;
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetRepositoryByTemplateID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public DataSet GetFileDetails(int FileID)
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "select sf.ID as FileID, sf.fileext,sf.targetfilepath,sf.NewFileName,substring(sf.OrignalFileName, 1, charindex('.', sf.OrignalFileName)-1) As TemplateName,tp.TemplateName as DFileName from tbl_SourceFileHistory sf  Inner Join tbl_Templates tp on tp.FileID=sf.ID where sf.ID=" + FileID;
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetFileDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int SaveHarmonizerTeamplateInfo(int TemplateID, string BPID, string UserID, string TemplateName, string TemplatePath, string comment = "")
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteHarmonizerTemplate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@TemplateID", SqlDbType.BigInt).Value = TemplateID;
                cmd.Parameters.Add("@TemplateName", SqlDbType.NVarChar).Value = TemplateName;
                cmd.Parameters.Add("@TemplatePath", SqlDbType.NVarChar).Value = TemplatePath;
                cmd.Parameters.Add("@NoOfDownloads", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = comment;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Insert";
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-SaveHarmonizerTeamplateInfo", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public DataSet GetTagWithShareData(string BPID, int FileID, string PersonalID, string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetTagNameByFileIDAndPersonalID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
                cmd.Parameters.Add("@PersonalID", SqlDbType.NVarChar).Value = PersonalID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("FHFile-GetTagWithShareData", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int CopyTemplate(int oldFileID, string oldName, string newName, string userId, string BPID)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_CopyTemplate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userId;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@OldFileID", SqlDbType.BigInt).Value = oldFileID;
                cmd.Parameters.Add("@OldTemplateName", SqlDbType.NVarChar).Value = oldName;
                cmd.Parameters.Add("@NewTemplateName", SqlDbType.NVarChar).Value = newName;
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-CopyTemplate", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        //Association Page finctions
        public int CreateAssociation(Association Association)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateAssociation";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FHnumber", SqlDbType.NVarChar).Value = Association.FHnumber;
                cmd.Parameters.Add("@Associate", SqlDbType.NVarChar).Value = Association.Associate;
                //cmd.Parameters.Add("@OriginalDateofAssoc", SqlDbType.DateTime).Value = Association.OriginalDateofAssoc;
                cmd.Parameters.Add("@AssocStatus", SqlDbType.Bit).Value = Association.AssocStatus;
                //cmd.Parameters.Add("@AssocCanceledDate", SqlDbType.DateTime).Value = Association.AssocCanceledDate;
                cmd.Parameters.Add("@AssocCanceledBy", SqlDbType.NVarChar).Value = Association.AssocCanceledBy;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("FHFile-CreateAssociation", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public List<Association> GetAssociation(string FHnumber)
        {
            List<Association> lstAssociation = new List<Association>();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateAssociation";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FHnumber", SqlDbType.NVarChar).Value = FHnumber;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Select";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Association association = new Association();
                        association.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        association.FHnumber = Convert.ToString(ds.Tables[0].Rows[i]["FHnumber"]);
                        association.Associate = Convert.ToString(ds.Tables[0].Rows[i]["Associate"]);
                        association.AssocCanceledBy = Convert.ToString(ds.Tables[0].Rows[i]["AssocCanceledBy"]);
                        association.OriginalDateofAssoc = Convert.ToDateTime(ds.Tables[0].Rows[i]["OriginalDateofAssoc"]);
                        association.AssocCanceledDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["AssocCanceledDate"]);
                        association.AssocStatus = Convert.ToBoolean(ds.Tables[0].Rows[i]["AssocStatus"]);
                        lstAssociation.Add(association);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetAssociationList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstAssociation;

        }


        public int UpdateAssociation(int RecordID)
        {
            int retValue = -1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand("update tbl_Association set assocStatus=1 where ID=" + Convert.ToInt32(RecordID));
                cmd.Connection = con;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                retValue = -1;
                DataLogger.Write("Association-UpdateAssociation", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

        public int RemoveAssociation(int RecordID)
        {
            int retValue = -1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand("update tbl_Association set assocStatus=0 where ID=" + Convert.ToInt32(RecordID));
                cmd.Connection = con;
                retValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                retValue = -1;
                DataLogger.Write("Association-UpdateAssociation", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }
        public string GetAssociationInActiveId(string FHnumber,string Associate)
        {
            string RecordID = "";
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT Convert(varchar,ID) FROM tbl_association where AssocStatus=0 and FHnumber='" + FHnumber + "' and Associate='" + Associate + "'";
                cmd.CommandType = CommandType.Text;
                RecordID = (string)(cmd.ExecuteScalar());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                RecordID = "";
                DataLogger.Write("FHFile-GetTAGSECCode", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return RecordID;
        }

       

    }
}
