using Harmonizer.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.API.Utility
{
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