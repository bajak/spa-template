using System.Web.Mvc;

namespace spa.web
{
    public class IndexController : Controller
    {
        public string Index()
        {
            if (HtmlSnapshot.IsCrawler(Request.UserAgent))
            {
                return HtmlSnapshot.GetSnapshot(Request.Url.ToString());
            }
            return ViewRenderer.RenderViewToString(ControllerContext, "~/Views/index.cshtml");
        }
    }
}
