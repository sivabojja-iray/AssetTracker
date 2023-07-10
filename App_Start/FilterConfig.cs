using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
