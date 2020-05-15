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
        public List<TagViewModel> GetTagInfo(TagModel tagModel)
        {
            List<TagViewModel> lstTagModel = new List<TagViewModel>();
            DataSet ds = new DataSet();
            for (int i = 0; i < tagModel.Tags.Count; i++)
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
                    cmd.Parameters.Add("@TagID", SqlDbType.NVarChar).Value = tagModel.Tags[i];
                    cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = "Select";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count > 0 &&
                      ds.Tables[0].Rows.Count > 0)
                    {
                        for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                        {
                            TagViewModel tagModels = new TagViewModel();
                            tagModels.FHnumber = ds.Tables[0].Rows[k]["FHNumber"].ToString();
                            tagModels.TagID = ds.Tables[0].Rows[k]["UTAGID"].ToString();
                            tagModels.ShareValue = ds.Tables[0].Rows[k]["Share"].ToString();
                            var checkData = lstTagModel.FirstOrDefault(x => x.FHnumber == tagModels.FHnumber && x.TagID == tagModels.TagID);
                            if (checkData == null)
                                lstTagModel.Add(tagModels);
                        }
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

            return lstTagModel;
        }

        public bool CheckAPIByAPIKey(string APIkey)
        {
            bool retValue = false;
            try
            {
                string strSql = "select * from tbl_APIkeyData where IsActive=1 and APIKey='" + APIkey + "' order by ID desc";
                con = ConnectionClass.getConnection();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
                if (ds.Tables[0].Rows.Count > 0)
                    retValue = true;
                else
                    retValue = false;
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("TagData-CheckAPIByAPIKey", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

    }
}
