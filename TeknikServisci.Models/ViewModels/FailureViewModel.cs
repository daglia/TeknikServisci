using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeknikServisci.Models.Entities;
using TeknikServisci.Models.Enums;
using TeknikServisci.Models.IdentityModels;

namespace TeknikServisci.Models.ViewModels
{
    public class FailureViewModel
    {
        public int FailureId { get; set; }

        [StringLength(100, ErrorMessage = "Arıza adı en az 3, en fazla 100 karakter içerebilir.", MinimumLength = 3)]
        [DisplayName("Arıza Adı")]
        [Required]
        public string FailureName { get; set; }
        [StringLength(100, ErrorMessage = "Model adı en az 3, en fazla 100 karakter içerebilir.", MinimumLength = 3)]
        [DisplayName("Model Adı")]
        [Required]
        public string ProductModel { get; set; }
        [StringLength(300, ErrorMessage = "Açıklama en az 20, en fazla 300 karakter içerebilir.", MinimumLength = 20)]
        [DisplayName("Açıklama")]
        [Required]
        public string Description { get; set; }

        [DisplayName("İşlem Durumu")]
        public OperationStatuses OperationStatus { get; set; } = OperationStatuses.Pending;
        [DisplayName("İşlem Zamanı")]
        public DateTime? OperationTime { get; set; }

        [DisplayName("Süreç")]
        public RepairProcesses? RepairProcess { get; set; }

        [DisplayName("Oluşturulma Zamanı")]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [DisplayName("Başlama Zamanı")]
        public DateTime? StartingTime { get; set; }
        [DisplayName("Bitirme Zamanı")]
        public DateTime? FinishingTime { get; set; }

        [DisplayName("Adres")]
        [StringLength(100, ErrorMessage = "Adres alanı  en az 10, en fazla 100 karakter içerebilir.", MinimumLength = 10)]
        [Required]
        public string Address { get; set; }

        //public string Latitude { get; set; }
        //public string Longitude { get; set; }

        [DisplayName("Rapor")]
        public string Report { get; set; }
        public string PhotoPath { get; set; }

        public string ClientId { get; set; }
        public string TechnicianId { get; set; }
        public string OperatorId { get; set; }

        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string Technician { get; set; }
        public string Operator { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Kullanıcı Avatarı")]
        public HttpPostedFileBase PostedFile { get; set; }

    }
}
