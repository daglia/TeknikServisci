using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServisci.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult DrawerPartial()
        {
            var data = new List<string>();
            return PartialView("Partial/_DrawerPartial", data);
        }

        public PartialViewResult HeaderPartial()
        {
            return PartialView("Partial/_HeaderPartial");
        }
    }
}