using Harmonizer.UI.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Harmonizer.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           // filters.Add(new GeneralExceptionHandlerAttribute());
        }
    }
}
