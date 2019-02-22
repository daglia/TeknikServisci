using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using TeknikServisci.DAL;
using TeknikServisci.Models.Enums;
using TeknikServisci.Models.IdentityModels;

namespace TeknikServisci.BLL.Identity
{
    public static class MembershipTools
    {
        private static MyContext _db;

        public static UserStore<User> NewUserStore() => new UserStore<User>(_db ?? new MyContext());
        public static UserManager<User> NewUserManager() => new UserManager<User>(NewUserStore());

        public static RoleStore<Role> NewRoleStore() => new RoleStore<Role>(_db ?? new MyContext());
        public static RoleManager<Role> NewRoleManager() => new RoleManager<Role>(NewRoleStore());


        public static string GetNameSurname(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";

                user = NewUserManager().FindById(id);
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;
            }

            return $"{user.Name} {user.Surname}";
        }
        public static string GetAvatarPath(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "/assets/img/user.png";

                user = NewUserManager().FindById(id);
                if (user.AvatarPath == null)
                    return "/assets/img/user.png";
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user.AvatarPath == null)
                    return "/assets/img/user.png";
            }

            return $"{user.AvatarPath}";
        }

        public static string GetRole(string userId)
        {
            User user;
            string rolename = "";
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;
                rolename = NewUserManager().GetRoles(user.Id).FirstOrDefault();
            }

            return rolename;
        }

        public static TechnicianStatuses? GetTechnicianStatus(string userId)
        {
            User user;
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return null;

                user = NewUserManager().FindById(id);
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;
            }

            return user.TechnicianStatus;
        }

        public static string GetRoleWithColor(string userId)
        {
            User user;
            string span = "";
            if (string.IsNullOrEmpty(userId))
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (string.IsNullOrEmpty(id))
                    return "";
            }
            else
            {
                user = NewUserManager().FindById(userId);
                if (user == null)
                    return null;

                var rolename = NewUserManager().GetRoles(user.Id).FirstOrDefault();

                switch (rolename)
                {
                    case "Admin":
                        span = "label-purple";
                        break;
                    case "Technician":
                        span = "label-warning";
                        break;
                    case "Operator":
                        span = "label-success";
                        break;
                    default:
                        span = "label-primary";
                        break;
                }
            }

            return span;
        }
    }
}
