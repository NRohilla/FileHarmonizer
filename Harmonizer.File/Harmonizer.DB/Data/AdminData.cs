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
    public class AdminData
    {
        SqlConnection con = new SqlConnection();

        #region Country CRUD

        public List<Country> GetCountryList()
        {
            List<Country> lst = new List<Country>();

            string strSql = "select ID,Alpha3,Alpha2,UNCode,Country from tbl_Country";
            con = ConnectionClass.getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSql, con);
            da.Fill(ds);
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Country lstcountry = new Country();
                        lstcountry.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        lstcountry.CountryName = ds.Tables[0].Rows[i]["Country"].ToString();
                        lstcountry.UNCode = ds.Tables[0].Rows[i]["UNCode"].ToString();
                        lstcountry.Alpha2 = ds.Tables[0].Rows[i]["Alpha2"].ToString();
                        lstcountry.Alpha3 = ds.Tables[0].Rows[i]["Alpha3"].ToString();
                        lst.Add(lstcountry);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetCountryList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lst;
        }

        public string UpdateCountryDetails(Country ObjCountry)
        {
            string Result = string.Empty;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
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
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateCountryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;

        }

        public string InsertCountryDetails(Country ObjCountry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
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
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertCountryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string DeleteCountry(Country ObjCountry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteCountry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UNCode", SqlDbType.BigInt).Value = ObjCountry.UNCode;

                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = ObjCountry.Operation;
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteCountry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        #endregion

        #region Sector CRUD

        public string UpdateSectorDetails(Sector sector)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = sector.ID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = sector.SECID;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = sector.Description;
                cmd.Parameters.Add("@SECCode", SqlDbType.NVarChar).Value = sector.SECCode;
                cmd.Parameters.Add("@TAGSEC", SqlDbType.NVarChar).Value = sector.TAGSEC;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = sector.ActionId;
                Result = Convert.ToString(cmd.ExecuteNonQuery());

                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateSectorDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string DeleteSector(Sector objSector)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = objSector.SECID;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = -1; // ActionId -1 to delete sector
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public List<Sector> GetSectorList()
        {
            List<Sector> lstSector = new List<Sector>();
            try
            {
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
                        lstSec.TAGSEC = Convert.ToString(ds.Tables[0].Rows[i]["TAGSEC"]);
                        lstSector.Add(lstSec);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetSectorList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstSector;
        }

        public List<Sector> GetSector()
        {
            List<Sector> lstSector = new List<Sector>();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Industry", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GetSector";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Sector lstSec = new Sector();
                        lstSec.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        lstSec.SECCode = Convert.ToString(ds.Tables[0].Rows[i]["SECCode"]);
                        lstSector.Add(lstSec);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
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
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = objSector.SECID;
                cmd.Parameters.Add("@SECCode", SqlDbType.NVarChar).Value = objSector.SECCode;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objSector.Description;
                cmd.Parameters.Add("@TAGSEC", SqlDbType.NVarChar).Value = objSector.TAGSEC;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = objSector.ActionId;
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertSectorDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        #endregion

        #region Industry CRUD

        public string InsertIndustryDetails(Industry ObjIndustry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ObjIndustry.ID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = ObjIndustry.SECID;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = ObjIndustry.INDID;
                cmd.Parameters.Add("@Industry", SqlDbType.NVarChar).Value = ObjIndustry.IndustryName;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertIndustryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string UpdateIndustryDetails(Industry ObjIndustry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ObjIndustry.ID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = ObjIndustry.SECID;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = ObjIndustry.INDID;
                cmd.Parameters.Add("@Industry", SqlDbType.NVarChar).Value = ObjIndustry.IndustryName;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Update";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateIndustryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string DeleteIndustry(Industry objIndustry)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = objIndustry.INDID;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Delete";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteIndustry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public List<Industry> GetIndustryList()
        {
            List<Industry> lstIndustry = new List<Industry>();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Industry", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Select";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Industry lstInd = new Industry();
                        lstInd.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        lstInd.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        lstInd.INDID = Convert.ToString(ds.Tables[0].Rows[i]["INDID"]);
                        lstInd.Share = Convert.ToString(ds.Tables[0].Rows[i]["Share"]);
                        lstInd.IndustryName = Convert.ToString(ds.Tables[0].Rows[i]["Industry"]);
                        lstIndustry.Add(lstInd);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetIndustryList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstIndustry;
        }

        #endregion

        #region Category CRUD

        public string InsertCategoryDetails(Category ObjCategory)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ObjCategory.ID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = ObjCategory.SECID;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = ObjCategory.INDID;
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = ObjCategory.CATID;
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar).Value = ObjCategory.CategoryName;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertCategoryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string UpdateCategoryDetails(Category ObjCategory)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ObjCategory.ID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = ObjCategory.SECID;
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar).Value = ObjCategory.INDID;
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar).Value = ObjCategory.CATID;
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar).Value = ObjCategory.CategoryName;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Update";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateCategoryDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string DeleteCategory(Category objCategory)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.NVarChar).Value = objCategory.ID;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Delete";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteCategory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public List<Category> GetCategoryList()
        {
            List<Category> lstCategory = new List<Category>();

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Select";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Category lstCat = new Category();
                        lstCat.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        lstCat.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        lstCat.INDID = Convert.ToString(ds.Tables[0].Rows[i]["Industry"]);
                        lstCat.CATID = Convert.ToString(ds.Tables[0].Rows[i]["CATID"]);
                        lstCat.Share = Convert.ToString(ds.Tables[0].Rows[i]["Share"]);
                        lstCat.CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["Category"]);
                        lstCategory.Add(lstCat);
                    }
                }

                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetCategoryList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstCategory;
        }

        public List<Sector> GetSectorFromCategory()
        {
            List<Sector> lstSector = new List<Sector>();

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GetSector";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Sector lstSec = new Sector();
                        lstSec.SECID = Convert.ToString(ds.Tables[0].Rows[i]["SECID"]);
                        lstSec.SECCode = Convert.ToString(ds.Tables[0].Rows[i]["SECCode"]);
                        lstSector.Add(lstSec);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetSectorFromCategory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstSector;
        }

        public List<Industry> GetIndustry()
        {
            List<Industry> lstIndustry = new List<Industry>();

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@INDID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@CATID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Category", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GetIndustry";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Industry lstInd = new Industry();
                        lstInd.INDID = Convert.ToString(ds.Tables[0].Rows[i]["INDID"]);
                        lstInd.IndustryName = Convert.ToString(ds.Tables[0].Rows[i]["Industry"]);
                        lstIndustry.Add(lstInd);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetIndustry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstIndustry;
        }

        #endregion

        #region Standard Tag CRUD

        public string InsertTagDetails(Tag ObjTag)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteStandardGlobalTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UTAGID", SqlDbType.BigInt).Value = ObjTag.UTAGID;
                cmd.Parameters.Add("@Tag", SqlDbType.NVarChar).Value = ObjTag.TagName;
                cmd.Parameters.Add("@GlobPri", SqlDbType.NVarChar).Value = ObjTag.GlobPri;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ObjTag.Description;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Insert";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertTagDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string UpdateTagDetails(Tag ObjTag)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteStandardGlobalTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UTAGID", SqlDbType.BigInt).Value = ObjTag.UTAGID;
                cmd.Parameters.Add("@Tag", SqlDbType.NVarChar).Value = ObjTag.TagName;
                cmd.Parameters.Add("@GlobPri", SqlDbType.NVarChar).Value = ObjTag.GlobPri;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = ObjTag.Description;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Update";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateTagDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public string DeleteTag(Tag ObjTag)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteStandardGlobalTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UTAGID", SqlDbType.BigInt).Value = ObjTag.UTAGID;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Delete";
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteTag", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public List<Tag> GetTagList()
        {
            List<Tag> lstStandardtag = new List<Tag>();

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteStandardGlobalTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar);
                cmd.Parameters.Add("@UTAGID", SqlDbType.BigInt);
                cmd.Parameters.Add("@UserID", SqlDbType.BigInt);
                cmd.Parameters.Add("@Tag", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Orig", SqlDbType.BigInt);
                cmd.Parameters.Add("@GlobPri", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar);
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "Select";
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Tag lstTag = new Tag();
                        lstTag.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        lstTag.BPID = Convert.ToString(ds.Tables[0].Rows[i]["BPID"]);
                        lstTag.UTAGID = Convert.ToInt32(ds.Tables[0].Rows[i]["UTAGID"]);
                        lstTag.UserID = Convert.ToString(ds.Tables[0].Rows[i]["UserID"]);
                        lstTag.TagName = Convert.ToString(ds.Tables[0].Rows[i]["Tag"]);
                        lstTag.Orig = Convert.ToString(ds.Tables[0].Rows[i]["Orig"]);
                        lstTag.GlobPri = Convert.ToString(ds.Tables[0].Rows[i]["GlobPri"]);
                        lstTag.Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                        lstStandardtag.Add(lstTag);
                    }
                }
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetTagList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstStandardtag;
        }

        public int UploadStandardTag(DataTable dt)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataStandardTag";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblStandardTag", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-UploadStandardTag", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int UploadSector(DataTable dt)
        {
            try
            {
              int deleteDatabeforedUpload=  DeleteMassData("sector");
            }
            catch
            {

            }

            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataSector";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblSector", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-UploadSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int UploadCategory(DataTable dt)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataCategory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblCategory", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-UploadCategory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int UploadIndustry(DataTable dt)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataIndustry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblIndustry", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-UploadIndustry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }
        public int UploadCountry(DataTable dt)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataCountry";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblCountry", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-UploadCountry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }
        public int DeleteMassData(string op)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_DeleteMassData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@op", op);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-DeleteMassData", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }


        #endregion

        #region Plan details
        public DataSet GetAllPlan(string op)
        {
            DataSet ds = new DataSet();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDelectPlan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetAllPlan", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        #endregion


        public int CreatePlan(string Title,string Description,string FreeInfo,decimal Cost,int Validity, string group, string planfor,  string op)
        {
            //Title,Description,FreeInfo ,Cost,ValidatityMonth , group, planfor(1-Weekly/2-Monthly/3-Yearly)
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDelectPlan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@FreeInfo", FreeInfo);
                cmd.Parameters.AddWithValue("@Cost", Cost);
                cmd.Parameters.AddWithValue("@Validity", Validity);
                cmd.Parameters.AddWithValue("@Group", group);
                cmd.Parameters.AddWithValue("@WeekMonthYear", planfor);
                cmd.Parameters.AddWithValue("@op", op);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-CreatePlan", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int UpateDeletePlan(string Title, string Description, decimal Cost, int ID,bool status,int order,string op)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDelectPlan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@Description", Description);
                //cmd.Parameters.AddWithValue("@FreeInfo", FreeInfo);
                cmd.Parameters.AddWithValue("@Cost", Cost);
                //cmd.Parameters.AddWithValue("@ValidatityMonth", ValidatityMonth);
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@IsActive", status);
                cmd.Parameters.AddWithValue("@OrderDisplay", order);
                cmd.Parameters.AddWithValue("@op", op);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-CreatePlan", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }


        public PlanDetails GetPlanByPlanId(string planId)
        {
            PlanDetails planDetails = new PlanDetails();
            DataSet ds = new DataSet();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDelectPlan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = "select";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetAllPlan", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds.Tables[0].DefaultView.RowFilter = "ID = '" + planId + "'";
                    DataTable dt = (ds.Tables[0].DefaultView).ToTable();
                    foreach (DataRow row in dt.Rows)
                    {
                        planDetails.Cost = Convert.ToDecimal(row["Cost"]);
                        planDetails.Title = row["Title"].ToString();
                        planDetails.Description = row["Description"].ToString();
                        planDetails.ID = Convert.ToInt32(row["ID"]);
                        planDetails.Validity = Convert.ToInt32(row["Validity"]);
                        planDetails.PlanFor = Convert.ToInt32(row["WeekMonthYear"]);
                    }
                }
            }
            catch(Exception ex)
            {
                DataLogger.Write("Admin-GetAllPlan", ex.Message);

            }


            return planDetails;

        }
        public int CreateLanguage(LanguageTimeZone languageTimeZone,string op)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteLanguage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", languageTimeZone.ID);
                cmd.Parameters.AddWithValue("@Language", languageTimeZone.Language);
                cmd.Parameters.AddWithValue("@Country", languageTimeZone.Country);
                cmd.Parameters.AddWithValue("@Code", languageTimeZone.Code);
                cmd.Parameters.AddWithValue("@Name", languageTimeZone.Name);
                cmd.Parameters.AddWithValue("@op", op);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-CreatePlan", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public DataSet GetLanguageList()
        {
            DataSet ds = new DataSet();
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteLanguage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = "select";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("GetLanguageList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }


        public int DeleteLanguage(int ID,string op)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteLanguage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@op", op);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-DeleteLanguage", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int UploadLanguage(DataTable dt)
        {
            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMaasDataLanguage";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblLanguage", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = 0;
                DataLogger.Write("Admin-UploadLanguage", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public List<PaymentModel> GetPaymentDetails(string BPID, string op)
        {
            PaymentModel obj = new PaymentModel();
            List<PaymentModel> objPayment = new List<PaymentModel>();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetPaymentDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        obj = new PaymentModel();
                        obj.BPID = Convert.ToString(ds.Tables[0].Rows[i]["BPID"]);
                        obj.id = Convert.ToString(ds.Tables[0].Rows[i]["PayId"]);
                        obj.state = Convert.ToString(ds.Tables[0].Rows[i]["TransactionStatus"]);
                        obj.failuarreasion = Convert.ToString(ds.Tables[0].Rows[i]["FailuarReasion"]);
                        obj.invoice = Convert.ToString(ds.Tables[0].Rows[i]["InvoiceId"]);
                        obj.TransactionId = Convert.ToString(ds.Tables[0].Rows[i]["PayerId"]);
                        obj.amount = Convert.ToString(ds.Tables[0].Rows[i]["Amount"]);
                        objPayment.Add(obj);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["TransactionType"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["Currency"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["TransactionDate"]);
                        //objPayment.Sector = Convert.ToString(ds.Tables[0].Rows[i]["PayerId"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["PlanId"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["Plan Title"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["ValideStartDate"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["ValideEndDate"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["IsActive"]);
                        //objPayment.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["CreateDate"]);
                    }
                }

                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("AdminData-GetPaymentDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return objPayment;
        }

        public List<TierDetails> GetTierList()
        {
            List<TierDetails> lstTierPlan = new List<TierDetails>();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetTierPlans";
                cmd.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TierDetails lstTier = new TierDetails();
                        lstTier.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"]);
                        lstTier.UserCount = Convert.ToInt32(ds.Tables[0].Rows[i]["UserCount"]);
                        lstTier.Tier = Convert.ToInt32(ds.Tables[0].Rows[i]["Tier"]);
                        lstTier.PartnerType = Convert.ToString(ds.Tables[0].Rows[i]["PartnerType"]);
                        lstTier.BusinessPartners = Convert.ToString(ds.Tables[0].Rows[i]["BusinessPartners"]);
                        lstTier.Title = Convert.ToString(ds.Tables[0].Rows[i]["Title"]);
                        lstTier.Description = Convert.ToString(ds.Tables[0].Rows[i]["Description"]);
                        lstTier.MonthlyCost = Convert.ToDecimal(ds.Tables[0].Rows[i]["MonthlyCost"]);
                        lstTier.AnnualCost = Convert.ToDecimal(ds.Tables[0].Rows[i]["AnnualCost"]);
                        lstTier.PerUserCost = Convert.ToDecimal(ds.Tables[0].Rows[i]["PerUserCost"]);
                        lstTier.UserCareFlag = Convert.ToBoolean(ds.Tables[0].Rows[i]["UserCareFlag"]);
                        lstTier.UserCareValue = Convert.ToDecimal(ds.Tables[0].Rows[i]["UserCareValue"]);
                        lstTier.PerUserCostwithCareValue = Convert.ToDecimal(ds.Tables[0].Rows[i]["PerUserCostwithCareValue"]);
                        lstTier.MonthlyCostWithCare = Convert.ToDecimal(ds.Tables[0].Rows[i]["MonthlyCostWithCare"]);
                        lstTierPlan.Add(lstTier);
                    }   
                }
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetTierList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstTierPlan;
        }

        public string DeleteTier(TierDetails objTier)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteTier";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objTier.ID;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = -1; // ActionId -1 to tier delete
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-DeleteTier", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }


        public string InsertTierDetails(TierDetails objTier)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteTier";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objTier.ID;
                cmd.Parameters.Add("@UserCount", SqlDbType.Int).Value = objTier.UserCount;
                cmd.Parameters.Add("@Tier", SqlDbType.Int).Value = objTier.Tier;
                cmd.Parameters.Add("@PartnerType", SqlDbType.NVarChar).Value = objTier.PartnerType;
                cmd.Parameters.Add("@BusinessPartners", SqlDbType.NVarChar).Value = objTier.BusinessPartners;
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = objTier.Title;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objTier.Description;
                cmd.Parameters.Add("@MonthlyCost", SqlDbType.Decimal).Value = objTier.MonthlyCost;
                cmd.Parameters.Add("@AnnualCost", SqlDbType.Decimal).Value = objTier.AnnualCost;
                cmd.Parameters.Add("@PerUserCost", SqlDbType.Decimal).Value = objTier.PerUserCost;
                cmd.Parameters.Add("@UserCareFlag", SqlDbType.Bit).Value = objTier.UserCareFlag;
                cmd.Parameters.Add("@UserCareValue", SqlDbType.Decimal).Value = objTier.UserCareValue;
                cmd.Parameters.Add("@PerUserCostwithCareValue", SqlDbType.Decimal).Value = objTier.PerUserCostwithCareValue;
                cmd.Parameters.Add("@MonthlyCostWithCare", SqlDbType.Decimal).Value = objTier.MonthlyCostWithCare;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = 0;
                Result = Convert.ToString(cmd.ExecuteNonQuery());
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-InsertTierDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }
        public string UpdateTierDetails(TierDetails objTier)
        {
            string Result = string.Empty;

            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.Parameters.Clear();
                cmd.CommandText = "sp_InsertUpdateDeleteTier";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objTier.ID;
                cmd.Parameters.Add("@UserCount", SqlDbType.Int).Value = objTier.UserCount;
                cmd.Parameters.Add("@Tier", SqlDbType.Int).Value = objTier.Tier;
                cmd.Parameters.Add("@PartnerType", SqlDbType.NVarChar).Value = objTier.PartnerType;
                cmd.Parameters.Add("@BusinessPartners", SqlDbType.NVarChar).Value = objTier.BusinessPartners;
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = objTier.Title;
                cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = objTier.Description;
                cmd.Parameters.Add("@MonthlyCost", SqlDbType.Decimal).Value = objTier.MonthlyCost;
                cmd.Parameters.Add("@AnnualCost", SqlDbType.Decimal).Value = objTier.AnnualCost;
                cmd.Parameters.Add("@PerUserCost", SqlDbType.Decimal).Value = objTier.PerUserCost;
                cmd.Parameters.Add("@UserCareFlag", SqlDbType.Bit).Value = objTier.UserCareFlag;
                cmd.Parameters.Add("@UserCareValue", SqlDbType.Decimal).Value = objTier.UserCareValue;
                cmd.Parameters.Add("@PerUserCostwithCareValue", SqlDbType.Decimal).Value = objTier.PerUserCostwithCareValue;
                cmd.Parameters.Add("@MonthlyCostWithCare", SqlDbType.Decimal).Value = objTier.MonthlyCostWithCare;
                cmd.Parameters.Add("@ActionId", SqlDbType.Int).Value = 1;
                Result = Convert.ToString(cmd.ExecuteNonQuery());

                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                Result = ex.Message;
                DataLogger.Write("Admin-UpdateTierDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return Result;
        }

        public int UploadTier(DataTable dt)
        {
            try
            {
                int deleteDatabeforedUpload = DeleteMassData("Tier");
            }
            catch
            {

            }

            int result = 0;
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_UploadMassDataTier";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tblTier", dt);
                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-UploadTier", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

    }
}
