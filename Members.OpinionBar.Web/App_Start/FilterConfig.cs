using Members.OpinionBar.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace Members.OpinionBar.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoggerAttribute());
        }
    }
}
