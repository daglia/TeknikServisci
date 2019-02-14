using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.Models.Abstracts;
using TeknikServisci.Models.IdentityModels;

namespace TeknikServisci.Models.Entities
{
    public class Invoice : BaseEntity2<int,int>
    {
        [DisplayName("Fiyat")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public bool HasWarranty { get; set; }

        public string ClientId { get; set; }
        public int FailureId { get; set; }

        [ForeignKey("ClientId")]
        public virtual User Client { get; set; }
        [ForeignKey("FailureId")]
        public virtual Failure Failure { get; set; }
    }
}
