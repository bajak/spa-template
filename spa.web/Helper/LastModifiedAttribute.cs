using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace spa.web
{
    public class LastModifiedCacheAttribute : ActionFilterAttribute
    {
        static LastModifiedCacheAttribute() 
        {
        }

        public static DateTime LastModified = DateTime.Now;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HasModification(filterContext.RequestContext, LastModified))
                filterContext.Result = NotModified(filterContext.RequestContext, LastModified);

            HttpContext.Current.Response.Cache.SetLastModified(LastModified);
            base.OnActionExecuting(filterContext);
        }

        private static bool HasModification(RequestContext context, DateTime modificationDate)
        {
            var headerValue = context.HttpContext.Request.Headers["If-Modified-Since"];
            if (headerValue == null)
                return true;

            var modifiedSince = DateTime.Parse(headerValue).ToLocalTime();
            return modifiedSince < modificationDate;
        }

        private static ActionResult NotModified(RequestContext response, DateTime lastModificationDate)
        {
            response.HttpContext.Response.Cache.SetLastModified(lastModificationDate);
            return new HttpStatusCodeResult(304, "Page has not been modified");
        }
    }
}