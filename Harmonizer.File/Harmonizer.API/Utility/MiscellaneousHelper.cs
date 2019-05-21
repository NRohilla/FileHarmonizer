using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Utility
{
    public class MiscellaneousHelper
    {
        public static DataTable ConvertToDataTableForRepostory(List<Repository> data, string UserId, string BPID)
        {

            DataTable table = new DataTable();
            table.Columns.Add("BPID", typeof(string));
            table.Columns.Add("UserID", typeof(string));
            table.Columns.Add("UTAGID", typeof(string));
            table.Columns.Add("Tag", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Org", typeof(string));
            table.Columns.Add("GlobPri", typeof(string));
            table.Columns.Add("Share", typeof(string));
            table.Columns.Add("TemplateID", typeof(Int64));

            for (int i = 0; i < data.ToList().Count; i++)
                table.Rows.Add(new object[] {
                            data[i].BPID =BPID,
                            data[i].UserID =UserId,
                            data[i].UTAGID,
                            data[i].Tag,
                            data[i].Description,
                            data[i].Org,
                            data[i].GlobPri,
                            data[i].Share,
                            data[i].TemplateID

                           });

            return table;
        }
    }
}