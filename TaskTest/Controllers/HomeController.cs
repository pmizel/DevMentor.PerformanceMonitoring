using DevMentor.PerformanceMonitoring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TaskTest.Controllers
{
    [PerfMonitorActionFilterAttribute]
    public class HomeController : Controller
    {
        public class IndexViewModel
        {
            public DateTime Date { get; set; }
        }

        public async Task<ActionResult> Index()
        {
            IndexViewModel model = null;

            Session["date"] = DateTime.Now;
            using (var monitor = new PerfMonitor("MyUsing")) 
            {
                try
                {
                    var ctx = System.Web.HttpContext.Current;
                    var id = PerfMonitorContext.Current.Id;
                    var session = Session;
                    var task = Task.Run<IndexViewModel>(() =>
                    {
                        using (var mon = new PerfMonitor("CreateIndexViewModel"))
                        {
                            mon.Line.Id = id;
                            var result = new IndexViewModel();
                            result.Date = (DateTime)session["date"];
                            System.Threading.Thread.Sleep(1000);
                            return result;
                        }
                    });
                    await task.TimeoutAfter(500);
                    model = task.Result;
                }
                catch (TimeoutException ex)
                {
                    //Log.Warning("Timeout");
                    monitor.Line.Message = "TimeoutException "+ex.Message;
                }
            }
            return View(model);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            using (new PerfMonitor())
            {
                System.Threading.Thread.Sleep(100);
            }
            using (new PerfMonitor("Method_Sleep_100"))
            {
                System.Threading.Thread.Sleep(100);
            }
            using (new PerfMonitor("Method_Sleep_200"))
            {
                System.Threading.Thread.Sleep(200);
            }
            return View();
        }

        public ActionResult Error()
        {
            System.Threading.Thread.Sleep(300);
            throw new NotImplementedException();
         
        }


    }
}