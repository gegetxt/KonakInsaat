using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Proje
    {
        [Key]
        public int ProjeID { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur")]
        [Display(Name = "Proje Adı (TR)")]
        [StringLength(200)]
        public string ProjeAdi { get; set; } = string.Empty;

        [Display(Name = "Proje Adı (EN)")]
        [StringLength(200)]
        public string? ProjeAdi_EN { get; set; }

        [Display(Name = "Proje Adı (RU)")]
        [StringLength(200)]
        public string? ProjeAdi_RU { get; set; }

        [Display(Name = "Açıklama (TR)")]
        public string? Aciklama { get; set; }

        [Display(Name = "Açıklama (EN)")]
        public string? Aciklama_EN { get; set; }

        [Display(Name = "Açıklama (RU)")]
        public string? Aciklama_RU { get; set; }

        [Display(Name = "Kısa Açıklama (TR)")]
        [StringLength(500)]
        public string? KisaAciklama { get; set; }

        [Display(Name = "Kısa Açıklama (EN)")]
        [StringLength(500)]
        public string? KisaAciklama_EN { get; set; }

        [Display(Name = "Kısa Açıklama (RU)")]
        [StringLength(500)]
        public string? KisaAciklama_RU { get; set; }

        [Required(ErrorMessage = "Proje durumu zorunludur")]
        [Display(Name = "Proje Durumu")]
        [StringLength(50)]
        public string ProjeDurumu { get; set; } = "Devam Ediyor"; // Tamamlandı, Devam Ediyor, Yakında

        [Display(Name = "Ana Resim Yolu")]
        public string? AnaResimYolu { get; set; }

        [NotMapped]
        [Display(Name = "Ana Resim Seç")]
        public IFormFile? AnaResim { get; set; }

        [Display(Name = "Konum")]
        [StringLength(200)]
        public string? Konum { get; set; }

        [Display(Name = "Metrekare")]
        public string? Metrekare { get; set; }

        [Display(Name = "Oda Sayısı")]
        public string? OdaSayisi { get; set; }

        [Display(Name = "Teslim Tarihi")]
        public DateTime? TeslimTarihi { get; set; }

        [Display(Name = "Proje Başlangıç Tarihi")]
        public DateTime? ProjeBaslangicTarihi { get; set; }

        [Display(Name = "Proje Tamamlanma Tarihi")]
        public DateTime? ProjeTamamlanmaTarihi { get; set; }

        [Display(Name = "Build Year")]
        [StringLength(50)]
        public string? BuildYear { get; set; }

        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Display(Name = "Ortak Alanlar")]
        public string? OrtakAlanlar { get; set; }

        [Display(Name = "Daire Tipleri (A Blok)")]
        public string? DaireTipleriABlok { get; set; }

        [Display(Name = "Daire Tipleri (B Blok)")]
        public string? DaireTipleriBBlok { get; set; }

        [Display(Name = "Telefon 1")]
        [StringLength(50)]
        public string? Telefon1 { get; set; }

        [Display(Name = "Telefon 1 Diller")]
        [StringLength(100)]
        public string? Telefon1Diller { get; set; }

        [Display(Name = "Telefon 2")]
        [StringLength(50)]
        public string? Telefon2 { get; set; }

        [Display(Name = "Telefon 2 Diller")]
        [StringLength(100)]
        public string? Telefon2Diller { get; set; }

        [Display(Name = "Google Maps URL")]
        [StringLength(500)]
        public string? GoogleMapsUrl { get; set; }

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;

        // Navigation Property
        public virtual ICollection<ProjeResim>? ProjeResimleri { get; set; }
    }
}


















