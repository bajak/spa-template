using System.Web.Optimization;

namespace spa.web
{
    public class BundleConfig
    {
        // Weitere Informationen zu Bundling finden Sie unter "http://go.microsoft.com/fwlink/?LinkId=254725".
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new StyleBundle("~/Content/Styles/css")
                .Include("~/Content/Styles/style.css"));

            bundles.Add(new ScriptBundle("~/Scripts/App/bundle")
                .Include("~/Scripts/libs/angular-cookies.js")
                .Include("~/Scripts/libs/angular-sanitize.js")
                .Include("~/Scripts/libs/angular-animate.js")
                .Include("~/Scripts/libs/angular-route.js")
                .Include("~/Scripts/libs/angular-resource.js")
                .Include("~/Scripts/libs/angular-localize.js")
                .Include("~/Scripts/libs/angular-local-storage.js")

                .Include("~/Scripts/app/app.js")
                .Include("~/Scripts/app/config/router.js")
                .Include("~/Scripts/app/config/localize.js")
                .Include("~/Scripts/app/config/http.js")

                .Include("~/Scripts/app/provider/alerts.js")
                .Include("~/Scripts/app/provider/helper.js")

                .Include("~/Scripts/app/other/interceptor.js")

                .Include("~/Scripts/app/controller/index.js")
                .Include("~/Scripts/app/controller/start.js")
                .Include("~/Scripts/app/controller/about.js")
                .Include("~/Scripts/app/controller/user.js")

                .Include("~/Scripts/app/directives/compile.js")
                .Include("~/Scripts/app/directives/alert.js")
                .Include("~/Scripts/app/directives/complete.js")

                .Include("~/Scripts/app/services/user.js"));
        }
    }
}