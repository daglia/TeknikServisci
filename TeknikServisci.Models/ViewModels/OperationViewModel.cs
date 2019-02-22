using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.Models.Enums;

namespace TeknikServisci.Models.ViewModels
{
    public class OperationViewModel
    {
        public string Message { get; set; }
        public IdentityRoles FromWhom { get; set; }
        public DateTime CreatedDate { get; set; }

        public int FailureId { get; set; }
    }
}
