using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class SendEmail
    {
       

        public static string CallForRate(string FName, string LName, string EmailId, string ContactNo, string Description)
        {
            string supportEmail = ConfigurationManager.AppSettings["supportEmailID"].ToString();
            string retValue = "Email sent successfully. Support team will contact you very soon.";
            SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress(EmailId);
                mm.To.Add(supportEmail); 
                mm.Subject = "Request for rate"; 
                mm.Body = "Hi Team,<br/>Please contact me.<br/> "+
                    "First Name:-"+FName+" <br/>Last Name:- "+LName+" <br/>Email:-"+EmailId+" <br/> Contact No. :- "+ContactNo+" <br/>Description :- "+Description+""+
                    "<br/><br/> Thanks,<br/> " + FName + " " + LName + "";

                mm.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = section.Network.Host; 
                smtp.EnableSsl =section.Network.EnableSsl;
                NetworkCredential NetworkCred = new NetworkCredential(section.Network.UserName, section.Network.Password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = section.Network.Port;
                mm.Priority = MailPriority.High;
                smtp.Timeout= 100000;
                try
                {
                    smtp.Send(mm);
                }
                catch(Exception ex)
                {
                    retValue = ex.Message;
                }
            }


            return retValue;
        }
    }
}