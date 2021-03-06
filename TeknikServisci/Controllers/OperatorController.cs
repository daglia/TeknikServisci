﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Repository;
using TeknikServisci.BLL.Services.Senders;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.Enums;
using TeknikServisci.Models.Models;
using TeknikServisci.Models.ViewModels;
using static TeknikServisci.BLL.Identity.MembershipTools;

namespace TeknikServisci.Controllers
{
    public class OperatorController : Controller
    {
        List<SelectListItem> Technicians = new List<SelectListItem>();

        // GET: Operator
        public ActionResult Index()
        {
            var data = new FailureRepo()
                .GetAll()
                .Select(x => Mapper.Map<FailureViewModel>(x))
                .ToList();

            return View(data);
        }

        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            try
            {
                var x = await new FailureRepo().GetByIdAsync(id);
                var data = Mapper.Map<FailureViewModel>(x);
                data.PhotoPath = new PhotoRepo().GetAll(y => y.FailureId == id).Select(y => y.Path).ToList();
                data.ClientId = x.ClientId;
                var client = await NewUserManager().FindByIdAsync(data.ClientId);
                data.ClientName = client.Name;
                data.ClientSurname = client.Surname;
                var operations = new OperationRepo()
                    .GetAll()
                    .Where(y => y.FailureId == data.FailureId)
                    .OrderByDescending(y => y.CreatedDate)
                    .ToList();
                data.Operations.Clear();
                foreach (Operation operation in operations)
                {
                    data.Operations.Add(Mapper.Map<OperationViewModel>(operation));
                }

                var TechnicianRole = NewRoleManager().FindByName("Technician").Users.Select(y => y.UserId).ToList();
                for (int i = 0; i < TechnicianRole.Count; i++)
                {
                    var distance = 0.0;
                    string distanceString = "";
                    var technician = NewUserManager().FindById(TechnicianRole[i]);
                    if (technician.Latitude.HasValue && technician.Longitude.HasValue && data.Latitude.HasValue && data.Longitude.HasValue)
                    {
                        var failureCoordinate = new GeoCoordinate(data.Latitude.Value, data.Longitude.Value);
                        var technicianCoordinate = new GeoCoordinate(technician.Latitude.Value, technician.Longitude.Value);

                        distance = failureCoordinate.GetDistanceTo(technicianCoordinate)/1000;
                        distanceString = $"(~{Convert.ToInt32(distance)} km)";
                    }

                    if (technician.TechnicianStatus == TechnicianStatuses.Available)
                    {
                        Technicians.Add(new SelectListItem()
                        {
                            Text = technician.Name + " " + technician.Surname + " " + distanceString,
                            Value = technician.Id
                        });
                    }
                }

                ViewBag.TechnicianList = Technicians;

                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Detail",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<ActionResult> FailureAccept(int id)
        {
            var OperatorId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                var failure = await new FailureRepo().GetByIdAsync(id);
                if (failure == null)
                {
                    RedirectToAction("Index", "Operator");
                }
                else
                {
                    failure.OperationTime = DateTime.Now;
                    failure.OperatorId = OperatorId;
                    failure.OperationStatus = OperationStatuses.Pending;
                    new FailureRepo().Update(failure);
                    RedirectToAction("Index", "Operator");

                }

                return RedirectToAction("Index", "Operator");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "FailureAccept",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult FailureList()
        {
            try
            {
                var data = new FailureRepo()
                    .GetAll()
                    .Select(x => Mapper.Map<FailureViewModel>(x))
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
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult OperationList()
        {
            var operatorId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                var data = new FailureRepo()
                    .GetAll()
                    .Select(x => Mapper.Map<FailureViewModel>(x))
                    .Where(x=>x.OperatorId == operatorId)
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
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult> TechnicianAdd(FailureViewModel model)
        {
            try
            {
                var failure = new FailureRepo().GetById(model.FailureId);
                failure.TechnicianId = model.TechnicianId;
                failure.OperationTime = DateTime.Now;
                failure.OperatorId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                failure.OperationStatus = OperationStatuses.Accepted;
                new FailureRepo().Update(failure);
                var technician = await NewUserStore().FindByIdAsync(failure.TechnicianId); 
                TempData["Message"] =
                    $"{failure.Id} nolu arızaya {technician.Name}  {technician.Surname} atanmıştır.İyi çalışmalar.";

                new OperationRepo().Insert(new Operation()
                {
                    FailureId = model.FailureId,
                    Message = $"Arızaya yeni teknisyen atanmıştır: {technician.Name} {technician.Surname}",
                    FromWhom = IdentityRoles.Operator
                });

                var emailService = new EmailService();
                var body = $"Merhaba <b>{failure.Client.Name} {failure.Client.Surname}</b><br>{failure.FailureName} adlı arızanız onaylanmış ve alanında uzman teknisyenlerimizden birine atanmıştır. Sizinle yeniden iletişime geçilecektir.<br><br>İyi günler dileriz.";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = $"{failure.FailureName} adlı arızanız onaylanmıştır. | Teknik Servisçi"
                }, failure.Client.Email);

                return RedirectToAction("Detail", "Operator",new {id = model.FailureId});
            }

            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "TechnicianAdd",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }

        }

        [HttpPost]
        [Authorize(Roles = "Admin, Operator")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Decline(FailureViewModel model)
        {
            try
            {
                var failure = new FailureRepo().GetById(model.FailureId);
                if (failure.OperationStatus == OperationStatuses.Declined)
                {
                    TempData["Message"] =
                        $"{failure.Id} nolu arıza zaten reddedilmiştir.";
                    return RedirectToAction("Detail", "Operator", new { id = model.FailureId });
                }
                failure.OperationStatus = OperationStatuses.Declined;
                failure.OperationTime = DateTime.Now;
                failure.OperatorId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                failure.Report = model.Report;
                new FailureRepo().Update(failure);
                new OperationRepo().Insert(new Operation()
                {
                    FailureId = failure.Id,
                    Message = $"Arızanız şu nedenden dolayı reddedilmiştir: {failure.Report}",
                    FromWhom = IdentityRoles.Operator
                });

                TempData["Message"] =
                    $"{failure.Id} nolu arıza reddedilmiştir.";

                var emailService = new EmailService();
                var body = $"Merhaba <b>{failure.Client.Name} {failure.Client.Surname}</b><br>{failure.FailureName} adlı arızanız şu nedenden dolayı reddedilmiştir:<br><br>{failure.Report}<br><br>İyi günler dileriz.";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = $"{failure.FailureName} adlı arızanız reddedilmiştir. | Teknik Servisçi"
                }, failure.Client.Email);

                return RedirectToAction("Detail", "Operator", new { id = model.FailureId });
            }

            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Decline",
                    ControllerName = "Operator",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        
        
        //[HttpGet]
        //public ActionResult TechnicianAdd(int id)
        //{
        //    try
        //    {
        //        var TechnicianRole = NewRoleManager().FindByName("Technician").Users.Select(x => x.UserId).ToList();
        //        for (int i = 0; i < TechnicianRole.Count; i++)
        //        {

        //            var User = NewUserManager().FindById(TechnicianRole[i]);
        //            Technicians.Add(new SelectListItem()
        //            {
        //                Text = User.Name + " " + User.Surname,
        //                Value = User.Id
        //            });
        //        }

        //        ViewBag.TechnicianList = Technicians;
        //        var data = new FailureRepo()
        //            .GetAll(x => x.Id == id)
        //            .Select(x => Mapper.Map<FailureViewModel>(x)).FirstOrDefault();
        //        return View("Detail",data);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //        throw;
        //    }
        //}
    }
}

