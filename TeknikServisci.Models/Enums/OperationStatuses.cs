using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Enums
{
    public enum OperationStatuses
    {
        [Description("Onaylanmadı")]
        Declined = 0,
        [Description("Onaylandı")]
        Accepted = 1,
        [Description("Onaylanması bekleniyor...")]
        Pending = 2
    }
}
