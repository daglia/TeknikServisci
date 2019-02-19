using System;
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
using static TeknikServisci.BLL.Identity.MembershipTools;

namespace TeknikServisci.Controllers
{
    public class OperatorController : Controller
    {
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
                    ActionName = "Index",
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult FailureList()
        {
            //operator bulunuyor ve o operatorun aldıgı kayıtlar listelenip çekiliyor.
            var operatorId = HttpContext.User.Identity.GetUserId();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Technician(FailureViewModel model)
        {
            //TODO NOT Eger bir yerlerde program view modelden patlarsa string birşeyler EKledik ondandır.
            try
            {
                //TODO Teknisyen atandıgı tarihde eklenebilir istenirse.
                var failure = new FailureRepo().GetById(model.FailureName);
                failure.TechnicianId = model.TechnicianId;
                failure.OperationStatus = OperationStatuses.Accepted;
                new FailureRepo().Update(failure);
                var technician = await NewUserStore().FindByIdAsync(failure.TechnicianId);
                //TODO Musteriye ve Teknisyene mail gönder. 
                TempData["Message"] =
                    $"{failure.Id} nolu arızaya {technician.Name}  {technician.Surname} atanmıştır.İyi çalışmalar.";

                return RedirectToAction("Index", "Operator");
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

        List<SelectListItem> Technicians = new List<SelectListItem>();
        public ActionResult OperationFailureDetail(int id)
        {

            try
            {

                var RoleTechnician = NewRoleManager().FindByName("Technician").Users.Select(x => x.UserId).ToList();
                for (int i = 0; i < RoleTechnician.Count; i++)
                {

                    var User = NewUserManager().FindById(RoleTechnician[i]);
                    Technicians.Add(new SelectListItem()
                    {
                        
                        Text = User.Name + " " + User.Surname,
                        Value = User.Id
                    });
                }

                ViewBag.TechnicianS = Technicians;
                var data = new FailureRepo()
                    .GetAll(x => x.Id == id)
                    .Select(x => Mapper.Map<FailureViewModel>(x)).FirstOrDefault();
                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}

