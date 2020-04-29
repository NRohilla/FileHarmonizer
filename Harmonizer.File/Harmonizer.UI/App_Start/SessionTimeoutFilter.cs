using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace Harmonizer.UI.App_Start
{
    [AttributeUsage(AttributeTargets.Class |
   AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionTimeoutFilter : ActionFilterAttribute
    {
        public string token = "";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            UserData _userData = new UserData();
            string UserIPAddrss = HttpContext.Current.Request.UserHostAddress;
            string UserBrowserName = HttpContext.Current.Request.Browser.Browser.ToLower().Trim();
            string contolername = filterContext.RouteData.Values["controller"].ToString();
            string actioname = filterContext.RouteData.Values["action"].ToString();

            //for get request
           
            if (contolername.ToLower() == "Payment".ToLower() && (actioname.ToLower() == "PaymentSucess".ToLower() || actioname.ToLower() == "PaymentCancel".ToLower()))
            { 
                if (HttpContext.Current.Request.QueryString["usertoken"] != null)
                    token = HttpContext.Current.Request.QueryString["usertoken"];
                else if (HttpContext.Current.Request.UrlReferrer != null && HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)["usertoken"] != null)
                    token = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)["usertoken"];
            }
            else
            {
                if (HttpContext.Current.Request.QueryString["token"] != null)
                    token = HttpContext.Current.Request.QueryString["token"];
                else if (HttpContext.Current.Request.UrlReferrer != null && HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)["token"] != null)
                    token = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)["token"];

            }

            try
            {
                HttpContext ctx = HttpContext.Current;
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    DataLogger.Write("session filter" + contolername + "-" + actioname, "Session ended at: " + DateTime.Now.ToString());
                    // Check and fill session data
                    DataLogger.Write("session filter" + contolername + "-" + actioname, "Select token: " + DateTime.Now.ToString() + "::"+ token);
                    DataSet ds = _userData.GetCustomSessionData(UserIPAddrss, UserBrowserName, token, "select");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        HttpContext.Current.Session["UserID"] = ds.Tables[0].Rows[0]["UserID"];
                        HttpContext.Current.Session["Role"] = Convert.ToInt32(ds.Tables[0].Rows[0]["Role"]);
                        HttpContext.Current.Session["Email"] = ds.Tables[0].Rows[0]["EmailID"];
                        HttpContext.Current.Session["SECID"] = ds.Tables[0].Rows[0]["SECID"];
                        HttpContext.Current.Session["BPID"] = ds.Tables[0].Rows[0]["BPID"];
                        HttpContext.Current.Session["Partner"] = ds.Tables[0].Rows[0]["Partner"];
                        HttpContext.Current.Session["BPType"] = ds.Tables[0].Rows[0]["BPType"];
                        DataLogger.Write("session filter" + contolername + "-" + actioname, "Session Refilled at: " + DateTime.Now.ToString() + " for token:" + token);
                        
                            // Check Expire date
                        if (actioname.ToLower() != "UserProfile".ToLower() && actioname.ToLower() != "StartMyFreeMonth".ToLower())
                        {
                            if (actioname.ToLower() != "PaymentSucess".ToLower() && actioname.ToLower() != "PaymentCancel".ToLower())
                            {
                                if (_userData.GetExpiredate(HttpContext.Current.Session["UserID"].ToString()) < DateTime.Today.Date)
                                {
                                    ////Changed by Nitin 28042020
                                    filterContext.Controller.TempData["expireddate"] = _userData.GetExpiredate(HttpContext.Current.Session["UserID"].ToString());
                                    filterContext.Controller.TempData.Keep();
                                    ////this will stop the page from going to User Profile 
                                    //filterContext.Result = new RedirectResult("~/User/UserProfile?token=" + token);
                                }
                            }
                        }
                        // check role
                        if(contolername.ToLower() == "Admin".ToLower())
                        {
                            if(Convert.ToInt32(HttpContext.Current.Session["Role"]) != 1 && Convert.ToInt32(HttpContext.Current.Session["Role"]) != 7)
                                filterContext.Result = new RedirectResult("~/Home/Index?token="+token);
                        }
                    }
                    else if(actioname.ToLower() == "StartMyFreeMonth".ToLower())
                    {

                    }
                    else
                    {
                        FormsAuthentication.SignOut();
                        HttpContext.Current.Session.Clear();
                        HttpContext.Current.Session.Abandon();
                        filterContext.Result = new RedirectResult("~/Account/SignIn");
                    }
                }
                else
                {
                    // Check Expire date
                    if (actioname.ToLower() != "UserProfile".ToLower() && actioname.ToLower() != "StartMyFreeMonth".ToLower())
                    {
                        if (actioname.ToLower() != "PaymentSucess".ToLower() && actioname.ToLower() != "PaymentCancel".ToLower())
                        {
                            if (_userData.GetExpiredate(HttpContext.Current.Session["UserID"].ToString()) < DateTime.Today.Date)
                            {
                                ////Changed by Nitin 28042020
                                filterContext.Controller.TempData["expireddate"] = _userData.GetExpiredate(HttpContext.Current.Session["UserID"].ToString());
                                filterContext.Controller.TempData.Keep();
                                ////this will stop the page from going to User Profile 
                                //filterContext.Result = new RedirectResult("~/User/UserProfile?token=" + token);
                            }
                        }
                    }

                    if (contolername.ToLower() == "Admin".ToLower())
                    {
                        if (Convert.ToInt32(HttpContext.Current.Session["Role"]) != 1 && Convert.ToInt32(HttpContext.Current.Session["Role"]) !=7)
                            filterContext.Result = new RedirectResult("~/Home/Index?token=" + token);
                    }

                    DataLogger.Write("session filter" + contolername + "-" + actioname, "Session still active at: " + DateTime.Now.ToString() + " ; HttpContext.Current.Session[UserID]=" + HttpContext.Current.Session["UserID"]);
                }
            }
            catch (Exception ex)
            {
                DataLogger.Write("Exception session filter" + contolername + "-" + actioname, ex.Message);
                filterContext.Result = new RedirectResult("~/Account/SignIn");
            }
            base.OnActionExecuting(filterContext);
        }
    }

    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    //public class SessionTimeoutFilter : AuthorizeAttribute
    //{
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        string contolername = filterContext.RouteData.Values["controller"].ToString();
    //        string actioname = filterContext.RouteData.Values["action"].ToString();

    //        //HttpContext ctx = HttpContext.Current;
    //        //// check if session is supported  
    //        //if (ctx.Session != null)
    //        //{
    //        //    // check if a new session id was generated  
    //        //    if (ctx.Session["UserID"] == null || ctx.Session.IsNewSession)
    //        //    {
    //        //        //Check is Ajax request  
    //        //        if (filterContext.HttpContext.Request.IsAjaxRequest())
    //        //        {
    //        //            DataLogger.Write("session filter ajax" + contolername + "-" + actioname, "Ajax request at: " + DateTime.Now.ToString());
    //        //            filterContext.HttpContext.Response.ClearContent();
    //        //            filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
    //        //        }
    //        //        // check if a new session id was generated  
    //        //        else
    //        //        {
    //        //            DataLogger.Write("session filter" + contolername + "-" + actioname, "Session ended at: " + DateTime.Now.ToString());
    //        //            filterContext.Result = new RedirectResult("~/Account/SignIn");
    //        //        }
    //        //    }
    //        //}
    //        base.HandleUnauthorizedRequest(filterContext);
    //    }
    //}


    [AttributeUsage(AttributeTargets.Class |
   AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class GeneralExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string contolername = filterContext.RouteData.Values["controller"].ToString();
            string actioname = filterContext.RouteData.Values["action"].ToString();
            DataLogger.Write("execption_1:-" + contolername + "-" + actioname, filterContext.Exception.Message);

            if (!filterContext.ExceptionHandled)
            {
                contolername = filterContext.RouteData.Values["controller"].ToString();
                actioname = filterContext.RouteData.Values["action"].ToString();
                DataLogger.Write("execption_2:-" + contolername + "-" + actioname, filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
            }
        }
    }


}