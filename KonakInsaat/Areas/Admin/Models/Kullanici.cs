using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Kullanici
    {
        [Key]
        public int KullaniciID { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(100)]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [Display(Name = "Şifre")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string Sifre { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [Display(Name = "E-posta")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Ad Soyad")]
        [StringLength(200)]
        public string? AdSoyad { get; set; }

        [Display(Name = "Telefon")]
        [StringLength(50)]
        public string? Telefon { get; set; }

        [Display(Name = "Rol")]
        [StringLength(50)]
        public string Rol { get; set; } = "Admin";

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;

        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public DateTime? SonGirisTarihi { get; set; }
    }
}


























