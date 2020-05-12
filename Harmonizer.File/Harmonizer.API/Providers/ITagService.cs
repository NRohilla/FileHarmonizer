using Harmonizer.API.Models;
using Harmonizer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.API.Providers
{
    public interface ITagService
    {
        object GetTagInfo(TagModel tagModel);
    }
}
