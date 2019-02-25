using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TeknikServisci.BLL.Identity;
using TeknikServisci.BLL.Repository;
using TeknikServisci.Models.Enums;
using static TeknikServisci.BLL.Identity.MembershipTools;

namespace TeknikServisci.Controllers
{
    public class BaseController : Controller
    {
      

        protected List<SelectListItem> GetUserSelectList()
        {
            var data = new List<SelectListItem>();
            NewUserStore().Users
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name} {x.Surname}",
                        Value = x.Id
                    });
                });

            //var users = MembershipTools.NewUserStore().Users;
            //foreach (var user in users)
            //{
            //    data.Add(new SelectListItem()
            //    {
            //        Text = $"{user.Name} {user.Surname}",
            //        Value = user.Id
            //    });
            //}
            return data;
        }
        protected List<SelectListItem> GetRoleSelectList()
        {
            var data = new List<SelectListItem>();
            NewRoleStore().Roles
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name}",
                        Value = x.Id
                    });
                });
            return data;
        }
        protected List<SelectListItem> GetTechnicianList()
        {
            var data = new List<SelectListItem>();
            var userManager = NewUserManager();
            var users = userManager.Users.ToList();

            var techIds = new FailureRepo().GetAll(x => x.Technician.TechnicianStatus == TechnicianStatuses.OnWay || x.Technician.TechnicianStatus == TechnicianStatuses.OnWork).Select(x => x.TechnicianId).ToList();

            foreach (var user in users)
            {
                if (userManager.IsInRole(user.Id, IdentityRoles.Technician.ToString()))
                {
                    if (!techIds.Contains(user.Id))
                    {
                        var techPoint = GetTechPoint(user.Id);
                        data.Add(new SelectListItem()
                        {
                            Text = $"{user.Name} {user.Surname} ({techPoint})",
                            Value = user.Id
                        });
                    }
                }
            }
            return data;
        }
    }
}