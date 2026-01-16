
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Iletisim
    {
        [Key]
        public int IletisimID { get; set; }

        [Display(Name = "Başlık")]
        [StringLength(100)]
        public string? Baslik { get; set; }

        [Display(Name = "Adres 1")]
        [StringLength(500)]
        public string? Adres_1 { get; set; }

        [Display(Name = "Telefon")]
        [StringLength(50)]
        public string? Telefon { get; set; }

        [Display(Name = "GSM")]
        [StringLength(50)]
        public string? Gsm { get; set; }

        [Display(Name = "WhatsApp")]
        [StringLength(50)]
        public string? Whatsapp { get; set; }

        [Display(Name = "E-Mail")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(100)]
        public string? Mail { get; set; }

        [Display(Name = "Harita")]
        [StringLength(1000)]
        public string? Harita { get; set; }

        [Display(Name = "Resim")]
        [StringLength(250)]
        public string? Resim { get; set; }

        [NotMapped]
        [Display(Name = "Resim Dosyası")]
        public IFormFile? ResimFile { get; set; }
    }
}
