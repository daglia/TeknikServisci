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

        public ActionResult Detail(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var data = new FailureRepo().GetById(id);
            if (data == null) return RedirectToAction("Index");

            return View(data);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var data = new FailureViewModel()
            {
                FailureName = "test",
                Address = "adres"
            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FailureViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var data = Mapper.Map<FailureViewModel, Failure>(model);
                data.Client = null;
                data.Invoices = null;
                new FailureRepo().Insert(data);
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