using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServisci.BLL.Identity;
using TeknikServisci.BLL.Repository;

namespace TeknikServisci.Controllers
{
    public class BaseController : Controller
    {
      

        protected List<SelectListItem> GetUserSelectList()
        {
            var data = new List<SelectListItem>();
            MembershipTools.NewUserStore().Users
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
            MembershipTools.NewRoleStore().Roles
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
    }
}