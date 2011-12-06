using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        TimeTrackingModelContainer dataContext = new TimeTrackingModelContainer();

        public ActionResult Month(string dateText, string sort)
        {
            dateText = dateText ?? DateTime.UtcNow.ToString("MM.yyyy.");
            DateTime date = DateTime.ParseExact(dateText, "MM.yyyy.", null);
            dataContext.ContextOptions.LazyLoadingEnabled = false;
            var employees = dataContext.HourLogs.Include("Employee").Where(hl => hl.Date.Year == date.Year && hl.Date.Month == date.Month).ToList().Select(hl => hl.Employee).Union(dataContext.Employees).Where(e => e.IsActive != false).Distinct().ToList();
            var temp = new List<EmployeeTotalsViewModel>();
            foreach (var employee in employees)
            {
                decimal night = 0;
                decimal day = 0;
                foreach (var hourLog in employee.HourLogs)
                {
                    var start = hourLog.StartHour;
                    var end = hourLog.EndHour;
                    var total = end - start;
                    var currDayHours = end > 6 ? (end > 22 ? 22 : end) - (start < 6 ? 6 : start > 22 ? 22 : start) : 0;
                    var currNightHours = total - currDayHours;
                    day += currDayHours;
                    night += currNightHours;
                }
                temp.Add(new EmployeeTotalsViewModel() { Employee = employee, DayHours = day, NightHours = night });
            }
            var model = sort == "hours" ? temp.OrderByDescending(i => i.TotalHours) : temp.OrderBy(i => i.Employee.FirstName);
            ViewBag.DateText = dateText;
            return View(model.ToList());
        }
    }
}
