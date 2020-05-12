using Harmonizer.Core;
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
    public class TagData
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();
        public TagModel GetTagInfo(TagModel tagModel)
        {
            DataSet ds = new DataSet();
            for (int i= 0;i<tagModel.Tags.Count;i++)
            {
                try
                {
                    con = ConnectionClass.getConnection();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "sp_GetTagValues";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FHnumber", SqlDbType.NVarChar).Value = tagModel.FHnumber;
                    cmd.Parameters.Add("@Associate", SqlDbType.NVarChar).Value = tagModel.AssociateNumber;
                    //cmd .Parameters.Add("@TimeProcessed", SqlDbType.Time).Value = costOfOwnership.TimeProcessed;
                    cmd.Parameters.Add("@TagID", SqlDbType.NVarChar).Value = tagModel.Tags[i];

                    cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = "Select";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0 &&
                      ds.Tables[0].Rows.Count > 0)
                    {


                        tagModel.FHnumber = ds.Tables[0].Rows[0]["FHNumber"].ToString();
                        tagModel.Tags[i] = ds.Tables[0].Rows[0]["UTAGID"].ToString();
                        tagModel.ShareValue.Add(ds.Tables[0].Rows[0]["Share"].ToString());

                    }
                    ConnectionClass.closeconnection(con);
                }
                catch (Exception ex)
                {
                    ConnectionClass.closeconnection(con);

                    DataLogger.Write("TagData-GetTAgInfo", ex.Message);
                }
                finally
                {
                    ConnectionClass.closeconnection(con);
                }
            }
          
            return tagModel;
        }

    }
}
