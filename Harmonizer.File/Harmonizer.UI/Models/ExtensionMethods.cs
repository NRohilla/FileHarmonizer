using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Harmonizer.UI.Models
{
    public static class ExtensionMethods
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        public static string Slice(this string source, int start, int end)
        {
            if (end < 0)
                end = source.Length + end;

            if (start > end)
                return string.Empty;

            int len = end - start;
            string tag = source.Substring(start, len);

            if (tag.Contains(" "))
                return string.Empty;

            return tag;
        }
    }
}