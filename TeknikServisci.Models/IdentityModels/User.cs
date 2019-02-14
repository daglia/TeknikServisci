using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TeknikServisci.Models.Entities;

namespace TeknikServisci.Models.IdentityModels
{
    public class User : IdentityUser
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(60)]
        [Required]
        public string Surname { get; set; }

        public string ActivationCode { get; set; }
        public string AvatarPath { get; set; }

        public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual List<Failure> Failures { get; set; } = new List<Failure>();
    }
}
