using DevMentor.PerformanceMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TaskTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["PerfMonitor"] = new PerfMonitor("Global: " + Context.Request.RawUrl);
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var mon = HttpContext.Current.Items["PerfMonitor"] as PerfMonitor;
            mon.Dispose();
        }


        
    }
}
