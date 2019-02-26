using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Models
{
    public class ReportData
    {
    }
    public class DailyReport
    {
        public int completed { get; set; }
        public bool success { get; set; }
    }
    public class DailyProfitReport
    {
        public decimal completed { get; set; }
        public bool success { get; set; }
    }

    public class SurveyReport
    {
        public string question { get; set; }
        public double point { get; set; }
    }

    public class TechReport
    {
        public string nameSurname { get; set; }
        public double point { get; set; }
    }
}
