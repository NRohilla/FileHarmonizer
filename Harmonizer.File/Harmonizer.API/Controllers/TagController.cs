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
    [RoutePrefix("api/tag_values/V1")]
    public class TagController : ApiController
    {
        TagService _tagService = new TagService();
    
        [HttpGet]
        [Route("tagval")]
        public List<TagViewModel> GetTagsData(string fhnumber, string associate, string tags)
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
            return _tagService.GetTagInfo(tagModel);
        }
    }
}
