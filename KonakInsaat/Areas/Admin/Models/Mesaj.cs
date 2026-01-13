using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Mesaj
    {
        [Key]
        public int MesajID { get; set; }

        [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
        [Display(Name = "Ad Soyad")]
        [StringLength(200)]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [Display(Name = "E-posta")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        [StringLength(50)]
        public string? Telefon { get; set; }

        [Required(ErrorMessage = "Konu alanı zorunludur")]
        [Display(Name = "Konu")]
        [StringLength(200)]
        public string Konu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj alanı zorunludur")]
        [Display(Name = "Mesaj")]
        public string MesajIcerigi { get; set; } = string.Empty;

        [Display(Name = "Gönderim Tarihi")]
        public DateTime GonderimTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Okundu")]
        public bool Okundu { get; set; } = false;

        [Display(Name = "Yanıtlandı")]
        public bool Yanitlandi { get; set; } = false;

        [Display(Name = "IP Adresi")]
        [StringLength(50)]
        public string? IpAdresi { get; set; }
    }
}


























