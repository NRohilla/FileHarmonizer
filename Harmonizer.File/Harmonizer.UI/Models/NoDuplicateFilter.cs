using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class NoDuplicateFilter : IEqualityComparer<ManageFilterTemplateModel>
    {
        public bool Equals(ManageFilterTemplateModel x, ManageFilterTemplateModel y)
        {
            // Two items are equal if their search word are equal
            //return x.SearchWord == y.SearchWord;
            return x.Filter.FLTRID == y.Filter.FLTRID;
        }
        public int GetHashCode(ManageFilterTemplateModel obj)
        {
            return obj.Filter.FLTRID.GetHashCode();
        }
    }
    public class NoDuplicateTemplate : IEqualityComparer<ManageFilterTemplateModel>
    {
        public bool Equals(ManageFilterTemplateModel x, ManageFilterTemplateModel y)
        {
            // Two items are equal if their search word are equal
            //return x.SearchWord == y.SearchWord;
            return x.Template.TEMPID == y.Template.TEMPID;
        }
        public int GetHashCode(ManageFilterTemplateModel obj)
        {
            return obj.Template.TEMPID.GetHashCode();
        }
    }
}