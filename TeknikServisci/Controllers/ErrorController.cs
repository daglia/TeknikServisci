using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServisci.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            Response.Status = "Aradığınız sayfa bulunamadı";
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult H500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}