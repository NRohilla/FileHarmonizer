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
    public class APIData
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();

        public int InsertAPIKey(string UserID, string APIKey)
        {

            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetUserAPIKey";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserID;
                cmd.Parameters.Add("@APIKey", SqlDbType.NVarChar).Value = APIKey;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = "Update";
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("API-InsertAPIKey", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }

        public string GetAPIKey(string UserID)
        {
            string APIKey = "";
            SqlCommand com = new SqlCommand();
            try
            {
                string strSql = "select APIKey from tbl_User where UserID='" + UserID + "' ";
                con = ConnectionClass.getConnection();
                com = new SqlCommand(strSql, con);
                APIKey = (string)com.ExecuteScalar();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetExpiredate", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return APIKey;
        }
    }
}
