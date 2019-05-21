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
    [RoutePrefix("api/Scheme")]
    public class SchemeController : ApiController
    {
        SchemeService _schemeService = new SchemeService();

        public string UserID;
        public int Role;
        public string BPID;

        public SchemeController()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value });
            UserID = claims.FirstOrDefault(x => x.type == ClaimTypes.Name.ToString()).value;
            Role = Convert.ToInt32(claims.FirstOrDefault(x => x.type == ClaimTypes.Role.ToString()).value);
            BPID = claims.FirstOrDefault(x => x.type == "BPID").value;

        }
        [HttpGet]
        [Route("GetSerialnumber")]
        public async Task<IHttpActionResult> GetSerialNumber()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var srno = await Task.Run(() => _schemeService.GetShemeSRNo(BPID));
                return Ok(srno);
            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return ResponseMessage(response);
            }
            finally
            {

            }
        }


        [HttpPost]
        [Route("CreateScheme")]
        public async Task<IHttpActionResult> CreateScheme([FromBody]UserScheme userScheme)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retvalue = await Task.Run(() => _schemeService.CreateScheme(userScheme));
                return Ok(retvalue);
            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return ResponseMessage(response);
            }
            finally
            {

            }
        }

        [HttpGet]
        [Route("GetAllScheme")]
        public async Task<IHttpActionResult> GetAllScheme()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var schemedata = await Task.Run(() => _schemeService.GetAllSchemeData(BPID));
                return Ok(schemedata);
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
        [Route("UpdateComment")]
         
        public async Task<IHttpActionResult> UpdateComment(int ID, string Comment)
        {
          
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.UpdateSchemeComment(ID,Comment));
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
        [Route("DeleteScheme")]
        public async Task<IHttpActionResult> DeleteScheme(int ID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.DeleteScheme(ID));
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
        [Route("Archive")]
        public async Task<IHttpActionResult> ArchiveScheme([FromBody]List<int> lstID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.ArchiveScheme(lstID,BPID));
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
        [Route("ViewScheme")]
        public async Task<IHttpActionResult> ViewScheme(int ID)
        {
        
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.ViewScheme(ID,BPID));
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
        [Route("UpdateScheme")]
        public async Task<IHttpActionResult> UpadateScheme([FromBody]UserScheme userScheme)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.UpadateScheme(userScheme));
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
        [Route("ArchiveSchemeData")]
        public async Task<IHttpActionResult> ArchiveSchemeData()
        {
           
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.ArchiveSchemeData(BPID));
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
        [Route("UnArchiveScheme")]
        public async Task<IHttpActionResult> UnArchiveScheme(int ID)
        {
           
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.UnArchiveScheme(ID,BPID));
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
        [Route("CheckBPID")]
        public async Task<IHttpActionResult> CheckBPIDExist()
        {
          
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _schemeService.CheckBPIDExist(BPID));
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