using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Kurumsal
    {
        [Key]
        public int KurumsalID { get; set; }

        [Display(Name = "Hakkımızda Başlık (TR)")]
        [StringLength(200)]
        public string? HakkimizdaBaslik { get; set; }

        [Display(Name = "Hakkımızda Başlık (EN)")]
        [StringLength(200)]
        public string? HakkimizdaBaslik_EN { get; set; }

        [Display(Name = "Hakkımızda Başlık (RU)")]
        [StringLength(200)]
        public string? HakkimizdaBaslik_RU { get; set; }

        [Display(Name = "Hakkımızda İçerik (TR)")]
        public string? HakkimizdaIcerik { get; set; }

        [Display(Name = "Hakkımızda İçerik (EN)")]
        public string? HakkimizdaIcerik_EN { get; set; }

        [Display(Name = "Hakkımızda İçerik (RU)")]
        public string? HakkimizdaIcerik_RU { get; set; }

        [Display(Name = "Misyon (TR)")]
        public string? Misyon { get; set; }

        [Display(Name = "Misyon (EN)")]
        public string? Misyon_EN { get; set; }

        [Display(Name = "Misyon (RU)")]
        public string? Misyon_RU { get; set; }

        [Display(Name = "Vizyon (TR)")]
        public string? Vizyon { get; set; }

        [Display(Name = "Vizyon (EN)")]
        public string? Vizyon_EN { get; set; }

        [Display(Name = "Vizyon (RU)")]
        public string? Vizyon_RU { get; set; }

        [Display(Name = "Hakkımızda Resim")]
        public string? HakkimizdaResim { get; set; }

        [NotMapped]
        [Display(Name = "Resim Seç")]
        public IFormFile? ResimFile { get; set; }

        [Display(Name = "Video URL")]
        [StringLength(500)]
        public string? VideoUrl { get; set; }

        public DateTime GuncellemeTarihi { get; set; } = DateTime.Now;
    }
}


























