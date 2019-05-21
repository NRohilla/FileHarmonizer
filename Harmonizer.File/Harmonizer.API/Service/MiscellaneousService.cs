using Harmonizer.API.Models;
using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;


namespace Harmonizer.API.Service
{
    public class MiscellaneousService
    {
        UserData _userData = new UserData();
        AdminData _admindata = new AdminData();
        Scheme _scheme = new Scheme();
        public  List<SelectListItem> GetIndustries()
        {
            List<SelectListItem> lstIndustry = new List<SelectListItem>();
            DataSet ds = _userData.GetIndustry();
            lstIndustry.Add(new SelectListItem { Text = "Select Industry", Value = "0" });
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    lstIndustry.Add(new SelectListItem { Text = ds.Tables[0].Rows[i]["Industry"].ToString(), Value = ds.Tables[0].Rows[i]["Share"].ToString() });
                }
            }
            return lstIndustry;
        }



        public List<Country> GetCountry()
        {
            List<Country> objCountrylst = new List<Country>();
            objCountrylst = _admindata.GetCountryList();
            objCountrylst.Insert(0, new Country { ID = 0, Alpha3 = "0", Alpha2 = "0", CountryName = "Select Country" });
            return objCountrylst;
        }

        public List<LanguageTimeZone> GetLanguage()
        {
            List<LanguageTimeZone> lst = new List<LanguageTimeZone>();
            DataSet ds = _userData.GetLanguageList();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    LanguageTimeZone lstLang = new LanguageTimeZone();
                    lstLang.Language = ds.Tables[0].Rows[i]["Language"].ToString();
                    lstLang.LanguageDescription = ds.Tables[0].Rows[i]["Language"].ToString() + "-" + ds.Tables[0].Rows[i]["LanguageDescription"].ToString();
                    lst.Add(lstLang);
                }
            }
            lst.Insert(0, new LanguageTimeZone { Language = "Select Language", LanguageDescription = "Select Language" });
            return lst;
        }


        public List<SelectListItem> GetSchemlist(string BPID)
        {
            List<SelectListItem> lstSchme = new List<SelectListItem>();
            DataSet dsScheme = _scheme.GetAllSchemeByBPID(BPID, "selectall");
            lstSchme.Add(new SelectListItem { Text = "-Scheme-", Value = "0" });
            if (dsScheme.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsScheme.Tables[0].Rows.Count; i++)
                {
                    lstSchme.Add(new SelectListItem { Text = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() + "-" + dsScheme.Tables[0].Rows[i]["SchemeName"].ToString(), Value = dsScheme.Tables[0].Rows[i]["SchemeNum"].ToString() });
                }
            }
            return lstSchme;
        }

        public int SendFeedback(FeedbackModel feedback)
        {
            int retValue = -1;
            string supportEmail = ConfigurationManager.AppSettings["supportEmailID"].ToString();
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress(feedback.Email);
                mm.To.Add(supportEmail);
                mm.Subject = "Request for rate";
                mm.Body = "Hi Team,<br/>Feedback data from mobile App<br/> " +
                    "First Name:-" + feedback.Name + " <br/>Email:- " + feedback.Email + " <br/>Contat No.:-" + feedback.ContactNo + " <br/>Subject :-"+feedback.Subject+" <br/> Description :- " + feedback.Brief + "" +
                    "<br/><br/> Thanks,<br/> " + feedback.Name + "";

                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = section.Network.Host;
                smtp.EnableSsl = section.Network.EnableSsl;
                NetworkCredential NetworkCred = new NetworkCredential(section.Network.UserName, section.Network.Password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = section.Network.Port;
                mm.Priority = MailPriority.High;
                smtp.Timeout = 100000;
                try
                {
                    smtp.Send(mm);
                    retValue = 1;
                }
                catch (Exception ex)
                {
                    retValue = -1;
                }
            }
            return retValue;
        }

    }


}