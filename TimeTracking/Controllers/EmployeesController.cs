using System.Linq;
using System.Web.Mvc;
using TimeTracking.Models;
using System;
using System.Collections.Generic;

namespace TimeTracking.Controllers
{
    public class EmployeesController : Controller
    {
        TimeTrackingModelContainer dataContext = new TimeTrackingModelContainer();

        public ActionResult Add()
        {
            ViewBag.Title = "Dodaj zaposlenog";
            var model = dataContext.Employees.CreateObject();
            return View(model);
        }

        [HttpPost]
        public ActionResult Add(Employee model)
        {
            ViewBag.Title = "Dodaj zaposlenog";
            if (ModelState.IsValid)
            {
                dataContext.Employees.AddObject(model);
                dataContext.SaveChanges();
                return RedirectToAction("Day", "Employees");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Title = "Izmeni zaposlenog";
            return View("Add", dataContext.Employees.FirstOrDefault(e => e.Id == id));
        }

        [HttpPost]
        public ActionResult Edit(Employee edited)
        {
            ViewBag.Title = "Izmeni zaposlenog";
            if (ModelState.IsValid)
            {
                dataContext.Employees.Attach(edited);
                dataContext.ObjectStateManager.ChangeObjectState(edited, System.Data.EntityState.Modified);
                dataContext.SaveChanges();
                return RedirectToAction("Day", "Employees");
            }
            return View("Add", edited);
        }

        public ActionResult Delete(int id)
        {
            var model = dataContext.Employees.SingleOrDefault(e => e.Id == id);
            //dataContext.DeleteObject(model);
            model.IsActive = false;
            dataContext.SaveChanges();
            return RedirectToAction("Day", "Employees");
        }

        public ActionResult Month(int id, string dateText)
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

        public ActionResult Day(string dateText)
        {
            dateText = dateText ?? DateTime.UtcNow.ToString("dd.MM.yyyy.");
            DateTime date = DateTime.ParseExact(dateText, "dd.MM.yyyy.", null);
            dataContext.ContextOptions.LazyLoadingEnabled = false;
            var model = dataContext.HourLogs.Include("Employee").Where(hl => hl.Date.Year == date.Year && hl.Date.Month == date.Month && hl.Date.Day == date.Day).ToList().Select(hl => hl.Employee).Union(dataContext.Employees).Where(emp => emp.IsActive != false).Distinct().OrderBy(e => e.FirstName);
            ViewBag.DateText = dateText;
            return View(model);
        }
    }
}
