using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Enums
{
    public enum TechnicianStatuses
    {
        [Description("Meşgul")]
        Busy = 1,
        [Description("Uygun")]
        Available = 0,
        [Description("Yolda")]
        OnWay = 2,
        [Description("İş Başında")]
        OnWork = 3
    }
}