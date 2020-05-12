using Harmonizer.API.Models;
using Harmonizer.API.Service;
using Harmonizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Harmonizer.API.Controllers
{
    [RoutePrefix("api/Tag")]
    public class TagController : ApiController
    {
        TagService _tagService = new TagService();
        // GET: api/Tag
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Tag/5
        [HttpPost]
        [Route("GetTagsInfo")]
        public TagModel GetTagsInfo(TagModel tagModel)
        {
            return _tagService.GetTagInfo(tagModel);
        }

        [HttpGet]
        [Route("GetTagsData")]
        public TagModel GetTagsData(string fhnumber,string associate,string tags)
        {
            TagModel tagModel = new TagModel();
            tagModel.Tags=new List<string>();
            tagModel.ShareValue = new List<string>();
            tagModel.FHnumber = fhnumber;
            tagModel.AssociateNumber = associate;
            var data= tags.Split(',');
            foreach(var item in data)
            {
                tagModel.Tags.Add(item);
            }

            return _tagService.GetTagInfo(tagModel); 
        }

    }
}
