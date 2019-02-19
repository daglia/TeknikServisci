using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServisci.Models.ViewModels
{
    public class OperationViewModel
    {
        [DisplayName("Fiyat")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool HasWarranty { get; set; }

        public string ClientId { get; set; }
        public int FailureId { get; set; }
    }
}
