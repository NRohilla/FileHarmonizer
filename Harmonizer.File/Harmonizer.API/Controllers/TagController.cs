using Harmonizer.API.Models;
using Harmonizer.API.Service;
using Harmonizer.Core;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [RoutePrefix("api/tag_values/V1")]
    public class TagController : ApiController
    {
        TagService _tagService = new TagService();
        TagData _tagData = new TagData();

        [HttpGet]
        [Route("tagval")]
        public IHttpActionResult GetTagsData(string fhnumber, string associate, string tags)
        {
            TagModel tagModel = new TagModel();
            tagModel.Tags = new List<string>();
            tagModel.FHnumber = fhnumber;
            tagModel.AssociateNumber = associate;
            var data = tags.Split(',');
            foreach (var item in data)
            {
                tagModel.Tags.Add(item);
            }
            return Ok(_tagService.GetTagInfo(tagModel));
        }

        [HttpGet]
        [Route("tagval")]
        public IHttpActionResult GetTagsData(string APIkey, string fhnumber, string associate, string tags)
        {
            bool IsAPIKeyExist = _tagData.CheckAPIByAPIKey(APIkey);
            if (IsAPIKeyExist)
            {
                TagModel tagModel = new TagModel();
                tagModel.Tags = new List<string>();
                tagModel.FHnumber = fhnumber;
                tagModel.AssociateNumber = associate;
                var data = tags.Split(',');
                foreach (var item in data)
                {
                    tagModel.Tags.Add(item);
                }
                return Ok(_tagService.GetTagInfo(tagModel));
            }
            return BadRequest("API Key is not Valid");
        }
    }
}
