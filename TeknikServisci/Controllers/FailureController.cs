using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TeknikServisci.BLL.Repository;
using TeknikServisci.BLL.Services.Senders;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.ViewModels;

namespace TeknikServisci.Controllers
{
    public class FailureController : Controller
    {
        // GET: Failure
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FailureViewModel model)
        {
            try
            {
                new FailureRepo().Insert(Mapper.Map<FailureViewModel, Failure>(model));
                TempData["Message"] = $"{model.FailureName} adlı arızanız operatörlerimizce incelenecektir ve size 24 saat içinde dönüş yapılacaktır.";
                return RedirectToAction("Add");
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Failure",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu: {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Failure",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}