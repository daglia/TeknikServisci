﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Repository;
using TeknikServisci.Models.Enums;
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
        public ActionResult FailureList()
        {
            var techId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                var data = new FailureRepo()
                    .GetAll()
                    .Select(x => Mapper.Map<FailureViewModel>(x))
                    .Where(x=>x.TechnicianId == techId)
                    .OrderBy(x => x.OperationTime)
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
        [HttpGet]
        public async Task<ActionResult> TeknisyenArızaBildiriOnayla(FailureViewModel model)
        {
            try
            {
                var failure = await new FailureRepo().GetByIdAsync(model.FailureId);
                if (model.TechnicianStatus== null)
                {
                    return RedirectToAction("", "Technician", model);
                }
                failure.Report = model.Report;
                failure.Technician.TechnicianStatus = model.TechnicianStatus;
                if (model.RepairProcess == RepairProcesses.Successful)
                {
                    failure.FinishingTime = DateTime.Now;
                }
                else if(model.RepairProcess == RepairProcesses.Failed)
                    failure.FinishingTime = DateTime.Now;
             

                new FailureRepo().Update(failure);
                TempData["Message"] = $"{model.FailureId} no lu Kayıt Raporu Alınmıştır. İyi çalışamlar";
                return RedirectToAction("Index", "Technician");

            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Teknisyen",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }

        }

    }
}