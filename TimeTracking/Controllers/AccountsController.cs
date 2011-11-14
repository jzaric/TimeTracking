using System.Web.Mvc;
using System.Web.Security;
using TimeTracking.Models;

namespace TimeTracking.Controllers
{
    public class AccountsController : Controller
    {
        //
        // GET: /Account/

        public ActionResult SignIn()
        {
            FormsAuthentication.SignOut();
            ViewData.Model = new SignInViewModel();
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid && model.UserName == "korisnik" && model.Password == "test")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return Redirect(Request.QueryString["ReturnUrl"] ?? "~");
            }
            return View(model);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn");
        }
    }
}
