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
        FHFileData _fileData = new FHFileData();
        public  bool CheckUserByUserID(string UserID)
        {
            bool retValue = false;
            try
            {
                string strSql = "select * from tbl_User where UserID='" + UserID + "' ";
                con = ConnectionClass.getConnection();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
                if (ds.Tables[0].Rows.Count > 0)
                    retValue= true;
                else
                    retValue= false;
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-CheckUserByUserID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

        public  bool CheckUserByEmailID(string EmailID)
        {
            bool retValue = false;
            try
            {
                string strSql = "select * from tbl_User where EmailID='" + EmailID + "' ";
                con = ConnectionClass.getConnection();
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
                if (ds.Tables[0].Rows.Count > 0)
                    retValue= true;
                else
                    retValue= false;
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-CheckUserByEmailID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return retValue;
        }

        //public bool CreateUser(User user, AddressIinformation addInfo, PersonalInformation personalInfo,BPInfo bpInfo)
        //{
        //    Random rand = new Random();
        //    int result = -1;
        //    bool returnValue = false;
        //    long FHNumner= (long)(rand.NextDouble() * 9000000000) + 1000000000;
        //    string FHIDValue = "";
        //    try
        //    {
        //        if (user.Role == 5)
        //            FHIDValue = "ME" + addInfo.CountryKey + addInfo.State + (bpInfo.CompanyDate.HasValue == true ? bpInfo.CompanyDate.Value.ToString("yyyy") : "") + FHNumner.ToString();
        //        else
        //            FHIDValue = "ME" + addInfo.CountryKey + addInfo.State + (personalInfo.DateOfBorn.HasValue == true ? personalInfo.DateOfBorn.Value.ToString("yyyy") : "") + FHNumner.ToString();



        //        con = ConnectionClass.getConnection();
        //        SqlCommand cmd = new SqlCommand();

        //        cmd.Connection = con;
        //        cmd.CommandText = "sp_CreateUser";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = user.UserID;
        //        cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = user.EmailID;
        //        cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.PasswordHash;
        //        cmd.Parameters.Add("@PasswordHasVAlue", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);//user.PasswordHasValue;
        //        cmd.Parameters.Add("@InitialPassword", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.InitialPassword;
        //        cmd.Parameters.Add("@CodeVersion", SqlDbType.NVarChar).Value = "1.0";
        //        cmd.Parameters.Add("@RegistrationType", SqlDbType.NVarChar).Value = user.RegistrationType;
        //        cmd.Parameters.Add("@IndustryShareID", SqlDbType.NVarChar).Value = user.IndustryShareID;
        //        cmd.Parameters.Add("@Role", SqlDbType.Int).Value = user.Role;
        //        cmd.Parameters.Add("@BPType", SqlDbType.NVarChar).Value = user.BPType;
        //        cmd.Parameters.Add("@Partner", SqlDbType.NVarChar).Value = user.Partner;

        //        //****************************** personal **************** 

        //        cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = personalInfo.Title;
        //        cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = personalInfo.FirstName;
        //        cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = personalInfo.LastName;
        //        cmd.Parameters.Add("@Name2", SqlDbType.NVarChar).Value = personalInfo.Name2;
        //        cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = personalInfo.MiddleName;
        //        cmd.Parameters.Add("@LastName2", SqlDbType.NVarChar).Value = personalInfo.LastName2;
        //        cmd.Parameters.Add("@AKA", SqlDbType.NVarChar).Value = personalInfo.AKA;
        //        cmd.Parameters.Add("@Initials", SqlDbType.NVarChar).Value = personalInfo.Initials;
        //        cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = personalInfo.DefaulCountry; //Removed and rename as default 
        //        cmd.Parameters.Add("@Profession", SqlDbType.NVarChar).Value = personalInfo.Profession;
        //        cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = personalInfo.Gender;
        //        cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = personalInfo.Language;
        //        cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = personalInfo.Phone;
        //        // Add new 
        //        cmd.Parameters.Add("@DateOfBorn", SqlDbType.DateTime).Value = personalInfo.DateOfBorn;
        //        cmd.Parameters.Add("@BirthCountry", SqlDbType.NVarChar).Value = personalInfo.BirthCountry;

        //        // cmd.Parameters.Add("@PesonalWorkPhone", SqlDbType.NVarChar).Value = personalInfo.WorkPhone;
        //        // cmd.Parameters.Add("@PesonalMobile", SqlDbType.NVarChar).Value = personalInfo.Mobile;
        //        // cmd.Parameters.Add("@PesonalFax", SqlDbType.NVarChar).Value = personalInfo.Fax;

        //        cmd.Parameters.Add("@PesonalEmail", SqlDbType.NVarChar).Value = personalInfo.Email;
        //        cmd.Parameters.Add("@MiddelInitials", SqlDbType.NVarChar).Value = personalInfo.MiddelInitials;
        //        cmd.Parameters.Add("@PesonalWebsite", SqlDbType.NVarChar).Value = personalInfo.Website;


        //        //****************************** Address ****************

        //        cmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = bpInfo.CompName; //addInfo.CompName;
        //        cmd.Parameters.Add("@CompName2", SqlDbType.NVarChar).Value = addInfo.CompName2;
        //        cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = bpInfo.Department; //addInfo.Department
        //        cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = addInfo.Address1;
        //        cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = addInfo.Address2;
        //        cmd.Parameters.Add("@POBox", SqlDbType.NVarChar).Value = addInfo.POBox;
        //        cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = addInfo.City;
        //        cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = addInfo.State;
        //        cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = addInfo.Zip;
        //        cmd.Parameters.Add("@AddCountry", SqlDbType.NVarChar).Value = addInfo.Country;
        //        cmd.Parameters.Add("@CountryKey", SqlDbType.NVarChar).Value = addInfo.CountryKey;
        //        // removed from interface 11/12/2018 data should be from business info / BP Info
        //        cmd.Parameters.Add("@AddLanguage", SqlDbType.NVarChar).Value = addInfo.Language;
        //        if (user.RegistrationType == "BusinessPartner")
        //        {
        //            cmd.Parameters.Add("@AddPhone", SqlDbType.NVarChar).Value = bpInfo.SecondPhone;
        //            cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = bpInfo.MainPhone;
        //            cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = "";// not at time
        //            cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = bpInfo.Fax;
        //        }
        //        else
        //        {
        //            cmd.Parameters.Add("@AddPhone", SqlDbType.NVarChar).Value = personalInfo.Phone;
        //            cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = personalInfo.WorkPhone;
        //            cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = personalInfo.Mobile;
        //            cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = personalInfo.Fax;
        //        }
        //        // end
        //        cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar).Value = addInfo.TimeZone;
        //        cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = addInfo.Location;
        //        cmd.Parameters.Add("@DistrictPostalCode", SqlDbType.NVarChar).Value = addInfo.DistrictPostalCode;
        //        // Add new 11/12/2018
        //        cmd.Parameters.Add("@county", SqlDbType.NVarChar).Value = addInfo.county;

        //        // Business partner info Add new 11/12/2018
        //        cmd.Parameters.Add("@BPCompName", SqlDbType.NVarChar).Value = bpInfo.CompName;
        //        cmd.Parameters.Add("@BPContactFirstName", SqlDbType.NVarChar).Value = bpInfo.ContactNameFirst;
        //        cmd.Parameters.Add("@BPContactLastName", SqlDbType.NVarChar).Value = bpInfo.ContactNameLast;
        //        cmd.Parameters.Add("@BPCompanyEIN", SqlDbType.NVarChar).Value = bpInfo.CompanyEIN;
        //        cmd.Parameters.Add("@BPCompanyDate", SqlDbType.DateTime).Value = bpInfo.CompanyDate;
        //        cmd.Parameters.Add("@BPLanguage", SqlDbType.NVarChar).Value = bpInfo.Language;
        //        cmd.Parameters.Add("@BPCountry", SqlDbType.NVarChar).Value = bpInfo.Country;
        //        cmd.Parameters.Add("@BPEmail", SqlDbType.NVarChar).Value = bpInfo.Email;
        //        cmd.Parameters.Add("@BPWebsite", SqlDbType.NVarChar).Value = bpInfo.Website;
        //        cmd.Parameters.Add("@BPDepartment", SqlDbType.NVarChar).Value = bpInfo.Department;
        //        cmd.Parameters.Add("@BPTollFreePhone", SqlDbType.NVarChar).Value = bpInfo.TollFreePhone;
        //        cmd.Parameters.Add("@FHNumber", SqlDbType.BigInt).Value = FHNumner;
        //        cmd.Parameters.Add("@FHIDValue", SqlDbType.NVarChar).Value = FHIDValue;


        //        result = cmd.ExecuteNonQuery();
        //        CreateTableForRepositoryATTimeCreateUser(user, addInfo, personalInfo, bpInfo, user.BPID, user.UserID);
        //        if (result >= 3)
        //            returnValue = true;
        //        else
        //            returnValue = false;
        //    }
        //    catch (SqlException sqlEx)
        //    {
        //        // Get orginal value from raise error by sql
        //        returnValue = false;
        //        ConnectionClass.closeconnection(con);
        //        // Write exception in log file 

        //        DataLogger.Write("UserData-CreateUser", sqlEx.Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        // Base exception
        //        returnValue = false;
        //        ConnectionClass.closeconnection(con);
        //        // Write exception in log file
        //        DataLogger.Write("UserData-CreateUser", ex.Message);
        //    }
        //    finally
        //    {
        //        ConnectionClass.closeconnection(con);
        //    }

        //    return returnValue;
        //}

        public bool CreateUser(User user, AddressIinformation addInfo, PersonalInformation personalInfo, BPInfo bpInfo)
        {
            Random rand = new Random();
            int result = -1;
            bool returnValue = false;
            string FHid;

            string FHNumber = FindFHValue();
            char CheckDigit = FindCheckDigit();
            if (user.Role == 5)
            {
                FHid = FindFHid(user.DefaultG, user.DefaultPL, addInfo.CountryKey, addInfo.City, addInfo.State, (bpInfo.CompanyDate.HasValue == true ? bpInfo.CompanyDate.Value.ToString("yyyy") : ""), "", FHNumber, CheckDigit);
            }
            else
            {
                FHid = FindFHid(user.DefaultG, user.DefaultPL, addInfo.CountryKey, addInfo.City, addInfo.State, (personalInfo.DateOfBorn.HasValue == true ? personalInfo.DateOfBorn.Value.ToString("yyyy") : ""), personalInfo.Gender, FHNumber, CheckDigit);
            }
            
            try
            {
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
                cmd.Parameters.Add("@RegistrationType", SqlDbType.NVarChar).Value = user.RegistrationType;
                cmd.Parameters.Add("@IndustryShareID", SqlDbType.NVarChar).Value = user.IndustryShareID;
                cmd.Parameters.Add("@Role", SqlDbType.Int).Value = user.Role;
                cmd.Parameters.Add("@BPType", SqlDbType.NVarChar).Value = user.BPType;
                cmd.Parameters.Add("@Partner", SqlDbType.NVarChar).Value = user.Partner;

                //***************************** personal *************** 

                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = personalInfo.Title;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = personalInfo.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = personalInfo.LastName;
                cmd.Parameters.Add("@Name2", SqlDbType.NVarChar).Value = personalInfo.Name2;
                cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = personalInfo.MiddleName;
                cmd.Parameters.Add("@LastName2", SqlDbType.NVarChar).Value = personalInfo.LastName2;
                cmd.Parameters.Add("@AKA", SqlDbType.NVarChar).Value = personalInfo.AKA;
                cmd.Parameters.Add("@Initials", SqlDbType.NVarChar).Value = personalInfo.Initials;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = personalInfo.DefaulCountry; //Removed and rename as default 
                cmd.Parameters.Add("@Profession", SqlDbType.NVarChar).Value = personalInfo.Profession;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = personalInfo.Gender;
                cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = personalInfo.Language;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = personalInfo.Phone;
                // Add new 
                cmd.Parameters.Add("@DateOfBorn", SqlDbType.DateTime).Value = personalInfo.DateOfBorn;
                cmd.Parameters.Add("@BirthCountry", SqlDbType.NVarChar).Value = personalInfo.BirthCountry;

                // cmd.Parameters.Add("@PesonalWorkPhone", SqlDbType.NVarChar).Value = personalInfo.WorkPhone;
                // cmd.Parameters.Add("@PesonalMobile", SqlDbType.NVarChar).Value = personalInfo.Mobile;
                // cmd.Parameters.Add("@PesonalFax", SqlDbType.NVarChar).Value = personalInfo.Fax;

                cmd.Parameters.Add("@PesonalEmail", SqlDbType.NVarChar).Value = personalInfo.Email;
                cmd.Parameters.Add("@MiddelInitials", SqlDbType.NVarChar).Value = personalInfo.MiddelInitials;
                cmd.Parameters.Add("@PesonalWebsite", SqlDbType.NVarChar).Value = personalInfo.Website;


                //***************************** Address ***************

                cmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = bpInfo.CompName; //addInfo.CompName;
                cmd.Parameters.Add("@CompName2", SqlDbType.NVarChar).Value = addInfo.CompName2;
                cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = bpInfo.Department; //addInfo.Department
                cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = addInfo.Address1;
                cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = addInfo.Address2;
                cmd.Parameters.Add("@POBox", SqlDbType.NVarChar).Value = addInfo.POBox;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = addInfo.City;
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = addInfo.State;
                cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = addInfo.Zip;
                cmd.Parameters.Add("@AddCountry", SqlDbType.NVarChar).Value = addInfo.Country;
                cmd.Parameters.Add("@CountryKey", SqlDbType.NVarChar).Value = addInfo.CountryKey;
                // removed from interface 11/12/2018 data should be from business info / BP Info
                cmd.Parameters.Add("@AddLanguage", SqlDbType.NVarChar).Value = addInfo.Language;
                if (user.RegistrationType == "BusinessPartner")
                {
                    cmd.Parameters.Add("@AddPhone", SqlDbType.NVarChar).Value = bpInfo.SecondPhone;
                    cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = bpInfo.MainPhone;
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = "";// not at time
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = bpInfo.Fax;
                }
                else
                {
                    cmd.Parameters.Add("@AddPhone", SqlDbType.NVarChar).Value = personalInfo.Phone;
                    cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = personalInfo.WorkPhone;
                    cmd.Parameters.Add("@MobilePhone", SqlDbType.NVarChar).Value = personalInfo.Mobile;
                    cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = personalInfo.Fax;
                }
                // end
                cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar).Value = addInfo.TimeZone;
                cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = addInfo.Location;
                cmd.Parameters.Add("@DistrictPostalCode", SqlDbType.NVarChar).Value = addInfo.DistrictPostalCode;
                // Add new 11/12/2018
                cmd.Parameters.Add("@county", SqlDbType.NVarChar).Value = addInfo.county;

                // Business partner info Add new 11/12/2018
                cmd.Parameters.Add("@BPCompName", SqlDbType.NVarChar).Value = bpInfo.CompName;
                cmd.Parameters.Add("@BPContactFirstName", SqlDbType.NVarChar).Value = bpInfo.ContactNameFirst;
                cmd.Parameters.Add("@BPContactLastName", SqlDbType.NVarChar).Value = bpInfo.ContactNameLast;
                cmd.Parameters.Add("@BPCompanyEIN", SqlDbType.NVarChar).Value = bpInfo.CompanyEIN;
                cmd.Parameters.Add("@BPCompanyDate", SqlDbType.DateTime).Value = bpInfo.CompanyDate;
                cmd.Parameters.Add("@BPLanguage", SqlDbType.NVarChar).Value = bpInfo.Language;
                cmd.Parameters.Add("@BPCountry", SqlDbType.NVarChar).Value = bpInfo.Country;
                cmd.Parameters.Add("@BPEmail", SqlDbType.NVarChar).Value = bpInfo.Email;
                cmd.Parameters.Add("@BPWebsite", SqlDbType.NVarChar).Value = bpInfo.Website;
                cmd.Parameters.Add("@BPDepartment", SqlDbType.NVarChar).Value = bpInfo.Department;
                cmd.Parameters.Add("@BPTollFreePhone", SqlDbType.NVarChar).Value = bpInfo.TollFreePhone;
                cmd.Parameters.Add("@BPNoofUsers", SqlDbType.Int).Value = bpInfo.NoofUsers;
                cmd.Parameters.Add("@BPUsageFee", SqlDbType.Bit).Value = bpInfo.UsageFee;
                cmd.Parameters.Add("@FHNumber", SqlDbType.NVarChar).Value = FHNumber;///FHNumner;
                cmd.Parameters.Add("@FHIDValue", SqlDbType.NVarChar).Value = FHid;// FHIDValue;
                cmd.Parameters.Add("@CheckDigit", SqlDbType.NVarChar).Value = CheckDigit;
                cmd.Parameters.Add("@G", SqlDbType.NVarChar).Value = user.DefaultG;
                cmd.Parameters.Add("@PL", SqlDbType.NVarChar).Value = user.DefaultPL;
                result = cmd.ExecuteNonQuery();
                CreateTableForRepositoryATTimeCreateUser(user, addInfo, personalInfo, bpInfo, user.BPID, user.UserID);
                if (result >= 3)
                    returnValue = true;
                else
                    returnValue = false;
            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file 

                DataLogger.Write("UserData-CreateUser", sqlEx.Message);

            }
            catch (Exception ex)
            {
                // Base exception
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file
                DataLogger.Write("UserData-CreateUser", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return returnValue;
        }

        public bool CreateTeamUser(User user, AddressIinformation addInfo, PersonalInformation personalInfo, BPInfo bpInfo)
        {
            int result = -1;
            bool returnValue = false;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = "sp_CreateTeamUser";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = user.UserID;
                cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = user.EmailID;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.PasswordHash;
                cmd.Parameters.Add("@PasswordHasVAlue", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);//user.PasswordHasValue;
                cmd.Parameters.Add("@InitialPassword", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(user.Password);// user.InitialPassword;
                cmd.Parameters.Add("@CodeVersion", SqlDbType.NVarChar).Value = "1.0";
                cmd.Parameters.Add("@RegistrationType", SqlDbType.NVarChar).Value = user.RegistrationType;
                cmd.Parameters.Add("@Role", SqlDbType.Int).Value = user.Role;
                cmd.Parameters.Add("@TeamMemberRole", SqlDbType.NVarChar).Value = personalInfo.TeamMemberRole;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = user.BPID;

                //****************************** personal **************** 

                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = personalInfo.Title;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = personalInfo.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = personalInfo.LastName;
                cmd.Parameters.Add("@Name2", SqlDbType.NVarChar).Value = personalInfo.Name2;
                cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = personalInfo.MiddleName;
                cmd.Parameters.Add("@LastName2", SqlDbType.NVarChar).Value = personalInfo.LastName2;
                cmd.Parameters.Add("@AKA", SqlDbType.NVarChar).Value = personalInfo.AKA;
                cmd.Parameters.Add("@Initials", SqlDbType.NVarChar).Value = personalInfo.Initials;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = personalInfo.DefaulCountry; //Removed and rename as default 
                cmd.Parameters.Add("@Profession", SqlDbType.NVarChar).Value = personalInfo.Profession;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = personalInfo.Gender;
                cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = personalInfo.Language;
                cmd.Parameters.Add("@DateOfBorn", SqlDbType.DateTime).Value = personalInfo.DateOfBorn;
                cmd.Parameters.Add("@BirthCountry", SqlDbType.NVarChar).Value = personalInfo.BirthCountry;
                cmd.Parameters.Add("@PesonalMobile", SqlDbType.NVarChar).Value = personalInfo.Mobile;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = personalInfo.Phone;
                cmd.Parameters.Add("@PesonalEmail", SqlDbType.NVarChar).Value = personalInfo.Email;
                cmd.Parameters.Add("@MiddelInitials", SqlDbType.NVarChar).Value = personalInfo.MiddelInitials;

                //****************************** Address ****************

                cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = addInfo.Address1;
                cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = addInfo.Address2;
                cmd.Parameters.Add("@POBox", SqlDbType.NVarChar).Value = addInfo.POBox;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = addInfo.City;
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = addInfo.State;
                cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = addInfo.Zip;
                cmd.Parameters.Add("@AddCountry", SqlDbType.NVarChar).Value = addInfo.Country;
                cmd.Parameters.Add("@CountryKey", SqlDbType.NVarChar).Value = addInfo.CountryKey;
                cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar).Value = addInfo.TimeZone;
                cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = addInfo.Location;
                cmd.Parameters.Add("@DistrictPostalCode", SqlDbType.NVarChar).Value = addInfo.DistrictPostalCode;
                cmd.Parameters.Add("@county", SqlDbType.NVarChar).Value = addInfo.county;

                result = cmd.ExecuteNonQuery();
                if (result >= 3)
                    returnValue = true;
                else
                    returnValue = false;
            }
            catch (SqlException sqlEx)
            {
                // Get orginal value from raise error by sql
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file 
                DataLogger.Write("UserData-CreateTeamUser", sqlEx.Message);
            }
            catch (Exception ex)
            {
                // Base exception
                returnValue = false;
                ConnectionClass.closeconnection(con);
                // Write exception in log file 
                DataLogger.Write("UserData-CreateTeamUser", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return returnValue;
        }

        public DataSet GetAllTeamMember(string BPID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetAllTeamUsers";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetAllTeamMember", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
        public int UpdateUserStatus(string userId,string BPID,Boolean status,DateTime ExDate,string Op)
        {
            int result = -1;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_UpdateUserStatusAndExpireDate";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userId;
                cmd.Parameters.Add("@ExpDate", SqlDbType.DateTime).Value = ExDate;
                cmd.Parameters.Add("@Status", SqlDbType.Bit).Value = status;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = Op;

                result = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                result = -1;
                DataLogger.Write("UserData-UpdateUserStatus", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public int FindAge(DateTime BDate)
        {
            int age = 0;
            TimeSpan ts = DateTime.Now - BDate;
            age = ts.Days / 365;
            return age;
        }
        public DataSet GetAutoFillValueAfterRegistration(string BPID, string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetAutoFillValue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetAutoFillValueAfterRegistration", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public string DayMonthYear(DateTime date,string returnType)
        {
           
            string retValue = "";
            try
            {
                if (returnType.ToLower() == "d")
                {
                    //Return day
                    retValue =String.Format("{0}, {1}",date.Day, date.ToString("dddd"));
                }
                else if (returnType.ToLower() == "m")
                {
                    //Return Month
                    retValue = date.ToString("MMMM");
                }
                else if (returnType.ToLower() == "y")
                {
                    // Return Year
                    retValue = date.ToString("yyyy");
                }
            }
            catch(Exception ex)
            {
                //TODO:
            }
            return retValue;
        }

        public List<Repository> AutoFillValueForShareData(List<Repository> tagList)
        {
            List<Repository> lstTag = new List<Repository>();
            lstTag = tagList;
            string BPID = string.Empty;
            string UserID = string.Empty;
            string Fname = string.Empty;
            string Lname = string.Empty;
            try
            {
                if (lstTag.Where(x => x.Tag.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault() != null)
                {
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Share =
                            lstTag.Where(x => x.Tag.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Share !=""? lstTag.Where(x => x.Tag.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Share.Substring(0, 1):"";
                    }
                }
                else
                {
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Share = "";
                    }
                }



                if (lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share != "")
                {
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Age>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Share = FindAge(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share)).ToString();
                    }
                    else
                    {
                        lstTag.Add(new Repository
                        {
                            ID = 1,
                            UTAGID = 1000074.ToString(),
                            Tag = "F<Age>H",
                            GlobPri = "G",
                            Description = "Age (Auto Calc today() - DOB)",
                            Share = FindAge(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share)).ToString(),
                            Org = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                    // BDay
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "d").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Repository
                        {
                            ID = 1,
                            UTAGID = 1000042.ToString(),
                            Tag = "F<Bday>H",
                            GlobPri = "G",
                            Description = "Birth Day (Auto calc: Birth Day)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "d").ToString(),
                            Org = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                    // BMonth
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "m").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Repository
                        {
                            ID = 1,
                            UTAGID = 1000041.ToString(),
                            Tag = "F<Bmonth>H",
                            GlobPri = "G",
                            Description = "Birth Month (Auto calc: Birth Month)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "m").ToString(),
                            Org = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                    //BYear
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "y").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Repository
                        {
                            ID = 1,
                            UTAGID = 1000043.ToString(),
                            Tag = "F<Byear>H",
                            GlobPri = "G",
                            Description = "Birth Year (Auto calc: Birth Year)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "y").ToString(),
                            Org = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                }
                else if (lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share == "")
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share = "MM/DD/YYYY";
                    lstTag.Where(x => x.Tag.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Share = "";

                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.Tag.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Share = "";
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.Tag.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Share = "";
                    if (lstTag.Where(x => x.Tag.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.Tag.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Share = "";
                }


                // system day
                if (lstTag.Where(x => x.Tag.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("dddd");
                }
                // system Time
                if (lstTag.Where(x => x.Tag.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortTimeString();
                }

                // System month
                if (lstTag.Where(x => x.Tag.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("MMMM");
                }
                // system year
                if (lstTag.Where(x => x.Tag.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("yyyy");
                }

                // system date
                if (lstTag.Where(x => x.Tag.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortDateString();
                }

                Fname = lstTag.Where(x => x.Tag.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault() != null ? lstTag.Where(x => x.Tag.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().Share : "";
                Lname = lstTag.Where(x => x.Tag.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault() != null ? lstTag.Where(x => x.Tag.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().Share : "";
                // Full name 27/3/2019
                if (Fname != "" || Lname != "")
                {
                    lstTag.Add(new Repository
                    {
                        ID = 1,
                        UTAGID = 1000215.ToString(),
                        Tag = "F<FLName>H",
                        GlobPri = "G",
                        Description = "Full Name (Auto calc: first name and Last name)",
                        Share = Fname + " " + Lname,
                        Org = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }
                // Business middle name
                if (lstTag.Where(x => x.Tag.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.Tag.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share != "")
                {

                    if (lstTag.Where(x => x.Tag.ToLower() == "F<BusMInitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.Tag.ToLower() == "F<BusMInitial>H".ToLower()).FirstOrDefault().Share = lstTag.Where(x => x.Tag.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share.Substring(0, 1);
                    }
                    else
                    {
                        lstTag.Add(new Repository
                        {
                            ID = 1,
                            UTAGID = 1000165.ToString(),
                            Tag = "F<BusMInitial>H",
                            GlobPri = "G",
                            Description = "Business Contact Middle Initial",
                            Share = lstTag.Where(x => x.Tag.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share.Substring(0, 1),
                            Org = "S",
                            BPID = BPID,
                            UserID = UserID
                        });

                    }
                }
                if (lstTag.Where(x => x.Tag.ToLower() == "F<Wrrnty_CDate>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.Tag.ToLower() == "F<Wrrnty_CDate>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortDateString();
                }
            }
            catch(Exception ex)
            {
                DataLogger.Write("UserData-AutoFillValueForShareData", ex.Message);
            }
            return lstTag;
        }

        public List<Tag> AutoCalculateValue(List<Tag> tagList)
        {
            DataSet ds = new DataSet();
            List<Tag> lstTag = new List<Tag>();
            lstTag = tagList;
            string Fname = string.Empty;
            string Lname = string.Empty;
            string BPID = string.Empty; 
            string UserID = string.Empty;
           

            // Age
            try
            {
                BPID=lstTag.FirstOrDefault().BPID;
                UserID=lstTag.FirstOrDefault().UserID;
                ds = GetAutoFillValueAfterRegistration(BPID, UserID);

                if (lstTag.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault() != null)
                {
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Share =
                            lstTag.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Share !=""? lstTag.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Share.Substring(0,1):"";
                    }
                }
                else
                {
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Share = "";
                    }
                }



                if (lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share != "")
                {
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault() != null )
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Share = FindAge(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share)).ToString();
                    }
                    else
                    {
                            lstTag.Add(new Tag
                            {
                                ID = 1,
                                UTAGID = 1000074,
                                TagName = "F<Age>H",
                                GlobPri = "G",
                                Description = "Age (Auto Calc today() - DOB)",
                                Share = FindAge(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share)).ToString(),
                                Orig = "S",
                                BPID = BPID,
                                UserID = UserID
                            });
                    }
                   
                    // BDay
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share),"d").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Tag
                        {
                            ID = 1,
                            UTAGID = 1000042,
                            TagName = "F<Bday>H",
                            GlobPri = "G",
                            Description = "Birth Day (Auto calc: Birth Day)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share),"d").ToString(),
                            Orig = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                    // BMonth
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "m").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Tag
                        {
                            ID = 1,
                            UTAGID = 1000041,
                            TagName = "F<Bmonth>H",
                            GlobPri = "G",
                            Description = "Birth Month (Auto calc: Birth Month)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "m").ToString(),
                            Orig = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                    //BYear
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "y").ToString();
                    }
                    else
                    {
                        lstTag.Add(new Tag
                        {
                            ID = 1,
                            UTAGID = 1000043,
                            TagName = "F<Byear>H",
                            GlobPri = "G",
                            Description = "Birth Year (Auto calc: Birth Year)",
                            Share = DayMonthYear(Convert.ToDateTime(lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share), "y").ToString(),
                            Orig = "S",
                            BPID = BPID,
                            UserID = UserID
                        });
                    }
                }
                else if (lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share == "")
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Share = "MM/DD/YYYY";
                    lstTag.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Share = "";

                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Share = "";
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Share = "";
                    if (lstTag.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault() != null)
                        lstTag.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Share = "";
                }

                // system day
                if (lstTag.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("dddd");
                }
                else
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000173,
                        TagName = "F<sysday>H",
                        GlobPri = "G",
                        Description = "System Day (Auto calc: Todays Day)",
                        Share = DateTime.Now.ToString("dddd"),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }

                // system Time
                if (lstTag.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortTimeString();
                }
                else
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000153,
                        TagName = "F<SysTime>H",
                        GlobPri = "G",
                        Description = "System Time (Auto calc: now Time)",
                        Share = DateTime.Now.ToShortTimeString(),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }

                // System month
                if (lstTag.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("MMMM");
                }
                else
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000174,
                        TagName = "F<sysmonth>H",
                        GlobPri = "G",
                        Description = "System Month (Auto calc: Todays Month)",
                        Share = DateTime.Now.ToString("MMMM"),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }

                // system year
                if (lstTag.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToString("yyyy");
                }
                else
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000175,
                        TagName = "F<sysyear>H",
                        GlobPri = "G",
                        Description = "System Year (Auto calc: Todays Year)",
                        Share = DateTime.Now.ToString("yyyy"),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }

                // system date
                if (lstTag.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortDateString();
                }
                else
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000110,
                        TagName = "F<sysdate>H",
                        GlobPri = "G",
                        Description = "System Date (Auto calc: Todays Date)",
                        Share = DateTime.Now.ToShortDateString(),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }
                // Contract date
                if (lstTag.Where(x => x.TagName.ToLower() == "F<Wrrnty_CDate>H".ToLower()).FirstOrDefault() != null)
                {
                    lstTag.Where(x => x.TagName.ToLower() == "F<Wrrnty_CDate>H".ToLower()).FirstOrDefault().Share = DateTime.Now.ToShortDateString();
                }
                else
                {

                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000250,
                        TagName = "F<Wrrnty_CDate>H",
                        GlobPri = "G",
                        Description = "Contract Date",
                        Share = DateTime.Now.ToShortDateString(),
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }


                //// Industry Name
                //if (lstTag.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault() != null)
                //{
                //    lstTag.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault().Share = "";
                //}
                //// Sector name
                //if (lstTag.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault() != null)
                //{
                //    lstTag.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault().Share ="";
                //}

                Fname = lstTag.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault() != null ? lstTag.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().Share : "";
                Lname = lstTag.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault() != null ? lstTag.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().Share : "";
                // Full name 27/3/2019
                if (Fname != "" || Lname != "")
                {
                    lstTag.Add(new Tag
                    {
                        ID = 1,
                        UTAGID = 1000215,
                        TagName = "F<FLName>H",
                        GlobPri = "G",
                        Description = "Full Name (Auto calc: first name and Last name)",
                        Share = Fname + " " + Lname,
                        Orig = "S",
                        BPID = BPID,
                        UserID = UserID
                    });
                }
                // Business middle name
                if (lstTag.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault() != null && lstTag.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share!="")
                {

                    if (lstTag.Where(x => x.TagName.ToLower() == "F<BusMInitial>H".ToLower()).FirstOrDefault() != null)
                    {
                        lstTag.Where(x => x.TagName.ToLower() == "F<BusMInitial>H".ToLower()).FirstOrDefault().Share = lstTag.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share.Substring(0, 1);
                    }
                    else
                    {
                        lstTag.Add(new Tag
                        {
                            ID = 1,
                            UTAGID = 1000165,
                            TagName = "F<BusMInitial>H",
                            GlobPri = "G",
                            Description = "Business Contact Middle Initial",
                            Share = lstTag.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Share.Substring(0, 1),
                            Orig = "S",
                            BPID = BPID,
                            UserID = UserID
                        });

                    }
                }
                //BPID
                lstTag.Add(new Tag
                {
                    ID = 1,
                    UTAGID = 1000206,
                    TagName = "F<FHBPNum>H",
                    GlobPri = "P",
                    Description = "FH BP Number (Auto Calc: after registration)",
                    Share = BPID,
                    Orig = "S",
                    BPID=BPID,
                    UserID=UserID
                });
                //UserID
                lstTag.Add(new Tag
                {
                    ID = 1,
                    UTAGID = 1000208,
                    TagName = "F<FHUserID>H",
                    GlobPri = "P",
                    Description = "FH User ID  (Auto Calc: after registration)",
                    Share = UserID,
                    Orig = "S",
                    BPID = BPID,
                    UserID = UserID
                });
                //FHNumber
                lstTag.Add(new Tag
                {
                    ID = 1,
                    UTAGID = 1000207,
                    TagName = "F<FHNum>H",
                    GlobPri = "P",
                    Description = "FH Number (Auto Calc: after registration)",
                    Share = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["FHNumber"].ToString() : "",
                    Orig = "S",
                    BPID = BPID,
                    UserID = UserID
                });

                //Registration Type
                lstTag.Add(new Tag
                {
                    ID = 1,
                    UTAGID = 1000214,
                    TagName = "F<FHRegType>H",
                    GlobPri = "P",
                    Description = "FH Registration Type (Auto Calc: after Registration)",
                    Share = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["Partner"].ToString():"",
                    Orig = "S",
                    BPID = BPID,
                    UserID = UserID
                });

                //FHNumber date
                lstTag.Add(new Tag
                {
                    ID = 1,
                    UTAGID = 1000289,
                    TagName = "F<FHNumDate>H",
                    GlobPri = "P",
                    Description = "FH Number Date when assigned (Auto Calc: during registration)",
                    Share = ds.Tables[0].Rows.Count > 0 ? Convert.ToDateTime(ds.Tables[0].Rows[0]["FHDate"]).ToShortDateString() : "",
                    Orig = "S",
                    BPID = BPID,
                    UserID = UserID
                });


            }
            catch (Exception ex)
            {
                DataLogger.Write("UserData-AutoCalculateValue", ex.Message);
            }
            return lstTag;
        }
        public void CreateTableForRepositoryATTimeCreateUser(User user, AddressIinformation addInfo, PersonalInformation personalInfo,BPInfo bPInfo,string BPID,string UserID)
        {
            string BDate = personalInfo.DateOfBorn.HasValue ? personalInfo.DateOfBorn.Value.ToShortDateString() : "";
            string BMonth = personalInfo.DateOfBorn.HasValue ? personalInfo.DateOfBorn.Value.ToString("MMMM") : "";
            string BDay = personalInfo.DateOfBorn.HasValue ? personalInfo.DateOfBorn.Value.ToString("dddd") : "";
            string BYear = personalInfo.DateOfBorn.HasValue ? personalInfo.DateOfBorn.Value.ToString("yyyy") : "";
            string BAge = personalInfo.DateOfBorn.HasValue ? FindAge(personalInfo.DateOfBorn.Value).ToString() : "";
            List<Repository> lst = new List<Repository>();
            List<Tag> lstTagDetails = new List<Tag>();
            try
            {
                // Get All tag details from globaltag table
                DataSet ds = GetAllTagFromGlobalTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lstTagDetails.Add(new Tag
                        {
                            ID = Convert.ToInt32(row["ID"]),
                            UserID = row["UserID"].ToString(),
                            UTAGID = Convert.ToInt32(row["UTAGID"]),
                            TagName = row["Tag"].ToString(),
                            Orig = row["Orig"].ToString(),
                            GlobPri = row["GlobPri"].ToString(),
                            Description = row["Description"].ToString()
                        });
                    }
                }
                else
                {
                    return;
                }

                // Add dyanmic value accordign to global tag value

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<ISector>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault().Description,
                        Share = GetSECIDByShareID(user.IndustryShareID),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<ISector>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Idustry>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault().Description,
                        Share = user.IndustryShareID,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Idustry>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<PTitle>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<PTitle>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<PTitle>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Title,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<PTitle>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<PTitle>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<PTitle>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Fname>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.FirstName,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fname>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Mname>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.MiddleName,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Mname>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Lname>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.LastName,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Lname>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Minitial>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.MiddleName != null ? personalInfo.MiddleName.Substring(0, 1) : "",
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Minitial>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Bdate>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.DateOfBorn.HasValue ? personalInfo.DateOfBorn.Value.ToShortDateString() : "",
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bdate>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<AKA>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<AKA>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<AKA>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.AKA,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<AKA>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<AKA>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<AKA>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fax>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Fax>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fax>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Fax,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fax>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fax>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Fax>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Wphone>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Wphone>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Wphone>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.WorkPhone,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Wphone>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Wphone>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Wphone>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Phone>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Phone>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Phone>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Phone,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Phone>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Phone>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Phone>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<WebSite>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<WebSite>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<WebSite>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Website,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<WebSite>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<WebSite>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<WebSite>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Email>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Email>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Email>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Email,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Email>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Email>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Email>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Occupation>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Occupation>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Occupation>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Profession,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Occupation>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Occupation>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Occupation>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Dcountry>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Dcountry>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Dcountry>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.DefaulCountry,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Dcountry>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Dcountry>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Dcountry>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bcountry>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Bcountry>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bcountry>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.BirthCountry,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bcountry>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bcountry>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bcountry>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<DLanguage>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<DLanguage>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<DLanguage>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Language,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<DLanguage>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<DLanguage>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<DLanguage>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Gender>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Gender>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Gender>H".ToLower()).FirstOrDefault().Description,
                        Share = personalInfo.Gender,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Gender>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Gender>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Gender>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CWeb>H

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CWeb>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CWeb>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CWeb>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.Website,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CWeb>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CWeb>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CWeb>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                //F<Address1>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Address1>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Address1>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Address1>H".ToLower()).FirstOrDefault().Description,
                        Share = addInfo.Address1,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Address1>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Address1>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Address1>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<City>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<City>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<City>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<City>H".ToLower()).FirstOrDefault().Description,
                        Share = addInfo.City,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<City>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<City>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<City>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<State>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<State>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<State>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<State>H".ToLower()).FirstOrDefault().Description,
                        Share = addInfo.State,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<State>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<State>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<State>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<Zip>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Zip>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Zip>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Zip>H".ToLower()).FirstOrDefault().Description,
                        Share = addInfo.Zip,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Zip>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Zip>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Zip>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                //	F<CEmail>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEmail>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CEmail>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEmail>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.Email,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEmail>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEmail>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEmail>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CName>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CName>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CName>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CName>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.CompName,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CName>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CName>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CName>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // F<CCFName>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCFName>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CCFName>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCFName>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.ContactNameFirst,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCFName>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCFName>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCFName>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // F<CCLName>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCLName>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CCLName>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCLName>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.ContactNameLast,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCLName>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCLName>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CCLName>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // F<CEIN>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEIN>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CEIN>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEIN>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.CompanyEIN,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEIN>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEIN>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CEIN>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CIDate>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CIDate>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CIDate>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CIDate>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.CompanyDate.HasValue ? bPInfo.CompanyDate.Value.ToShortDateString() : "",
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CIDate>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CIDate>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CIDate>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CDept>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CDept>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CDept>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CDept>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.Department,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CDept>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CDept>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CDept>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CICountry>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CICountry>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CICountry>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CICountry>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.Country,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CICountry>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CICountry>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CICountry>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // 	F<CTFree>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CTFree>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CTFree>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CTFree>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.TollFreePhone,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CTFree>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CTFree>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CTFree>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CMPhone>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CMPhone>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CMPhone>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CMPhone>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.MainPhone,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CMPhone>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CMPhone>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CMPhone>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                //F<CSPhone>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CSPhone>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CSPhone>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CSPhone>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.SecondPhone,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CSPhone>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CSPhone>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CSPhone>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // F<CFax>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<CFax>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<CFax>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CFax>H".ToLower()).FirstOrDefault().Description,
                        Share = bPInfo.Fax,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CFax>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CFax>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<CFax>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // Bdate month
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Bmonth>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Description,
                        Share = BMonth,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bmonth>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // Bdate day
                
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Bday>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Description,
                        Share = BDay,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Bday>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // Bdate Year
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Byear>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Description,
                        Share = BYear,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Byear>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // Age
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<Age>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Description,
                        Share = BAge,
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<Age>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }

                // System day
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<sysday>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToString("dddd"),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysday>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // System Time
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<SysTime>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToShortTimeString(),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<SysTime>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // System month
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<sysmonth>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToString("MMMM"),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysmonth>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // System Year
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<sysyear>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToString("yyyy"),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysyear>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // System Date

                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<sysdate>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToShortDateString(),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<sysdate>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }
                // F<BusMName>H
                if (lstTagDetails.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault() != null)
                {
                    lst.Add(new Repository
                    {
                        ID = 0,
                        BPID = BPID,
                        UserID = UserID,
                        Tag = "F<BusMName>H",
                        Description = lstTagDetails.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Description,
                        Share = DateTime.Now.ToShortDateString(),
                        Org = lstTagDetails.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().Orig,
                        GlobPri = lstTagDetails.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().GlobPri,
                        UTAGID = lstTagDetails.Where(x => x.TagName.ToLower() == "F<BusMName>H".ToLower()).FirstOrDefault().UTAGID.ToString()

                    });
                }


                // Cnovert into table and bulk upload data
                CommanUserData userData = GetCommanData(UserID);
                DataTable dt = ConvertToDataTableForRepostory(lst, Convert.ToString(Convert.ToInt32(userData.BPID)), UserID);
                _fileData.CreateRepository(dt);
                _fileData.CreateUpdateShareValue(dt, Convert.ToString(Convert.ToInt32(userData.BPID)), "Insert");
            }
            catch(Exception ex)
            {
                DataLogger.Write("UserData-CreateTableForRepositoryATTimeCreateUser", ex.Message);
            }
            finally
            {

            }

        }

        public static DataTable ConvertToDataTableForRepostory(List<Repository> data, string BPID, string UserId)
        {

            DataTable table = new DataTable();
            table.Columns.Add("BPID", typeof(string));
            table.Columns.Add("UserID", typeof(string));
            table.Columns.Add("UTAGID", typeof(string));
            table.Columns.Add("Tag", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Org", typeof(string));
            table.Columns.Add("GlobPri", typeof(string));
            table.Columns.Add("Share", typeof(string));
            table.Columns.Add("TemplateID", typeof(Int64));

            for (int i = 0; i < data.ToList().Count; i++)
                table.Rows.Add(new object[] {
                            data[i].BPID =BPID,
                            data[i].UserID =UserId,
                            data[i].UTAGID,
                            data[i].Tag,
                            data[i].Description,
                            data[i].Org,
                            data[i].GlobPri,
                            data[i].Share,
                            data[i].TemplateID

                           });

            return table;
        }


        public User GetUserDetailByUserIDAndPassword(string userID , string password)
        {
            User user = new User();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = "sp_VerifyUserLogin";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userID;
                cmd.Parameters.Add("@Password", SqlDbType.Binary, 16).Value = Utility.EncryptPassword(password);

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
                        user.SECID = row["SECID"].ToString();
                        user.BPID = row["BPID"].ToString();
                        user.Partner = row["Partner"].ToString();
                        user.BPType = row["BPType"].ToString();
                        user.ValidFrom = Convert.ToDateTime(row["ValidFrom"]);
                        user.ValidTo = Convert.ToDateTime(row["ValidTo"]);
                        user.BPValidTo = Convert.ToDateTime(row["BPValidTo"]);
                    }
                }
                else
                    user = null;

            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetUserDetailByUserIDAndPassword", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return user;
        }
        
        public int ResetPassword(User user)
        {
            int result = -1;
            try
            {
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
                result = cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                // write in log file, sql exception 
                ConnectionClass.closeconnection(con);
                result = -1;
                DataLogger.Write("UserData-ResetPassword", sqlEx.Message);
            }
            catch (Exception ex)
            {
                // write in log file,
                ConnectionClass.closeconnection(con);
                result = -1;
                DataLogger.Write("UserData-ResetPassword", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return result;
        }

        public void LoginHistory(LoginHistory logHist)
        {
            try
            {
                //sp_LoginHistory
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_LoginHistory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = logHist.UserID;
                cmd.Parameters.Add("@TerminalID", SqlDbType.NVarChar).Value = logHist.TerminalID;
                cmd.Parameters.Add("@Server", SqlDbType.NVarChar).Value = logHist.Server;
                cmd.Parameters.Add("@TerminalIP", SqlDbType.NVarChar).Value = logHist.TerminalIP;
                cmd.Parameters.Add("@LogonLang", SqlDbType.NVarChar).Value = "EN";
                // update data
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sql)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-LoginHistory", sql.Message);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-LoginHistory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
        }

        public DataSet GetIndustry()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "select ID,SECID,INDID,Share,Industry from tbl_Industry";
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetIndustry", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public string GetSECIDByShareID(string shareId)
        {
            string revValue = "";
            try
            {
                string strSql = "select SECID from tbl_Industry where Share='"+ shareId + "'";
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand(strSql, con);
                revValue = (string)cmd.ExecuteScalar();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetSECIDByShareID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return revValue;
        }

        public DataSet GetSector()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "SELECT ID,SECID,SECCode,Description FROM tbl_Sector";
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetSector", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return ds;
        }

        public DataSet GetAllTagFromGlobalTable()
        {
            DataSet ds = new DataSet();
            try
            {
                string strSql = "SELECT ID, UTAGID,UserID ,Tag ,Orig ,GlobPri ,Description FROM tbl_StandardGlobalTag";
                con = ConnectionClass.getConnection();
                SqlDataAdapter da = new SqlDataAdapter(strSql, con);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }

            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetAllTagFromGlobalTable", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }


        public CommanUserData GetCommanData(string UserId)
        {
            CommanUserData objComman = new CommanUserData();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_CommanUserData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objComman.UserID = Convert.ToString(ds.Tables[0].Rows[i]["UserID"]);
                        objComman.BPID = Convert.ToString(ds.Tables[0].Rows[i]["BPID"]);
                        objComman.BPType = Convert.ToString(ds.Tables[0].Rows[i]["BPType"]);
                        objComman.Sector = Convert.ToString(ds.Tables[0].Rows[i]["Sector"]);
                        objComman.HarmonizerValue = Convert.ToString(ds.Tables[0].Rows[i]["HarmonizerValue"]);
                        objComman.ActiveDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ActiveDate"]);
                        objComman.ExpireDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["ExpireDate"]);
                    }
                }

                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetCommanData", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return objComman;

        }
        public DataSet GetBusinessTypeList()
        {
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetBusinessType";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetBusinessTypeList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;

        }
        public DataSet GetLanguageList()
        {
            DataSet ds = new DataSet();
            try
            {
                con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "select Language,LanguageDescription from tbl_LanguageTimeZone where Language IS NOT NULL";
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetLanguageList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;

        }

        public DataSet GetProfileDetails(string BPID,string UserID,int RollID,string Op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetProfileDetails";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserID;
                cmd.Parameters.Add("@RollID", SqlDbType.Int).Value = RollID;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = Op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetProfileDetails", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return ds;
        }
        public DataSet GetUserByRoleID(string RoleID, string Op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetAllUsersByRole";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = Convert.ToInt32(RoleID);
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = Op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetUserByRoleID", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return ds;
        }

        public int UpdateUserAddress(AddressIinformation Address,int RoleID)
        {
            int rValue = 0;
            SqlCommand cmd = new SqlCommand();
            try
            {
                con = ConnectionClass.getConnection();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteAddress";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@AddressId", SqlDbType.BigInt).Value = Address.AddressID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = Address.UserID;
                cmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = Address.Address1;
                cmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = Address.Address2;
                cmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = Address.City;
                cmd.Parameters.Add("@Zip", SqlDbType.NVarChar).Value = Address.Zip;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = Address.Country;
                cmd.Parameters.Add("@POBox", SqlDbType.NVarChar).Value = Address.POBox;
                cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar).Value = Address.TimeZone;
                cmd.Parameters.Add("@Location", SqlDbType.NVarChar).Value = Address.Location;
                cmd.Parameters.Add("@DistrictPostalCode", SqlDbType.NVarChar).Value = Address.DistrictPostalCode;
                cmd.Parameters.Add("@county", SqlDbType.NVarChar).Value = Address.county;
                cmd.Parameters.Add("@CountryKey", SqlDbType.NVarChar).Value = Address.CountryKey;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Update";
                cmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = RoleID;
                cmd.Parameters.Add("@State", SqlDbType.NVarChar).Value = Address.State;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                rValue = 0;
                DataLogger.Write("UserData-UpdateUserAddress", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }

            return rValue;
        }
        public int UpdateUserPersonalInfo(PersonalInformation personalInformation)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeletePersonalInfo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PersonalID", SqlDbType.BigInt).Value = personalInformation.PersonalID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = personalInformation.UserID;
                cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = personalInformation.Title.Trim();
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = personalInformation.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = personalInformation.LastName;
                cmd.Parameters.Add("@MiddleName", SqlDbType.NVarChar).Value = personalInformation.MiddleName;
                cmd.Parameters.Add("@Name2", SqlDbType.NVarChar).Value = personalInformation.Name2;
                cmd.Parameters.Add("@LastName2", SqlDbType.NVarChar).Value = personalInformation.LastName2;
                cmd.Parameters.Add("@AKA", SqlDbType.NVarChar).Value = personalInformation.AKA;
                cmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = personalInformation.Gender.Trim();
                cmd.Parameters.Add("@DateOfBorn", SqlDbType.DateTime).Value = personalInformation.DateOfBorn;
                cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = personalInformation.Language;
                cmd.Parameters.Add("@BirthCountry", SqlDbType.NVarChar).Value = personalInformation.BirthCountry;
                cmd.Parameters.Add("@Profession", SqlDbType.NVarChar).Value = personalInformation.Profession;
                cmd.Parameters.Add("@DefaulCountry", SqlDbType.NVarChar).Value = personalInformation.DefaulCountry;
                cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar).Value = personalInformation.Mobile;
                cmd.Parameters.Add("@WorkPhone", SqlDbType.NVarChar).Value = personalInformation.WorkPhone;
                cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = personalInformation.Fax;
                cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = personalInformation.Phone;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = personalInformation.Email;
                cmd.Parameters.Add("@Website", SqlDbType.NVarChar).Value = personalInformation.Website;
                cmd.Parameters.Add("@Initials", SqlDbType.NVarChar).Value = personalInformation.FirstName != "" ? personalInformation.FirstName.Substring(0, 1).ToUpper() : "";
                cmd.Parameters.Add("@MiddelIntials", SqlDbType.NVarChar).Value = personalInformation.MiddleName != "" ? personalInformation.MiddleName.Substring(0, 1).ToUpper() : "";
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Update";
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                rValue = 0;
                DataLogger.Write("UserData-UpdateUserPersonalInfo", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }
        public int UpdateUserBPInfo(BPInfo bPInfo,string BPID)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteBPInfo";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = bPInfo.UserID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@CompName", SqlDbType.NVarChar).Value = bPInfo.CompName;
                cmd.Parameters.Add("@ContactNameFirst", SqlDbType.NVarChar).Value = bPInfo.ContactNameFirst;
                cmd.Parameters.Add("@ContactNameLast", SqlDbType.NVarChar).Value = bPInfo.ContactNameLast;
                cmd.Parameters.Add("@CompanyEIN", SqlDbType.NVarChar).Value = bPInfo.CompanyEIN;
                cmd.Parameters.Add("@CompanyDate", SqlDbType.DateTime).Value = bPInfo.CompanyDate;
                cmd.Parameters.Add("@Language", SqlDbType.NVarChar).Value = bPInfo.Language;
                cmd.Parameters.Add("@Country", SqlDbType.NVarChar).Value = bPInfo.Country;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = bPInfo.Email;
                cmd.Parameters.Add("@Website", SqlDbType.NVarChar).Value = bPInfo.Website;
                cmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = bPInfo.Department;
                cmd.Parameters.Add("@TollFreePhone", SqlDbType.NVarChar).Value = bPInfo.TollFreePhone;
                cmd.Parameters.Add("@MainPhone", SqlDbType.NVarChar).Value = bPInfo.MainPhone;
                cmd.Parameters.Add("@SecondPhone", SqlDbType.NVarChar).Value = bPInfo.SecondPhone;
                cmd.Parameters.Add("@Fax", SqlDbType.NVarChar).Value = bPInfo.Fax;
                cmd.Parameters.Add("@NoofUsers", SqlDbType.Int).Value = bPInfo.NoofUsers;
                cmd.Parameters.Add("@UsageFee", SqlDbType.Bit).Value = bPInfo.UsageFee;
                cmd.Parameters.Add("@Operation", SqlDbType.NVarChar).Value = "Update";
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                rValue = 0;
                DataLogger.Write("UserData-UpdateUserBPInfo", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }
        public DataSet GetFolderLocation(string BPID,string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetFolderLocationByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetFolderLocation", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int UpdateFolderLocation(FolderLocation folderLocation, string op)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetFolderLocationByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = folderLocation.BPID;
                cmd.Parameters.Add("@Source", SqlDbType.NVarChar).Value = folderLocation.Source;
                cmd.Parameters.Add("@Target", SqlDbType.NVarChar).Value = folderLocation.Target;
                cmd.Parameters.Add("@Conversion", SqlDbType.NVarChar).Value = folderLocation.Conversion;
                cmd.Parameters.Add("@Filter", SqlDbType.NVarChar).Value = folderLocation.Filter;
                cmd.Parameters.Add("@Archive", SqlDbType.NVarChar).Value = folderLocation.Archive;
                cmd.Parameters.Add("@Template", SqlDbType.NVarChar).Value = folderLocation.Template;
                cmd.Parameters.Add("@Report", SqlDbType.NVarChar).Value = folderLocation.Report;
                cmd.Parameters.Add("@Changelog", SqlDbType.NVarChar).Value = folderLocation.Changelog;
                cmd.Parameters.Add("@Errorlog", SqlDbType.NVarChar).Value = folderLocation.Errorlog;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = op;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                rValue = 0;
                DataLogger.Write("UserData-UpdateFolderLocation", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }


        public DataSet GetNaming(string BPID, string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetNamingByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetNaming", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int UpdateNaming(string BPID, string prostfix, string prefix, string op)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetNamingByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@Postfix", SqlDbType.NVarChar).Value = prostfix;
                cmd.Parameters.Add("@Prefix", SqlDbType.NVarChar).Value = prefix;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = op;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                rValue = 0;
                DataLogger.Write("UserData-UpdateNaming", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }

        public DataSet GetPrivateTag(string BPID,string UserID,string op,string SecID="")
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeletePrivateTagByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = UserID;
                cmd.Parameters.Add("@SecID", SqlDbType.NVarChar).Value = SecID;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetPrivateTag", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int UpdateShareVaue(int ID, string shareValue, string globalPriValue, int utagId, string BPID,string op)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeletePrivateTagByBPID";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = BPID;
                cmd.Parameters.Add("@ID", SqlDbType.BigInt).Value = ID;
                cmd.Parameters.Add("@Share", SqlDbType.NVarChar).Value = shareValue;
                cmd.Parameters.Add("@GlobPri", SqlDbType.NVarChar).Value = globalPriValue;
                cmd.Parameters.Add("@UTagID", SqlDbType.BigInt).Value = utagId;
                cmd.Parameters.Add("@Op", SqlDbType.NVarChar).Value = op;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch(Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-UpdateShareVaue", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }

        public int InsertUpdateCustomSessionData(User userdata,string op)
        {
           
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCustomSessionData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = userdata.UserID;
                cmd.Parameters.Add("@Role", SqlDbType.Int).Value = userdata.Role;
                cmd.Parameters.Add("@EmailID", SqlDbType.NVarChar).Value = userdata.EmailID;
                cmd.Parameters.Add("@SECID", SqlDbType.NVarChar).Value = userdata.SECID;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = userdata.BPID;
                cmd.Parameters.Add("@Partner", SqlDbType.NVarChar).Value = userdata.Partner;
                cmd.Parameters.Add("@BPType", SqlDbType.NVarChar).Value = userdata.BPType;
                cmd.Parameters.Add("@UserIPAddress", SqlDbType.NVarChar).Value = userdata.UserIPAddress;
                cmd.Parameters.Add("@UserBrowserName", SqlDbType.NVarChar).Value = userdata.UserBrowserName;
                cmd.Parameters.Add("@SessionToken", SqlDbType.NVarChar).Value = userdata.SessionToken;
           
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-InsertUpdateCustomSessionData", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }
        public DataSet GetCustomSessionData(string UserIPAddrss,string UserBrowserName,string sessionToken,string op)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeleteCustomSessionData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserIPAddress", SqlDbType.NVarChar).Value = UserIPAddrss;
                cmd.Parameters.Add("@UserBrowserName", SqlDbType.NVarChar).Value = UserBrowserName;
                cmd.Parameters.Add("@SessionToken", SqlDbType.NVarChar).Value = sessionToken;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetCustomSessionData", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public int PaymentHistory(PaymentModel paymentModel, string op)
        {
            int rValue = 0;
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertUpdateDeletPaymentHistory";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = paymentModel.UserId;
                cmd.Parameters.Add("@BPID", SqlDbType.NVarChar).Value = paymentModel.BPID;
                cmd.Parameters.Add("@PlanId", SqlDbType.NVarChar).Value = paymentModel.PlanId;
                cmd.Parameters.Add("@amount", SqlDbType.NVarChar).Value = paymentModel.amount;
                cmd.Parameters.Add("@cart", SqlDbType.NVarChar).Value = paymentModel.cart;
                cmd.Parameters.Add("@create_time", SqlDbType.NVarChar).Value = paymentModel.create_time;
                cmd.Parameters.Add("@currency", SqlDbType.NVarChar).Value = paymentModel.currency;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = paymentModel.description;
                cmd.Parameters.Add("@invoice", SqlDbType.NVarChar).Value = paymentModel.invoice;
                cmd.Parameters.Add("@paymentmethod", SqlDbType.NVarChar).Value = paymentModel.paymentmethod;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = paymentModel.state;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = paymentModel.id;
                cmd.Parameters.Add("@payerId", SqlDbType.NVarChar).Value = paymentModel.payerId;
                cmd.Parameters.Add("@failuarreasion", SqlDbType.NVarChar).Value = paymentModel.failuarreasion;
                cmd.Parameters.Add("@TransactionId", SqlDbType.NVarChar).Value = paymentModel.TransactionId;
                cmd.Parameters.Add("@op", SqlDbType.NVarChar).Value = op;
                // update data
                rValue = cmd.ExecuteNonQuery();
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-PaymentHistory", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return rValue;
        }

        public List<Tag> GetCustomeTag(string BPID, string UserID)
        {
            //List<Tag> lst = new List<Tag>();
            //Tag tag = new Tag();
            ////string query = "select * from tbl_StandardGlobalTag where UserID='" + UserID+"'";
            //DataSet ds = new DataSet();
            //con = ConnectionClass.getConnection();
            //SqlCommand cmd = new SqlCommand();
            //cmd.Connection = con;
            //cmd.CommandText = "sp_InsertUpdateDeleteStandardGlobalTag";
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    foreach(DataRow row in ds.Tables[0].Rows)
            //    {
            //        tag = new Tag();
            //        tag.ID = Convert.ToInt32(row["ID"]);
            //        tag.UTAGID = Convert.ToInt32(row["UTAGID"]);
            //        tag.TagName = row["Tag"].ToString();
            //        tag.GlobPri = row["GlobPri"].ToString();
            //        tag.Description = row["Description"].ToString();
            //        lst.Add(tag);
            //    }
            //}
            //return lst; 
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
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("Admin-GetTagList", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return lstStandardtag.Where(x=>x.BPID== BPID).ToList();

        }

        // Logic for FH Number security
        #region FHNumber    security
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public char NumberToChar(int number)
        {
            //long num = 93462394623946;
            // char s=char(num);
            return (char)number;
        }

        public string FindFHValue()
        {

            int RNo1 = RandomNumber(0, 9); int RNo2 = RandomNumber(65, 90);
            char Rno3 = NumberToChar(RNo2); int RNo4 = RandomNumber(65, 90);
            char Rno5 = NumberToChar(RNo4);

            int RNo6 = RandomNumber(0, 9); int RNo7 = RandomNumber(65, 90);
            char Rno8 = NumberToChar(RNo7); int RNo9 = RandomNumber(65, 90);
            char Rno10 = NumberToChar(RNo9);

            int RNo11 = RandomNumber(0, 9);
            int RNo12 = RandomNumber(0, 9);
            int RNo13 = RandomNumber(0, 9);
            int RNo14 = RandomNumber(65, 90);
            char Rno15 = NumberToChar(RNo14);
            string FhValue = Convert.ToString(RNo1) + Rno3 + Rno5 + RNo6 + Rno8 + Rno10 + RNo11 + RNo12 + RNo13 + RNo14 + Convert.ToString(Rno15);
            return FhValue;

        }

        public char FindCheckDigit()//=CHAR(TRUNC(RAND()*$ID$9+$IC$6))
        {
            //(Char)Convert.ToInt32(Math.Truncate((s.NextDouble() * 33) + 90))
            Random s = new Random();
            Char charvalue=Convert.ToChar(Convert.ToInt32(Math.Truncate((s.NextDouble() * 33) + 90)));
            return charvalue;

        }
        public string FindFHid(string G, string PL, string Country, string city, string State, string year, string Gender, string FHValue, char CheckDigit)//=CONCATENATE(HN6,HO6,HP6,HQ6,HR6,HS6,HT6,HU6,HV6)
        {
            if (Gender != "")
            {
                Gender = Gender.Substring(0, 1);
            }
            else
            {
                Gender = "";
            }
            string FhId = Convert.ToString(G) + Convert.ToString(PL) + Country + city.Replace(" ",string.Empty) + State.Replace(" ", string.Empty) + year + Gender + FHValue + Convert.ToString(CheckDigit);
            return FhId;
        }
        #endregion


        public DateTime GetExpiredate(string UserID)
        {
            DateTime retValue=Convert.ToDateTime("1/1/1800");
            SqlCommand com = new SqlCommand();
            try
            {
                string strSql = "select ExpireDate from tbl_User where UserID='" + UserID + "' ";
                con = ConnectionClass.getConnection();
                com = new SqlCommand(strSql, con);
                retValue = (DateTime)com.ExecuteScalar();
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
            return retValue;
        }

        public DataSet GetUserCount(string UserID)
        {
            DataSet ds = new DataSet();
            try
            {
                con = ConnectionClass.getConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_GetUserCountFeeByUserId";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = UserID;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);
                DataLogger.Write("UserData-GetUserCount", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }

        public DataSet GetCost(int userCount, bool usageFee,string Action)
        {
            con = ConnectionClass.getConnection();
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {

                cmd.Connection = con;
                cmd.CommandText = "sp_GetPriceUserCareValue";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UserCount", SqlDbType.NVarChar).Value = userCount;
                cmd.Parameters.Add("@UserCareValue", SqlDbType.NVarChar).Value = usageFee;
                cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = Action;
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                ConnectionClass.closeconnection(con);
            }
            catch (Exception ex)
            {
                ConnectionClass.closeconnection(con);

                DataLogger.Write("UserData-GetCost", ex.Message);
            }
            finally
            {
                ConnectionClass.closeconnection(con);
            }
            return ds;
        }
    }
}
