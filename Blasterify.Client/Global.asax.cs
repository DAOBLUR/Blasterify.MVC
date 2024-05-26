using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Blasterify.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {

#if DEBUG
        public static readonly string ServicesPath = System.Configuration.ConfigurationManager.ConnectionStrings["Blasterify.Services.Debug"].ConnectionString;
#else
        public static readonly string ServicesPath = System.Configuration.ConfigurationManager.ConnectionStrings["Blasterify.Services.Release"].ConnectionString;
#endif

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}