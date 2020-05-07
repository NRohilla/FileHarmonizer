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
   public class Report
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();

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

        public List<CostOfOwnership> GetCostOfOwnershipDetails(string FHnumber)
        {
            List<CostOfOwnership> lstCostOfOwnership = new List<CostOfOwnership>();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_GetCostOfOwnerShipDetails";
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
                        CostOfOwnership costOfOwnership = new CostOfOwnership();
                        costOfOwnership.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        costOfOwnership.FHnumber = Convert.ToString(ds.Tables[0].Rows[i]["FHnumber"]);
                        costOfOwnership.Associate = Convert.ToString(ds.Tables[0].Rows[i]["Associate"]);
                        costOfOwnership.DateProcessed = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateProcessed"]);
                        costOfOwnership.TimeProcessed = (TimeSpan)ds.Tables[0].Rows[i]["TimeProcessed"];
                        costOfOwnership.TransactionName = Convert.ToString(ds.Tables[0].Rows[i]["TransactionName"]);
                        costOfOwnership.Msg = Convert.ToString(ds.Tables[0].Rows[i]["Msg"]);
                        costOfOwnership.TransactionCount = Convert.ToInt32(ds.Tables[0].Rows[i]["Count"]);
                        lstCostOfOwnership.Add(costOfOwnership);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Report-sp_GetCostOfOwnerShipDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstCostOfOwnership;

        }

        public List<string> GetUsers()
        {
            List<string> lstuser = new List<string>();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_GetUsers";
                 DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count > 0 &&
                    ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                   
                        lstuser.Add(ds.Tables[0].Rows[i]["userid"].ToString());
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
            return lstuser;

        }
    }
}
