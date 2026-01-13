using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Mail
    {
        [Key]
        public int MailID { get; set; }

        [Required(ErrorMessage = "SMTP Sunucusu zorunludur")]
        [Display(Name = "SMTP Sunucusu")]
        [StringLength(200)]
        public string SmtpServer { get; set; } = string.Empty;

        [Required(ErrorMessage = "SMTP Port zorunludur")]
        [Display(Name = "SMTP Port")]
        public int SmtpPort { get; set; } = 587;

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [Display(Name = "E-posta Adresi")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [Display(Name = "Şifre")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Gönderen Adı")]
        [StringLength(200)]
        public string? GonderenAdi { get; set; }

        [Display(Name = "SSL Kullan")]
        public bool SslKullan { get; set; } = true;

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}


























