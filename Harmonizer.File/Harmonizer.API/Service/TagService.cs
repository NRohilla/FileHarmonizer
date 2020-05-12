using Harmonizer.API.Models;
using Harmonizer.Core;
using Harmonizer.DB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Service
{
    public class TagService
    {
        TagData _tagData = new TagData();
        public TagModel GetTagInfo(TagModel tagModel)
        {
          
            return _tagData.GetTagInfo(tagModel);
        }
    }
}