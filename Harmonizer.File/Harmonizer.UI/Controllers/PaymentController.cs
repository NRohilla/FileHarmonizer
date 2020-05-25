using Harmonizer.Core.Model;
using Harmonizer.DB.Data;
using Harmonizer.UI.App_Start;
using Harmonizer.UI.Models;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Harmonizer.UI.Controllers
{
    
    public class PaymentController : Controller
    {
        UserData _userdata = new UserData();
        AdminData _admindata = new AdminData();
        PaymentModel paymentModel = new PaymentModel();
        // GET: Payment
        public ActionResult PaymentWithPaypal(string Cancel = null,string planid=null,string token=null)
        {
            DataLogger.Write("PaymentWithPaypal", "First Call");

            paymentModel = new PaymentModel();
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                DataLogger.Write("PaymentWithPaypal-", "Inter Try block");
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Payment/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid,planid, guid, token);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    DataLogger.Write("PaymentWithPaypal-If condition", ""+ createdPayment.id +"-Redirect url-"+ paypalRedirectUrl);
                    // saving the paymentID in the key guid
                    //Session.Add(guid, createdPayment.id);
                    Session[guid] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    //// This function exectues after receving all parameters for the payment

                    //var guid = Request.Params["guid"];

                    //var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                   
                    //paymentModel.amount = executedPayment.transactions.FirstOrDefault().amount.total;
                    //paymentModel.cart = executedPayment.cart;
                    //paymentModel.create_time =Convert.ToDateTime( executedPayment.create_time);
                    //paymentModel.currency = executedPayment.transactions.FirstOrDefault().amount.currency;
                    //paymentModel.description = executedPayment.transactions.FirstOrDefault().description;
                    //paymentModel.invoice = executedPayment.transactions.FirstOrDefault().invoice_number;
                    //paymentModel.paymentmethod = executedPayment.payer.payment_method;
                    //paymentModel.state = executedPayment.state;
                    //paymentModel.id = executedPayment.id;
                    //paymentModel.payerId = payerId;
                    //paymentModel.failuarreasion = executedPayment.failure_reason;
                    //paymentModel.TransactionId = executedPayment.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
                    //paymentModel.PlanId = planid;
                    ////If executed payment failed then we will show payment failure message to user
                    //if (executedPayment.state.ToLower() != "approved")
                    //{
                    //    return View("PaymetFailure", paymentModel);
                    //}
                }
            }
            catch(PayPal.PayPalException ex)
            {
                DataLogger.Write("PaymentWithPaypal-Exception on first call", ex.Message);
                paymentModel.failuerexception = ex.Message;
                // logger to write 
                return View("PaymetFailure", paymentModel);
            }
            catch(Exception e)
            {
                DataLogger.Write("PaymentWithPaypal-General exception", e.Message);
                return View("PaymetFailure", paymentModel);
            }
            return View("PaymentSuccess", paymentModel);
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl, string planId, string guid,string token)
        {
            DataLogger.Write("PaymentWithPaypal-CreatePayment", "apiContext");


            PlanDetails planDetails = new PlanDetails();

            string BPtype = null;
            if (Session["BPType"] != null)
            {
                BPtype = Session["BPType"].ToString();
            }


            planDetails = _admindata.GetPlanByPlanId(planId);

            //create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };

            DataSet ds = new DataSet();
            decimal cost=0;
            ds = _userdata.GetUserCount(Session["UserID"].ToString());
            if (BPtype == "CUST")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int userCount = Convert.ToInt32(ds.Tables[0].Rows[0]["NoofUsers"]);
                    bool usageFee = Convert.ToBoolean(ds.Tables[0].Rows[0]["UsageFee"]);

                    ds = _userdata.GetCost(userCount, usageFee, "CUST");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
                    }
                }

                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = planDetails.Title,
                    currency = "USD",
                    price = Convert.ToString(cost),
                    quantity = "1",
                    sku = "sku"
                });

                var payer = new Payer() { payment_method = "paypal" };
                //var payer = new Payer() {  payment_method = "credit_card" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    //Request.Url.Scheme + "://" + Request.Url.Authority +"/Payment/PaymentWithPayPal?";
                    cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentCancel?planid=" + planId + "&usertoken=" + token + "&guid=" + guid, //redirectUrl + "&Cancel=true",
                    return_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentSucess?planid=" + planId + "&usertoken=" + token + "&guid=" + guid
                };

                // Adding Tax, shipping and Subtotal details
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = Convert.ToString(cost)
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "USD",
                    total = Convert.ToString(cost), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {

                    description = planDetails.Description,
                    invoice_number = Convert.ToString((new Random()).Next(100000)), //"your invoice number", //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });


                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }
           else if (BPtype == "VEND")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int userCount = Convert.ToInt32(ds.Tables[0].Rows[0]["NoofUsers"]);
                    bool usageFee = Convert.ToBoolean(ds.Tables[0].Rows[0]["UsageFee"]);

                    ds = _userdata.GetCost(userCount, usageFee, "VEND");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
                    }
                }
                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = planDetails.Title,
                    currency = "USD",
                    price = Convert.ToString(cost),
                    quantity = "1",
                    sku = "sku"
                });

                var payer = new Payer() { payment_method = "paypal" };
                //var payer = new Payer() {  payment_method = "credit_card" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    //Request.Url.Scheme + "://" + Request.Url.Authority +"/Payment/PaymentWithPayPal?";
                    cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentCancel?planid=" + planId + "&usertoken=" + token + "&guid=" + guid, //redirectUrl + "&Cancel=true",
                    return_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentSucess?planid=" + planId + "&usertoken=" + token + "&guid=" + guid
                };

                // Adding Tax, shipping and Subtotal details
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = Convert.ToString(cost)
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "USD",
                    total = Convert.ToString(cost), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {

                    description = planDetails.Description,
                    invoice_number = Convert.ToString((new Random()).Next(100000)), //"your invoice number", //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });


                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }
            else if (BPtype == "SPROV")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int userCount = Convert.ToInt32(ds.Tables[0].Rows[0]["NoofUsers"]);
                    bool usageFee = Convert.ToBoolean(ds.Tables[0].Rows[0]["UsageFee"]);

                    ds = _userdata.GetCost(userCount, usageFee, "VEND");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cost = Convert.ToDecimal(ds.Tables[0].Rows[0]["Amount"]);
                    }
                }
                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = planDetails.Title,
                    currency = "USD",
                    price = Convert.ToString(cost),
                    quantity = "1",
                    sku = "sku"
                });

                var payer = new Payer() { payment_method = "paypal" };
                //var payer = new Payer() {  payment_method = "credit_card" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    //Request.Url.Scheme + "://" + Request.Url.Authority +"/Payment/PaymentWithPayPal?";
                    cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentCancel?planid=" + planId + "&usertoken=" + token + "&guid=" + guid, //redirectUrl + "&Cancel=true",
                    return_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentSucess?planid=" + planId + "&usertoken=" + token + "&guid=" + guid
                };

                // Adding Tax, shipping and Subtotal details
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = Convert.ToString(cost)
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "USD",
                    total = Convert.ToString(cost), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {

                    description = planDetails.Description,
                    invoice_number = Convert.ToString((new Random()).Next(100000)), //"your invoice number", //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });


                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }
            else
            {
                //Adding Item Details like name, currency, price etc
                itemList.items.Add(new Item()
                {
                    name = planDetails.Title,
                    currency = "USD",
                    price = Convert.ToString(planDetails.Cost),
                    quantity = "1",
                    sku = "sku"
                });

                var payer = new Payer() { payment_method = "paypal" };
                //var payer = new Payer() {  payment_method = "credit_card" };

                // Configure Redirect Urls here with RedirectUrls object
                var redirUrls = new RedirectUrls()
                {
                    //Request.Url.Scheme + "://" + Request.Url.Authority +"/Payment/PaymentWithPayPal?";
                    cancel_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentCancel?planid=" + planId + "&usertoken=" + token + "&guid=" + guid, //redirectUrl + "&Cancel=true",
                    return_url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentSucess?planid=" + planId + "&usertoken=" + token + "&guid=" + guid
                };

                // Adding Tax, shipping and Subtotal details
                var details = new Details()
                {
                    tax = "0",
                    shipping = "0",
                    subtotal = Convert.ToString(planDetails.Cost)
                };

                //Final amount with details
                var amount = new Amount()
                {
                    currency = "USD",
                    total = Convert.ToString(planDetails.Cost), // Total must be equal to sum of tax, shipping and subtotal.
                    details = details
                };

                var transactionList = new List<Transaction>();
                // Adding description about the transaction
                transactionList.Add(new Transaction()
                {

                    description = planDetails.Description,
                    invoice_number = Convert.ToString((new Random()).Next(100000)), //"your invoice number", //Generate an Invoice No
                    amount = amount,
                    item_list = itemList
                });


                this.payment = new Payment()
                {
                    intent = "sale",
                    payer = payer,
                    transactions = transactionList,
                    redirect_urls = redirUrls
                };
            }

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

        [SessionTimeoutFilter]
        public ActionResult PaymentSucess()
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            paymentModel = new PaymentModel();
            try
            {
                string payerId = Request.Params["PayerID"];
                var guid = Request.Params["guid"];
                var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                paymentModel.amount = executedPayment.transactions.FirstOrDefault().amount.total;
                paymentModel.cart = executedPayment.cart;
                paymentModel.create_time = Convert.ToDateTime(executedPayment.create_time);
                paymentModel.currency = executedPayment.transactions.FirstOrDefault().amount.currency;
                paymentModel.description = executedPayment.transactions.FirstOrDefault().description;
                paymentModel.invoice = executedPayment.transactions.FirstOrDefault().invoice_number;
                paymentModel.paymentmethod = executedPayment.payer.payment_method;
                paymentModel.state = executedPayment.state;
                paymentModel.id = executedPayment.id;
                paymentModel.payerId = payerId;
                paymentModel.failuarreasion = executedPayment.failure_reason;
                paymentModel.TransactionId = executedPayment.transactions.FirstOrDefault().related_resources.FirstOrDefault().sale.id;
                paymentModel.PlanId = Request.Params["planid"];
                paymentModel.UserId = Session["UserID"].ToString();
                paymentModel.BPID = Session["BPID"].ToString();
                paymentModel.TokenId = Request.Params["usertoken"];

                //If executed payment failed then we will show payment failure message to user
                if (executedPayment.state.ToLower() != "approved")
                {
                    return View("PaymetFailure", paymentModel);
                }
                else
                {
                    // processs for store history
                    try
                    {
                        int retStoreHistory = _userdata.PaymentHistory(paymentModel, "insert");
                        if (retStoreHistory > 0)
                            DataLogger.Write("Payment-Store History", "Success to store: "+ paymentModel.payerId);
                        else
                            DataLogger.Write("Payment-Store History", "Failed to store: "+ paymentModel.payerId);

                    }
                    catch(Exception ex)
                    {
                        // failed to store history
                        DataLogger.Write("Payment-Store History", ex.Message);
                    }
                }
            }
            catch (PayPal.PayPalException ex)
            {
                DataLogger.Write("Payment-PaymentSucess paypal exception-", ex.Message);
                paymentModel.TokenId = Request.Params["usertoken"];
                paymentModel.failuerexception = ex.Message;
                return View("PaymetFailure", paymentModel);
            }
            catch(Exception e)
            {
                DataLogger.Write("Payment-PaymentSucess-General Exception-", e.Message);
                paymentModel.TokenId = Request.Params["usertoken"];
                paymentModel.failuerexception = e.Message;
                return View("PaymetFailure", paymentModel);
            }
            return View("PaymentSuccess", paymentModel);
        }
        [SessionTimeoutFilter]
        public ActionResult PaymentCancel()
        {
            paymentModel = new PaymentModel();
            paymentModel.TokenId = Request.Params["usertoken"];
            return View("PaymetFailure", paymentModel);
        }
        public ActionResult MPaymentsuccess()
        {
            return View();
        }
        public ActionResult MPaymentCanceled()
        {
            return View();
        }
    }
}