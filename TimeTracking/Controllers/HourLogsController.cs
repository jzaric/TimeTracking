using System;
using System.Linq;
using System.Web.Mvc;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class HourLogsController : Controller
    {
        TimeTrackingModelContainer dataContext = new TimeTrackingModelContainer();

        public ActionResult Add(int EmployeeId, string DateString, byte? Shift)
        {
            DateTime date = DateTime.ParseExact(DateString, "dd.MM.yyyy.", null);
            var hourLog = Shift == 1 ? new HourLog() { Date = date, StartHour = 7.5M, EndHour = 13.5M, UpdatedAt = DateTime.UtcNow } : new HourLog() { Date = date, StartHour = 13M, EndHour = 19M, UpdatedAt = DateTime.UtcNow };
            var employee = dataContext.Employees.SingleOrDefault(e => e.Id == EmployeeId);
            employee.HourLogs.Add(hourLog);
            dataContext.SaveChanges();
            return PartialView("HourLogView", hourLog);
        }

        public ActionResult Remove(int HourLogId)
        {
            var entity = dataContext.HourLogs.SingleOrDefault(hl => hl.Id == HourLogId);
            dataContext.HourLogs.DeleteObject(entity);
            dataContext.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(int HourLogId, decimal StartHour, decimal EndHour)
        {
            var entity = dataContext.HourLogs.SingleOrDefault(hl => hl.Id == HourLogId);
            entity.StartHour = StartHour;
            entity.EndHour = EndHour;
            dataContext.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
