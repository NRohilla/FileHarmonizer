using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmonizer.Core.Model;
using System.Data;

namespace Harmonizer.DB.Data
{
   public class Scheme
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();
        public int CreateScheme(UserScheme userScheme,string op)
        {
            int retValue = -1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = userScheme.ID;
                cmd.Parameters.Add("@FHNumber", SqlDbType.NVarChar).Value = userScheme.FHNumber;
                cmd.Parameters.Add("@SchemeNum", SqlDbType.Int).Value = userScheme.SchemeNum;
                cmd.Parameters.Add("@SchemeName", SqlDbType.NVarChar).Value = userScheme.SchemeName;
                cmd.Parameters.Add("@SchemeDescription", SqlDbType.NVarChar).Value = userScheme.SchemeDescription;
                cmd.Parameters.Add("@SchemeType", SqlDbType.NVarChar).Value = userScheme.SchemeType;
                cmd.Parameters.Add("@Client", SqlDbType.BigInt).Value = userScheme.Client;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = userScheme.Name;
                cmd.Parameters.Add("@RegistrationType", SqlDbType.NVarChar).Value = userScheme.RegistrationType;
                cmd.Parameters.Add("@ProjectType", SqlDbType.NVarChar).Value = userScheme.ProjectType;
                cmd.Parameters.Add("@ProjectName", SqlDbType.NVarChar).Value = userScheme.ProjectName;
                cmd.Parameters.Add("@ProjectDescription", SqlDbType.NVarChar).Value = userScheme.ProjectDescription;
                cmd.Parameters.Add("@ProjectStartDate", SqlDbType.DateTime).Value = userScheme.ProjectStartDate;
                cmd.Parameters.Add("@ProjectEndDate", SqlDbType.DateTime).Value = userScheme.ProjectEndDate;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                cmd.Parameters.Add("@Suggestion", SqlDbType.NVarChar).Value = userScheme.Suggestion;
                retValue= cmd.ExecuteNonQuery();
              
            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = -1;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-CreateScheme", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = -1;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-CreateScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;

        }

        public int GetSchemeValue(string BPID, string op)
        {
            int retValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                var obj = cmd.ExecuteScalar();
                retValue = (obj == null ? 0 : Convert.ToInt32(obj));

            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-CreateScheme", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-CreateScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;
        }

        public DataSet GetAllSchemeByBPID(string BPID, string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-GetAllSchemeByFHNumber", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int ArchiveSchemeAndDependentFilter(int schemenum ,string BPID,string op)
        {
            int retValue = 1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_ArchiveSchemeAndFilterTempalte";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@Scheme", SqlDbType.Int).Value = schemenum;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                retValue = cmd.ExecuteNonQuery();

            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-UpdateComment", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-UpdateComment", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;
        }


        public int UpdateComment(int ID,string Comment, string op)
        {
            int retValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                cmd.Parameters.Add("@SchemeComment", SqlDbType.NVarChar).Value = Comment;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                retValue = cmd.ExecuteNonQuery();

            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-UpdateComment", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-UpdateComment", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;
        }

        public int DeleteOrArchiveScheme(int ID, string op)
        {
            int retValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                retValue = cmd.ExecuteNonQuery();

            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-DeleteOrArchiveScheme", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-DeleteOrArchiveScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;
        }

        public DataSet GetScheme(int ID,string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-GetScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        public int CheckBPIDExist(string BPID, string op)
        {
            int retValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteScheme";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                var obj = cmd.ExecuteScalar();
                retValue = (obj == null ? 0 : 1);
            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-DeleteOrArchiveScheme", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                retValue = 0;

                // Write exception in log file 
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-DeleteOrArchiveScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return retValue;
        }

        public DataSet GetSchemeDetailsByID(int ID, string BPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetSchemeDetailWithFilterAndTemplate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Scheme-GetScheme", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
    }
}
