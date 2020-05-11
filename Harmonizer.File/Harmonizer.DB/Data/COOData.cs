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
   public  class COOData
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();


        public int CreateCostOfOwnership(string FHnumber,string Associate,string TransactionName,string Msg,int TransactionCount,string DateProcessed)
        {
            int Result = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertCostOfOwnership";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@FHnumber", SqlDbType.NVarChar).Value = FHnumber;
                cmd.Parameters.Add("@Associate", SqlDbType.NVarChar).Value = Associate;
                //cmd.Parameters.Add("@TimeProcessed", SqlDbType.Time).Value = costOfOwnership.TimeProcessed;
                cmd.Parameters.Add("@TransactionName", SqlDbType.NVarChar).Value = TransactionName;
                cmd.Parameters.Add("@Msg", SqlDbType.NVarChar).Value =Msg;
                cmd.Parameters.Add("@Count", SqlDbType.BigInt).Value = TransactionCount;
                cmd.Parameters.Add("@DateProcessed", SqlDbType.Date).Value = DateProcessed;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
                Result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = 0;
                DataLogger.Write("COOData-InsertCostofOwnership", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }
    }
}
