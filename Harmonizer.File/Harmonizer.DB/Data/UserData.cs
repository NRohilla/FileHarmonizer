using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Harmonizer.Core.Model;

namespace Harmonizer.DB.Data
{
   public  class UserData
    {
        public static string constr = ConfigurationManager.AppSettings["FHConnStr"].ToString();
        SqlConnection con = new SqlConnection();

        public  bool CheckUserByUserID(string UserID)
        {
            string strSql = "select * from tbl_User where UserID='"+UserID+"' ";
            con = ConnectionClass.getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSql, con);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        public  bool CheckUserByEmailID(string EmailID)
        {
            string strSql = "select * from tbl_User where EmailID='" + EmailID + "' ";
            con = ConnectionClass.getConnection();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(strSql, con);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        public bool CreateUser(User user, AddressIinformation addInfo, PersonalInformation personalInfo)
        {
            int result = -1;
            bool returnValue = false;

            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = "sp_CreateUser";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = user.UserID;
            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = user.EmailID;
            cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.PasswordHash;
            cmd.Parameters.Add("@PasswordHasVAlue", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);//user.PasswordHasValue;
            cmd.Parameters.Add("@InitialPassword", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.InitialPassword;
            cmd.Parameters.Add("@CodeVersion", SqlDbType.NVarChar).Value = "1.0";

            // personal 
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = personalInfo.Title;
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = personalInfo.FirstName;
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = personalInfo.LastName;
            cmd.Parameters.Add("@Name2", SqlDbType.NVarChar).Value = personalInfo.Name2;
            cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = personalInfo.MiddleName;
            cmd.Parameters.Add("@LastName2", SqlDbType.NVarChar).Value = personalInfo.LastName2;
            cmd.Parameters.Add("@AKA", SqlDbType.NVarChar).Value = personalInfo.AKA;
            cmd.Parameters.Add("@Initials", SqlDbType.NVarChar).Value = personalInfo.Initials;
            cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = personalInfo.Country;
            cmd.Parameters.Add("@Profession", SqlDbType.NVarChar).Value = personalInfo.Profession;
            cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = personalInfo.Gender;
            cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = personalInfo.Language;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = personalInfo.Phone;
            // Address
            cmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = addInfo.CompName;
            cmd.Parameters.Add("@CompName2", SqlDbType.NVarChar).Value = addInfo.CompName2;
            cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = addInfo.Department;
            cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = addInfo.Address1;
            cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = addInfo.Address2;
            cmd.Parameters.Add("@POBox", SqlDbType.NVarChar).Value = addInfo.POBox;
            cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = addInfo.City;
            cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = addInfo.State;
            cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = addInfo.Zip;
            cmd.Parameters.Add("@AddCountry", SqlDbType.NVarChar).Value = addInfo.Country;
            cmd.Parameters.Add("@CountryKey", SqlDbType.NVarChar).Value = addInfo.CountryKey;
            cmd.Parameters.Add("@AddLanguage", SqlDbType.NVarChar).Value = addInfo.Language;
            cmd.Parameters.Add("@AddPhone", SqlDbType.NVarChar).Value = addInfo.Phone;
            cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = addInfo.WorkPhone;
            cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = addInfo.MobilePhone;
            cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = addInfo.Fax;
            cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar).Value = addInfo.TimeZone;
            cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = addInfo.Location;
            cmd.Parameters.Add("@DistrictPostalCode", SqlDbType.NVarChar).Value = addInfo.DistrictPostalCode;

            try
            {
                result= cmd.ExecuteNonQuery();
               
                if (result == 3)
                    returnValue = true;
                else
                    returnValue = false;
            }
            catch(SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file 

            }
            catch(Exception ex)
            {
                // Base exception
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file 
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return returnValue;
        }

        public User GetUserDetailByUserIDAndPassword(string userID , string password)
        {
            User user = new User();
            con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = "sp_VerifyUserLogin";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;
            cmd.Parameters.Add("@Password", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(password);
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // UserID,EmailID,IsActive,ExpireDate,ActiveDate,RoleID 
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //RR_ID.Add(int.Parse(row.ItemArray.GetValue(0).ToString()));
                        user.UserID = row["UserID"].ToString();
                        user.EmailID = row["EmailID"].ToString();
                        user.IsActive = Convert.ToBoolean(row["IsActive"]);
                        user.ExpireDate = Convert.ToDateTime(row["ExpireDate"]);
                        user.ActiveDate = Convert.ToDateTime(row["ActiveDate"]);
                        user.Role = Convert.ToInt32(row["RoleID"]);
                    }
                }
                else
                    user = null;

            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
            }
            return user;
        }
        
        public int ResetPassword(User user)
        {
            int result = -1;

            //sp_Resetpassword
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = "sp_Resetpassword";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = user.UserID;
            cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = user.EmailID;
            cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);
            cmd.Parameters.Add("@PasswordHasVAlue", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);
            try
            {
               
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                // write in log file, sql exception 
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                // write in log file,
                ConnectionClass.closeconnection(con);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public void LoginHistory(LoginHistory logHist)
        {

            //sp_LoginHistory
           con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "sp_LoginHistory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = logHist.UserID;
                cmd.Parameters.Add("@TerminalID", SqlDbType.NVarChar).Value = logHist.TerminalID;
                cmd.Parameters.Add("@Server", SqlDbType.Binary, 16).Value = logHist.Server;
                cmd.Parameters.Add("@TerminalIP", SqlDbType.Binary, 16).Value = logHist.TerminalIP;
                cmd.Parameters.Add("@LogonLang", SqlDbType.Binary, 16).Value = "EN";
                // update data
                cmd.ExecuteNonQuery();
            }
            catch
            {
                ConnectionClass.closeconnection(con);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
        }
    }
}
