using System.Web.Mvc;
using System.Web.Routing;
using Spark;
using Spark.Web.Mvc;

namespace GoDashboard.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("BlueStatus", "BlueStatus/{branchname}", new { controller = "BlueStatus", action = "Index" });
            routes.MapRoute("RedStatus", "RedStatus/{branchname}", new { controller = "BlueStatus", action = "Remove" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            RegisterViewEngine(ViewEngines.Engines);
        }

        public static void RegisterViewEngine(ViewEngineCollection engines)
        {
            var settings = new SparkSettings();

            settings
                .AddNamespace("System")
                .AddNamespace("System.Collections.Generic")
                .AddNamespace("System.Linq")
                .AddNamespace("System.Web.Mvc")
                .AddNamespace("System.Web.Mvc.Html");

            settings
                .AddAssembly("Spark.Web.Mvc")
                .AddAssembly("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35")
                .AddAssembly("System.Web.Routing, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

            engines.Add(new SparkViewFactory(settings));
        }
    }
}