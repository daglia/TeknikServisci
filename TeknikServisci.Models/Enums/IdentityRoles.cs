using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Enums
{
    public enum IdentityRoles
    {
        [Description("Yönetici")]
        Admin,
        [Description("Müşteri")]
        User,
        [Description("Teknisyen")]
        Technician,
        [Description("Operatör")]
        Operator
    }
}
