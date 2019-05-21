using Harmonizer.API.Service;
using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        MiscellaneousService miscellaneousService = new MiscellaneousService();
        UserService userService = new UserService();
        public string UserID;
        public int Role;
        public string BPID;

        public UserController()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value });
            UserID = claims.FirstOrDefault(x => x.type == ClaimTypes.Name.ToString()).value;
            Role = Convert.ToInt32(claims.FirstOrDefault(x => x.type == ClaimTypes.Role.ToString()).value);
            BPID = claims.FirstOrDefault(x => x.type == "BPID").value;
        }

        [HttpGet]
        [Route("StartFreeTrail")]
        public async Task<IHttpActionResult> StartFreeTrail()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var PlanDetails = await Task.Run(() => userService.StartMyFreeMonth());
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

        [HttpGet]
        [Route("UserProfile")]
        public async Task<IHttpActionResult> UserProfile()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var PlanDetails = await Task.Run(() => userService.UserProfile(BPID,UserID,Role));
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
        [Route("UpdatePersonalInfo")]
        public async Task<IHttpActionResult> UpdateUserPersonalInfo([FromBody]PersonalInformation personalInformation)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                personalInformation.UserID = UserID;
                var retValue = await Task.Run(() => userService.UpdateUserPersonalInfo(personalInformation));
                return Ok(retValue);
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
        [Route("UpdateBPInfo")]
        public async Task<IHttpActionResult> UpdateUserBPInfo([FromBody]BPInfo bPInfo)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                bPInfo.UserID = UserID;
                var retValue = await Task.Run(() => userService.UpdateUserBPInfo(bPInfo, BPID));
                return Ok(retValue);
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
        [Route("UpdateAddress")]
        public async Task<IHttpActionResult> UpdateUserAddress([FromBody]AddressIinformation Address)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                Address.UserID = UserID;
                var retValue = await Task.Run(() => userService.UpdateUserAddress(Address,Role));
                return Ok(retValue);
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


        [HttpGet]
        [Route("ViewCustomTag")]
        public async Task<IHttpActionResult> ViewCustomTag()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var CustomTag = await Task.Run(() => userService.ViewCustomTag(BPID,UserID));
                return Ok(CustomTag);
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
        [Route("AddCustomTag")]
        public async Task<IHttpActionResult> AddCustomTag([FromBody]Tag tag)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => userService.AddCustomTag(tag, UserID));
                return Ok(retValue);
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

        [HttpGet]
        [Route("GetIndustry")]
        public async Task<IHttpActionResult> Industry()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => miscellaneousService.GetIndustries());
                return Ok(retValue);
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


        [HttpGet]
        [Route("GetCountry")]
        public async Task<IHttpActionResult> Country()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => miscellaneousService.GetCountry());
                return Ok(retValue);
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
        [HttpGet]
        [Route("GetLanguage")]
        public async Task<IHttpActionResult> Language()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => miscellaneousService.GetLanguage());
                return Ok(retValue);
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


        [HttpGet]
        [Route("GetSchemeList")]
        public async Task<IHttpActionResult> GetSchemeList()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => miscellaneousService.GetSchemlist(BPID));
                return Ok(retValue);
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