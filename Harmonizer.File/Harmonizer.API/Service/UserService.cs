using Harmonizer.API.Models;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Service
{
    public class UserService
    {
        AdminData _admindata = new AdminData();
        UserData _userData = new UserData();
        FHFileData _fileData = new FHFileData();
        public List<PlanDetails> StartMyFreeMonth()
        {
            List<PlanDetails> lst = new List<PlanDetails>();
            DataSet ds = new DataSet();
            ds = _admindata.GetAllPlan("select");
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    PlanDetails planDetails = new PlanDetails();
                    planDetails.ID = Convert.ToInt32(row["ID"]);
                    planDetails.Title = row["Title"].ToString();
                    planDetails.Description = row["Description"].ToString();
                    planDetails.FreeInfo = row["FreeInfo"].ToString();
                    planDetails.Cost = Convert.ToDecimal(row["Cost"]);
                    planDetails.Validity = Convert.ToInt32(row["Validity"]);
                    planDetails.Year = Convert.ToInt32(row["Year"]);
                    planDetails.CreatedDate = Convert.ToDateTime(row["CreatedDate"]);
                    planDetails.UpdateDate = Convert.ToDateTime(row["UpdateDate"]);
                    planDetails.OrderDisplay = Convert.ToInt32(row["OrderDisplay"]);
                    planDetails.IsActive = Convert.ToBoolean(row["IsActive"]);
                    planDetails.Group = row["GroupOrder"].ToString();
                    lst.Add(planDetails);
                }
            }
            return lst;
        }
        public RegisterUser UserProfile(string BPID, string UserID, int RollID)
        {
            RegisterUser registerUser = new RegisterUser();
            User user = new User();
            PersonalInformation personalInformation = new PersonalInformation();
            AddressIinformation addressIinformation = new AddressIinformation();
            BPInfo bPInfo = new BPInfo();

            DataSet dsProfile = _userData.GetProfileDetails(BPID, UserID, RollID, "GetData");
            if (dsProfile.Tables[0].Rows.Count > 0)
            {
                addressIinformation.AddressID = Convert.ToInt32(dsProfile.Tables[0].Rows[0]["AddressID"]);
                addressIinformation.CompName = dsProfile.Tables[0].Rows[0]["CompName"].ToString();
                addressIinformation.CompName2 = dsProfile.Tables[0].Rows[0]["CompName2"].ToString();
                addressIinformation.Department = dsProfile.Tables[0].Rows[0]["Department"].ToString();
                addressIinformation.Address1 = dsProfile.Tables[0].Rows[0]["Address1"].ToString();
                addressIinformation.Address2 = dsProfile.Tables[0].Rows[0]["Address2"].ToString();
                addressIinformation.POBox = dsProfile.Tables[0].Rows[0]["POBox"].ToString();
                addressIinformation.City = dsProfile.Tables[0].Rows[0]["City"].ToString();
                addressIinformation.State = dsProfile.Tables[0].Rows[0]["State"].ToString();
                addressIinformation.Zip = dsProfile.Tables[0].Rows[0]["Zip"].ToString();
                addressIinformation.CountryKey = dsProfile.Tables[0].Rows[0]["CountryKey"].ToString();
                addressIinformation.Country = dsProfile.Tables[0].Rows[0]["Country"].ToString();
                addressIinformation.Language = dsProfile.Tables[0].Rows[0]["Language"].ToString();
                addressIinformation.Phone = dsProfile.Tables[0].Rows[0]["Phone"].ToString();
                addressIinformation.WorkPhone = dsProfile.Tables[0].Rows[0]["WorkPhone"].ToString();
                addressIinformation.MobilePhone = dsProfile.Tables[0].Rows[0]["MobilePhone"].ToString();
                addressIinformation.Fax = dsProfile.Tables[0].Rows[0]["Fax"].ToString();
                addressIinformation.TimeZone = dsProfile.Tables[0].Rows[0]["TimeZone"].ToString();
                addressIinformation.Location = dsProfile.Tables[0].Rows[0]["Location"].ToString();
                addressIinformation.DistrictPostalCode = dsProfile.Tables[0].Rows[0]["DistrictPostalCode"].ToString();
                addressIinformation.county = dsProfile.Tables[0].Rows[0]["County"].ToString();


                if (RollID == 2 || RollID == 6)
                {
                    personalInformation.PersonalID = Convert.ToInt32(dsProfile.Tables[0].Rows[0]["PersonalID"]);
                    personalInformation.BirthCountry = dsProfile.Tables[0].Rows[0]["BirthCountry"].ToString();
                    personalInformation.Country = dsProfile.Tables[0].Rows[0]["pCountry"].ToString();
                    personalInformation.DateOfBorn = Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["DateOfBorn"]);
                    personalInformation.Email = dsProfile.Tables[0].Rows[0]["Email"].ToString();
                    personalInformation.FirstName = dsProfile.Tables[0].Rows[0]["FirstName"].ToString();
                    personalInformation.LastName = dsProfile.Tables[0].Rows[0]["LastName"].ToString();
                    personalInformation.AKA = dsProfile.Tables[0].Rows[0]["AKA"].ToString();
                    personalInformation.Language = dsProfile.Tables[0].Rows[0]["pLanguage"].ToString();
                    personalInformation.LastName2 = dsProfile.Tables[0].Rows[0]["LastName2"].ToString();
                    personalInformation.Website = dsProfile.Tables[0].Rows[0]["Website"].ToString();
                    personalInformation.Profession = dsProfile.Tables[0].Rows[0]["Profession"].ToString();
                    personalInformation.Title = dsProfile.Tables[0].Rows[0]["Title"].ToString().Trim();
                    personalInformation.MiddleName = dsProfile.Tables[0].Rows[0]["MiddleName"].ToString();
                    personalInformation.Name2 = dsProfile.Tables[0].Rows[0]["Name2"].ToString();
                    personalInformation.Gender = dsProfile.Tables[0].Rows[0]["Gender"].ToString().Trim();
                }
                else if (RollID == 5)
                {

                    bPInfo.AddressID = Convert.ToInt32(dsProfile.Tables[0].Rows[0]["AddressID"]);
                    bPInfo.Country = dsProfile.Tables[0].Rows[0]["bpCountry"].ToString();
                    bPInfo.ContactNameFirst = dsProfile.Tables[0].Rows[0]["bpName"].ToString();
                    bPInfo.City = dsProfile.Tables[0].Rows[0]["bpCity"].ToString();
                    bPInfo.State = dsProfile.Tables[0].Rows[0]["bpState"].ToString();
                    bPInfo.Fax = dsProfile.Tables[0].Rows[0]["bpFax"].ToString();
                    bPInfo.Zip = dsProfile.Tables[0].Rows[0]["bpZip"].ToString();
                    bPInfo.Partner = dsProfile.Tables[0].Rows[0]["Partner"].ToString();
                    bPInfo.Language = dsProfile.Tables[0].Rows[0]["bpLanguage"].ToString();
                    bPInfo.TollFreePhone = dsProfile.Tables[0].Rows[0]["TollFreeNo"].ToString();
                    bPInfo.ContactNameLast = dsProfile.Tables[0].Rows[0]["bpLastName"].ToString();
                    bPInfo.CompName = dsProfile.Tables[0].Rows[0]["bpCompName"].ToString();
                    bPInfo.CompanyEIN = dsProfile.Tables[0].Rows[0]["CompanyEIN"].ToString();
                    bPInfo.CompanyDate = Convert.ToDateTime(dsProfile.Tables[0].Rows[0]["CompanyDate"]);
                    bPInfo.Email = dsProfile.Tables[0].Rows[0]["bpEmail"].ToString();
                    bPInfo.Department = dsProfile.Tables[0].Rows[0]["bpDepartment"].ToString();
                    bPInfo.Website = dsProfile.Tables[0].Rows[0]["bpWebsite"].ToString();
                }
            }
            registerUser.User = user;
            registerUser.PersonalInfo = personalInformation;
            registerUser.AddInfo = addressIinformation;
            registerUser.BPinfo = bPInfo;

            return registerUser;
        }

        public int UpdateUserPersonalInfo(PersonalInformation personalInformation)
        {
           return  _userData.UpdateUserPersonalInfo(personalInformation);
        }

        public int UpdateUserBPInfo (BPInfo bPInfo, string BPID)
        {
          return  _userData.UpdateUserBPInfo(bPInfo, BPID);
        }

        public int UpdateUserAddress(AddressIinformation Address,int RoleID)
        {
          return  _userData.UpdateUserAddress(Address, RoleID);
        }

        public List<Tag> ViewCustomTag(string BPID,string UserID)
        {
            return _userData.GetCustomeTag(BPID,UserID);
        }

        public int AddCustomTag(Tag obj, string UserID)
        {
            Tag _tag = new Tag();
            int Result = 0;
            _tag.ID = obj.ID;
            _tag.Orig = obj.Orig;
            _tag.Share = obj.Share;
            _tag.TagName = obj.TagName;
            _tag.UserID = UserID;
            _tag.UTAGID = obj.UTAGID;
            _tag.GlobPri = obj.GlobPri;
            _tag.Description = obj.Description;
            return Result = _fileData.AddTagNameDetails(_tag);
        }

        public int ForgotPassword(User user)
        {
            return _userData.ResetPassword(user);
        }
        public int PaymentHistory(PaymentModel paymentModel, string op)
        {
            return _userData.PaymentHistory(paymentModel, op);
        }
    }
}