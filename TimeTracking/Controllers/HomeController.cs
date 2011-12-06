using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        TimeTrackingModelContainer dataContext = new TimeTrackingModelContainer();

        public ActionResult Index()
        {
            return RedirectToAction("Day", "Employees");
        }
    }
}
