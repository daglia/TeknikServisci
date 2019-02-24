using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServisci.BLL.Identity;
using TeknikServisci.BLL.Repository;

namespace TeknikServisci.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Logout", "Account");

            if (!User.IsInRole("User"))
            {
                ViewBag.pendingFailureCount = new FailureRepo().GetAll().Where(x => x.OperationStatus == Models.Enums.OperationStatuses.Pending).Count();

                ViewBag.availableTechnicianCount = MembershipTools.NewUserStore().Users.Where(x => x.TechnicianStatus == Models.Enums.TechnicianStatuses.Available).Count();
            }
            

            return View();
        }
        public ActionResult Error()
        {

            return View();
        }
    }
}