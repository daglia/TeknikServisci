using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.Models.Abstracts;

namespace TeknikServisci.Models.Entities
{
    public class Photo : BaseEntity<string>
    {
        public Photo()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [DisplayName("Yol")]
        public string Path { get; set; }

        public int FailureId { get; set; }

        [ForeignKey("FailureId")]
        public virtual Failure Failure { get; set; }
    }
}
