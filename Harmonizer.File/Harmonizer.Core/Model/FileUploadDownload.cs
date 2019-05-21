using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonizer.Core.Model
{
   public class FileUploadDownload
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string BPID { get; set; }
        public string SECID { get; set; }
        public DateTime DataDate { get; set; }
        public string Time { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int NoOfOccerance { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string DownloadBy { get; set; }
        public DateTime DownloadDate { get; set; }
        public string OrginalFileName { get; set; }
        public string FileExt { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string SearchData { get; set; }
        public string ReplaceDate { get; set; }
        public string Tag { get; set; }
        public bool IsArchive { get; set; }
        public int NoOfDownloads { get; set; }
        public string TemplateName { get; set; }
        public string NewFileName { get; set; }
        public bool IsDeleted { get; set; }
        public string TemplateDownloadPath { get; set; }
    }
}
