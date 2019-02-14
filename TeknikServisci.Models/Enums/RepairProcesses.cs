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
        [Description("Bekleniyor...")]
        Pending = 101,
        [Description("Yolda")]
        OnWay = 102,
        [Description("İş Başında")]
        OnWork = 103
    }
}
