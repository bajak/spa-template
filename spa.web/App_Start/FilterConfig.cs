using System.Web.Mvc;

namespace spa.web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LastModifiedCacheAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}