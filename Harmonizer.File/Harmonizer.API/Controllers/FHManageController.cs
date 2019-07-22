using Harmonizer.API.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/FHManage")]
    public class FHManageController : ApiController
    {
        FHManageService _fHManageService = new FHManageService();
        public string UserID;
        public int Role;
        public string BPID;
        public FHManageController()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value });
            UserID = claims.FirstOrDefault(x => x.type == ClaimTypes.Name.ToString()).value;
            Role = Convert.ToInt32(claims.FirstOrDefault(x => x.type == ClaimTypes.Role.ToString()).value);
            BPID = claims.FirstOrDefault(x => x.type == "BPID").value;
        }

        [HttpGet]
        [Route("GetTemplateFileDetails")]
        public async Task<IHttpActionResult> GetTemplateFileDetails()
        {
            string HostName = ConfigurationManager.AppSettings["HostName"];
            string PortNo = ConfigurationManager.AppSettings["PortNo"];
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.TemplateFileDetails(UserID,BPID, HostName, PortNo));
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
        // ManageHarmonizeTemplateDetails

        [HttpGet]
        [Route("GetManageHarmonizeTemplateDetails")]
        public async Task<IHttpActionResult> ManageHarmonizeTemplateDetails()
        {
            string HostName = ConfigurationManager.AppSettings["HostName"];
            string PortNo = ConfigurationManager.AppSettings["PortNo"];
            string FHNumber = "";// This is for filter template which is generate from other Fhnumber data
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.ManageHarmonizeTemplateDetails(UserID, BPID, HostName, PortNo,FHNumber));
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
        [Route("ArchiveTemplate")]
        public async Task<IHttpActionResult> ArchiveTemplate(List<int> lstFileID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.ArchiveTemplate(lstFileID, BPID));
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
        [Route("UpdateAssignScheme")]
        public async Task<IHttpActionResult> UpdateAssignScheme(int schemenum, string FLTRID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.UpdateAssignScheme(schemenum, FLTRID, BPID));
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
        [Route("DeleteTemplate")]
        public async Task<IHttpActionResult> DeleteTemplate(int FileID,string FLTRID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.DeleteTemplate(FileID, BPID, FLTRID));
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
        [Route("RenameTemplate")]
        public async Task<IHttpActionResult> RenameTemplate(int FileID, string TemplateText, string Description, string HFLTRID, string IE, int op = 0)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.RenameTemplate(FileID, TemplateText, Description, HFLTRID, IE==null?"":IE, op));
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
        [Route("UpdateHarmonizeComment")]
        public async Task<IHttpActionResult> UpdateHarmonizeComment(int ID, string Comment)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var retValue = await Task.Run(() => _fHManageService.UpdateHarmonizeCommnet(ID, Comment));
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