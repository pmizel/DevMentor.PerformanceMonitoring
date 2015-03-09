# DevMentor.PerformanceMonitoring
ASP.NET MVC Performance Monitoring Helpers


```cs
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
```

Output in c:\temp\perflog[dt].log

```txt
id dt GET / -532462766 666ms MyUsing - TimeoutException Timeout für den Vorgang wurde überschritten.
id dt GET /Home/Index 0 768ms PerfLogActionFilter - 
id dt METHOD HttpContext==null 0 1002ms CreateIndexViewModel - 
```
