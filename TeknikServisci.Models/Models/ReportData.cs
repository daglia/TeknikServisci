using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.Models
{
   
    public class SurveyReport
    {
        public string question { get; set; }
        public double point { get; set; }
    }
    public class WeeklyReport
    {
        public string date { get; set; }
        public int count { get; set; }
    }

    public class TechReport
    {
        public string nameSurname { get; set; }
        public double point { get; set; }
    }
}
