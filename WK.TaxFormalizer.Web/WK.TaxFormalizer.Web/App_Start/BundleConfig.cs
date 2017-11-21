using System.Web;
using System.Web.Optimization;

namespace WK.TaxFormalizer.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                   "~/Scripts/sto.js",
                   "~/Scripts/DashBoard.js",
                   "~/Scripts/jquery.validate.js",
                   "~/Scripts/jquery.validate.unobtrusive.js",
                   "~/Scripts/CustomKendoUIScripts.js",
                   "~/Scripts/customStoScripts.js",
                   "~/Scripts/kendo.modernizr.custom.js",
                   "~/Scripts/jquery.unobtrusive-ajax.min.js"
                   ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Common.css",
                        "~/Content/kendo.custom.css"

                      ));

            //Bundling Home Page CSS - KD- 09-05-2014
            bundles.Add(new StyleBundle("~/Content/Default/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Default/Custom.css"

                      ));

            //Bundleing Login Page CSS - KD- 09-05-2014
            bundles.Add(new StyleBundle("~/Content/Login/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/Login/Custom.css"
                     ));

            /*Kendo UI configuration (CSS and Scripts Loading) - Kunal Deshmukh - 17th October 2014*/

            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/kendo/scripts").Include(
           "~/Scripts/kendo/2014.3.1316/kendo.all.min.js",
           "~/Scripts/kendo/2014.3.1316/kendo.aspnetmvc.min.js"
           ));

            //Styling Options
            bundles.Add(new StyleBundle("~/Content/kendoui").Include(
            "~/Content/kendo/2014.3.1316/kendo.common-bootstrap.core.min.css",
            "~/Content/kendo/2014.3.1316/kendo.common.min.css",
            "~/Content/kendo/2014.3.1316/kendo.mobile.all.min.css",
            "~/Content/kendo/2014.3.1316/kendo.dataviz.min.css",
            "~/Content/kendo/2014.3.1316/kendo.dataviz.default.min.css",
            "~/Content/kendo/2014.3.1316/kendo.default.min.css",
            "~/Content/kendo.custom.css"


            ));
        }
    }
}
