using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class ManageFilterTemplateModel
    {
        public Template Template { get; set; }
        public CHFilter Filter { get; set; }
        public FileUploadDownload FileUploadDownload { get; set; }
        public HarmonizeTemplate HarmonizeTemplate { get; set; }
    }
}