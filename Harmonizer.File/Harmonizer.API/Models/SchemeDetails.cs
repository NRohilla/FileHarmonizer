using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Models
{
    public class SchemeDetails
    {
        public UserScheme SchemeInfo { get; set; }
        public List<UserScheme> SchemeDetail { get; set; }
    }
}