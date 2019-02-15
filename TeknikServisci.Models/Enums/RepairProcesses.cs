using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Enums
{
    public enum RepairProcesses
    {
        [Description("Başarısız")]
        Failed = 0,
        [Description("Başarılı")]
        Successful = 1,
    }
}
