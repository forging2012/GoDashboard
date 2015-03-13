using System.Web.Optimization;
namespace GoDashboard.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /** 
             * Default behaviour is to only bundle if debug=false.
             * Only enable the below line to test bundling works in Dev
             **/
            BundleTable.EnableOptimizations = false;

            bundles.Add(new StyleBundle("~/content/bundles/css").Include("~/Styles/*.css"));
            bundles.Add(new ScriptBundle("~/content/bundles/js").Include("~/Scripts/*.js"));
        }

    }
}