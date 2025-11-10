using Members.NewOpinionBar.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace Members.NewOpinionBar.Web
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
