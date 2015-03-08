using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevMentor.PerformanceMonitoring
{
    public class PerfMonitorActionFilterAttribute : ActionFilterAttribute
    {
        PerfMonitor monitor;
        public PerfMonitorActionFilterAttribute()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            monitor = new PerfMonitor("PerfLogActionFilter");
            monitor.Line.Url = "/" + filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                monitor.Line.Message = filterContext.Exception.Message;
                monitor.Line.Status = 520;
            }
            monitor.Dispose();
            base.OnActionExecuted(filterContext);
        }
    }
}
