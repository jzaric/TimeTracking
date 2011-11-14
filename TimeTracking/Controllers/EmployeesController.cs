using System.Linq;
using System.Web.Mvc;
using TimeTracking.Models;

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
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }
            return View("Add", edited);
        }

        public ActionResult Delete(int id)
        {
            var model = dataContext.Employees.SingleOrDefault(e => e.Id == id);
            //dataContext.DeleteObject(model);
            model.IsActive = false;
            dataContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
