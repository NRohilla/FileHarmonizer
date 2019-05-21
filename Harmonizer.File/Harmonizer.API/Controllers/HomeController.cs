using Harmonizer.API.Models;
using Harmonizer.API.Service;
using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : ApiController
    {
        MiscellaneousService _miscellaneousService = new MiscellaneousService();
        UserService _userService = new UserService();
        [HttpPost]
        [Route("Feedback")]
        public async Task<IHttpActionResult> SendFeeback([FromBody]FeedbackModel feedbackModel)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var PlanDetails = await Task.Run(() => _miscellaneousService.SendFeedback(feedbackModel));
                return Ok(PlanDetails);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return ResponseMessage(response);
            }
            finally
            {

            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword([FromBody]ForgotPasswordModel forgotPasswordModel)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                User user = new User() {
                    UserID=forgotPasswordModel.UserId,
                    Password=forgotPasswordModel.Password,
                    EmailID=forgotPasswordModel.Email
                };
                var returnValue = await Task.Run(() => _userService.ForgotPassword(user));
                return Ok(returnValue);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return ResponseMessage(response);
            }
            finally
            {

            }
        }
    }
}