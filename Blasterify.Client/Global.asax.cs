using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Blasterify.Client
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //public static readonly string ServicesPath = "http://www.blasterify.services.com:9000/api";
        public static readonly string ServicesPath = "https://localhost:7276/api";
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
