using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [Display(Name = "Başlık (TR)")]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;

        [Display(Name = "Başlık (EN)")]
        [StringLength(200)]
        public string? Baslik_EN { get; set; }

        [Display(Name = "Başlık (RU)")]
        [StringLength(200)]
        public string? Baslik_RU { get; set; }

        [Required(ErrorMessage = "İçerik alanı zorunludur")]
        [Display(Name = "İçerik (TR)")]
        public string Icerik { get; set; } = string.Empty;

        [Display(Name = "İçerik (EN)")]
        public string? Icerik_EN { get; set; }

        [Display(Name = "İçerik (RU)")]
        public string? Icerik_RU { get; set; }

        [Display(Name = "Kısa Açıklama (TR)")]
        [StringLength(500)]
        public string? KisaAciklama { get; set; }

        [Display(Name = "Kısa Açıklama (EN)")]
        [StringLength(500)]
        public string? KisaAciklama_EN { get; set; }

        [Display(Name = "Kısa Açıklama (RU)")]
        [StringLength(500)]
        public string? KisaAciklama_RU { get; set; }

        [Display(Name = "Resim Yolu")]
        public string? ResimYolu { get; set; }

        [NotMapped]
        [Display(Name = "Resim Seç")]
        public IFormFile? Resim { get; set; }

        [Display(Name = "Yayın Tarihi")]
        public DateTime YayinTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}


























