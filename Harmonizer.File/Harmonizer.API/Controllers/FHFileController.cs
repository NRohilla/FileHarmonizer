using Harmonizer.API.Service;
using Harmonizer.API.Utility;
using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/FHFile")]
    public class FHFileController : ApiController
    {
        FHFileService _fhFileService = new FHFileService();
        public string UserID;
        public int Role;
        public string BPID;


        public FHFileController()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var claims = claimsIdentity.Claims.Select(x => new { type = x.Type, value = x.Value });
            UserID = claims.FirstOrDefault(x => x.type == ClaimTypes.Name.ToString()).value;
            Role = Convert.ToInt32(claims.FirstOrDefault(x => x.type == ClaimTypes.Role.ToString()).value);
            BPID = claims.FirstOrDefault(x => x.type == "BPID").value;
        }

        [HttpGet]
        [Route("ViewAllUserTag")]
        public async Task<IHttpActionResult> ViewAllTag()
        {

            HttpResponseMessage response=new HttpResponseMessage();
            try
            {
                var TagList = await Task.Run(() => _fhFileService.ViewAllTag(BPID, UserID));
                if (TagList != null)
                    return Ok(TagList);
                else
                    return NotFound();
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
        [Route("ExternalTemplate")]
        public async Task<IHttpActionResult> ViewAllExternalTemplae(string FHNumber, string SECID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var ExternalTemplate = await Task.Run(() => _fhFileService.GetAllExternalTemplate(FHNumber, SECID, BPID));
                if (ExternalTemplate != null)  
                    return Ok(ExternalTemplate);
                else
                    return NotFound();

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
        [Route("BusinessFilter")]
        public async Task<IHttpActionResult> GetBusinessFilter(string BPIDOrFH, string SECID, int FileID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var ExternalTemplate = await Task.Run(() => _fhFileService.GetBusinessFilter(BPIDOrFH, SECID, FileID, BPID));
                if (ExternalTemplate != null)
                    return Ok(ExternalTemplate);
                else
                    return NotFound();

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
        [Route("SaveRepository")]
        public async Task<IHttpActionResult> SaveRepositoryInfo([FromBody] List<Repository> lstSearch)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DataTable dt = new DataTable();
            try
            {
                dt = MiscellaneousHelper.ConvertToDataTableForRepostory(lstSearch, UserID, BPID);
                await Task.Run(() => _fhFileService.SaveRepositoryInfo(dt,BPID));
                return Ok();
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
        [Route("GetTemplate")]
        public async Task<IHttpActionResult> GetTemplateDataForFinalTemplate()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
               var data= await Task.Run(() => _fhFileService.GetTemplateDataForFinalTemplate(BPID));
                return Ok(data);
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
        [Route("TemplateFilterTag")]
        public async Task<IHttpActionResult> GetTemplateDetail(int FileID)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var data = await Task.Run(() => _fhFileService.GetTemplateDetail(FileID,BPID,UserID));
                return Ok(data);

            }
            catch(Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
                return ResponseMessage(response);
            }
        }


    }
}