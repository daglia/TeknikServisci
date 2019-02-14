using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServisci.Models.Abstracts;

namespace TeknikServisci.Models.Entities
{
    public class Category : BaseEntity<int>
    {
        [StringLength(100, ErrorMessage = "Kategori adı en az 3, en fazla 100 karakter içerebilir.", MinimumLength = 3)]
        [DisplayName("Kategori Adı")]
        [Required]
        public string CategoryName { get; set; }
        public virtual ICollection<Failure> Failures { get; set; } = new HashSet<Failure>();
    }
}
