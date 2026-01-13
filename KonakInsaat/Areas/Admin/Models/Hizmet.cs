


// Models/Hizmet.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KonakInsaat.Areas.Admin.Models
{
    public class Hizmet
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [Display(Name = "Başlık (TR)")]
        [StringLength(200)]
        public string Baslik { get; set; } = string.Empty;

        // HizmetAdi property'si Baslik'in kısa hali
        [NotMapped]
        public string HizmetAdi => Baslik;

        [Display(Name = "Başlık (EN)")]
        [StringLength(200)]
        public string? Baslik_EN { get; set; }

        [Display(Name = "Başlık (RU)")]
        [StringLength(200)]
        public string? Baslik_RU { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        [Display(Name = "Açıklama (TR)")]
        public string Aciklama { get; set; } = string.Empty;

        [Display(Name = "Açıklama (EN)")]
        public string? Aciklama_EN { get; set; }

        [Display(Name = "Açıklama (RU)")]
        public string? Aciklama_RU { get; set; }

        [Required(ErrorMessage = "Kısa açıklama alanı zorunludur")]
        [Display(Name = "Kısa Açıklama (TR)")]
        [StringLength(500)]
        public string KisaAciklama { get; set; } = string.Empty;

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

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}