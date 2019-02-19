using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Repository;
using TeknikServisci.Models.ViewModels;

namespace TeknikServisci.Controllers
{
    public class TechnicianController : Controller
    {
        // GET: Technician
        public ActionResult Index()
        {
            try
            {
                var technicianId = HttpContext.User.Identity.GetUserId();
                var data = new FailureRepo()
                    .GetAll(x => x.TechnicianId == technicianId).Select(x => Mapper.Map<FailureViewModel>(x))
                    .ToList();

                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Home",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}