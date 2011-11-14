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

        public ActionResult Index(string dateText)
        {
            dateText = dateText ?? DateTime.UtcNow.ToString("dd.MM.yyyy.");
            DateTime date = DateTime.ParseExact(dateText, "dd.MM.yyyy.", null);
            dataContext.ContextOptions.LazyLoadingEnabled = false;
            var model = dataContext.HourLogs.Include("Employee").Where(hl => hl.Date.Year == date.Year && hl.Date.Month == date.Month && hl.Date.Day == date.Day).ToList().Select(hl => hl.Employee).Union(dataContext.Employees).Where(emp => emp.IsActive != false).Distinct().OrderBy(e => e.FirstName);
            ViewBag.DateText = dateText;
            return View(model);
        }

        public ActionResult Monthly(int id, string dateText)
        {
            dateText = dateText ?? DateTime.UtcNow.ToString("MM.yyyy.");
            DateTime date = DateTime.ParseExact(dateText, "MM.yyyy.", null);
            dataContext.ContextOptions.LazyLoadingEnabled = false;
            var logs = dataContext.HourLogs.Where(hl => hl.Date.Year == date.Year && hl.Date.Month == date.Month && hl.EmployeeId == id).OrderBy(hl => hl.Date.Day).ToList();
            var model = new Dictionary<DateTime, List<HourLog>>();
            for (int i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                model.Add(new DateTime(date.Year, date.Month, i), logs.Where(hl => hl.Date.Day == i).ToList());
            }
            ViewBag.EmployeeId = id;
            ViewBag.DateText = dateText;
            var employee = dataContext.Employees.SingleOrDefault(e => e.Id == id);
            ViewBag.EmployeeName = string.Format("{0} {1}", employee.FirstName, employee.LastName);
            return View(model);
        }
    }
}
