using Harmonizer.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public class ItemEqualityComparer:IEqualityComparer<SwordAndTagReplace>
    {
        public bool Equals(SwordAndTagReplace x, SwordAndTagReplace y)
        {
            // Two items are equal if their search word are equal
            //return x.SearchWord == y.SearchWord;
            return x.TagName == y.TagName;
        }
        public int GetHashCode(SwordAndTagReplace obj)
        {
            return obj.TagName.GetHashCode();
        }
    }
}