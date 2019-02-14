using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.ViewModels
{
    public class UpdateUserRoleViewModel
    {
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
