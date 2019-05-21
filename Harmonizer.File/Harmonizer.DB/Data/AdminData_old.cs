using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmonizer.Core.Model;
using Harmonizer.DB;


namespace Harmonizer.DB.Data
{
   public class AdminData_old
    {
        SqlConnection con = new SqlConnection();
        public List<Country> GetCountryList()
        {
            List<Country> lst = new List<Country>();
           
            string strSql = "select ID,Alpha3,Alpha2,UNCode,Country from tbl_Country";
            con = ConnectionClass.getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSql, con);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Country lstcountry = new Country();
                    lstcountry.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                    lstcountry.CountryName = ds.Tables[0].Rows[i]["Country"].ToString();
                    lstcountry.UNCode = ds.Tables[0].Rows[i]["UNCode"].ToString();
                    lstcountry.Alpha2= ds.Tables[0].Rows[i]["Alpha2"].ToString();
                    lstcountry.Alpha3= ds.Tables[0].Rows[i]["Alpha3"].ToString();
                    lst.Add(lstcountry);

                }
            }
            return lst;

        }


        public string InsertCountryDetails(Country ObjCountry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCountry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ObjCountry.ID;
                cmd.Parameters.Add("@Alpha3", SqlDbType.NVarChar).Value = ObjCountry.Alpha3;
                cmd.Parameters.Add("@Alpha2", SqlDbType.NVarChar).Value = ObjCountry.Alpha2;
                cmd.Parameters.Add("@UNCode", SqlDbType.BigInt).Value = ObjCountry.UNCode;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = ObjCountry.CountryName;
                cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar).Value = ObjCountry.CreatedBy;
                cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar).Value = ObjCountry.UpdatedBy;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = ObjCountry.Operation;
                Result = Convert.ToString(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                Result = ex.Message;
            }
            return Result;

        }
        public List<Sector> GetSectorList()
        {
            List<Sector> lstSector = new List<Sector>();

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "sp_InsertUpdateDeleteSector";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
            cmd.Parameters.Add("@SECCode", SqlDbType.NVarChar);
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
            cmd.Parameters.Add("@TAGSEC", SqlDbType.NVarChar);
            cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = 2; // ActionId 2 for get Sector list
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Sector lstSec = new Sector();
                    lstSec.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                    lstSec.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                    lstSec.SECCode = Convert.ToString(ds.Tables[0].Rows[i]["SECCode"]);
                    lstSec.Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                    lstSector.Add(lstSec);

                }
            }
            return lstSector;

        }

        public string InsertSectorDetails(Sector objSector)
        {

            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = objSector.SECID;
                cmd.Parameters.Add("@SECCode", SqlDbType.NVarChar).Value = objSector.SECCode;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objSector.Description;
                cmd.Parameters.Add("@TAGSEC", SqlDbType.NVarChar).Value = objSector.TAGSEC;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = objSector.ActionId;
                Result = Convert.ToString(cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                Result = ex.Message;
            }
            return Result;

        }

    }
}
