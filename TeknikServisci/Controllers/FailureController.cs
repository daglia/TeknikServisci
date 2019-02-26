using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Repository;
using TeknikServisci.BLL.Services.Senders;
using static TeknikServisci.BLL.Identity.MembershipTools;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.Enums;
using TeknikServisci.Models.ViewModels;

namespace TeknikServisci.Controllers
{
    public class FailureController : BaseController
    {
        // GET: Failure
        public ActionResult Index()
        {
            var clientId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                var data = new FailureRepo()
                    .GetAll()
                    .Select(x => Mapper.Map<FailureViewModel>(x))
                    .Where(x => x.ClientId == clientId)
                    .OrderBy(x => x.CreatedTime)
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
        public async Task<ActionResult> Detail(int id)
        {
            try
            {
                var x = await new FailureRepo().GetByIdAsync(id);
                var data = Mapper.Map<FailureViewModel>(x);
                var operations = new OperationRepo()
                    .GetAll()
                    .Where(y=>y.FailureId == data.FailureId)
                    .OrderByDescending(y=>y.CreatedDate)
                    .ToList();
                data.Operations.Clear();
                foreach (Operation operation in operations)
                {
                    data.Operations.Add(Mapper.Map<OperationViewModel>(operation));
                }

                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Detail",
                    ControllerName = "Failure",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Invoice(int id)
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
                    ActionName = "Detail",
                    ControllerName = "Failure",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
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
                model.ClientId = System.Web.HttpContext.Current.User.Identity.GetUserId();

                var data = Mapper.Map<FailureViewModel, Failure>(model);

                var failureRepo = new FailureRepo();
                failureRepo.Insert(data);
                var photoRepo = new PhotoRepo();
                if (model.PostedPhoto.Count > 0)
                {
                    model.PostedPhoto.ForEach(file =>
                    {
                        if (file == null || file.ContentLength <= 0)
                            return;

                        string fileName = "failure-";
                        fileName += Path.GetFileNameWithoutExtension(file.FileName);
                        string extName = Path.GetExtension(file.FileName);
                        fileName = StringHelpers.UrlFormatConverter(fileName);
                        fileName += StringHelpers.GetCode();
                        var klasoryolu = Server.MapPath("~/Upload/Failure/");
                        var dosyayolu = Server.MapPath("~/Upload/Failure/") + fileName + extName;

                        if (!Directory.Exists(klasoryolu))
                            Directory.CreateDirectory(klasoryolu);
                        file.SaveAs(dosyayolu);

                        WebImage img = new WebImage(dosyayolu);
                        img.Resize(800, 600, false);
                        img.AddTextWatermark("Teknik Servisçi");
                        img.Save(dosyayolu);
                        photoRepo.Insert(new Photo()
                        {
                            FailureId = data.Id,
                            Path = "/Upload/Failure/" + fileName + extName
                        });
                    });
                }

                var photos = photoRepo.GetAll(x => x.FailureId == data.Id).ToList();
                var photo = photos.Select(x => x.Path).ToList();
                data.PhotoPath = photo;
                failureRepo.Update(data);

                new OperationRepo().Insert(new Operation()
                {
                    FailureId = data.Id,
                    Message = $"#{data.Id} - {data.FailureName} adlı arıza kaydı oluşturuldu.",
                    FromWhom = IdentityRoles.User
                });
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
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Survey(string code)
        {
            try
            {
                var surveyRepo = new SurveyRepo();
                var survey = surveyRepo.GetById(code);
                if (survey == null)
                    return RedirectToAction("Index", "Home");
                var data = Mapper.Map<Survey, SurveyViewMdel>(survey);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Survey",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public ActionResult Survey(SurveyViewMdel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                return RedirectToAction("Survey", "Failure", model);
            }
            try
            {
                var surveyRepo = new SurveyRepo();
                var survey = surveyRepo.GetById(model.SurveyId);
                if (survey == null)
                    return RedirectToAction("Index", "Home");
                survey.Pricing = model.Pricing;
                survey.Satisfaction = model.Satisfaction;
                survey.Solving = model.Solving;
                survey.Speed = model.Speed;
                survey.TechPoint = model.TechPoint;
                survey.Suggestions = model.Suggestions;
                survey.IsDone = true;
                surveyRepo.Update(survey);
                TempData["Message2"] = "Anket tamamlandı.";
                return RedirectToAction("UserProfile", "Account");
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Survey",
                    ControllerName = "Issue",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Operator")]
        public ActionResult GetAllFailures()
        {
            var data = new FailureRepo().GetAll(x => x.FinishingTime != null);
            if (data == null)
                return RedirectToAction("Index", "Home");
            return View(data);
        }
    }
}