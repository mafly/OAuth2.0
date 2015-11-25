using System.Web;
using System.Web.Mvc;

namespace Mafly.OAuth2._0.Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
