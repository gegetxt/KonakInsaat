using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Galeri
    {
        [Key]
        public int GaleriID { get; set; }

        [Display(Name = "Başlık (TR)")]
        [StringLength(200)]
        public string? Baslik { get; set; }

        [Display(Name = "Başlık (EN)")]
        [StringLength(200)]
        public string? Baslik_EN { get; set; }

        [Display(Name = "Başlık (RU)")]
        [StringLength(200)]
        public string? Baslik_RU { get; set; }

        [Display(Name = "Resim Yolu")]
        public string? ResimYolu { get; set; }

        [NotMapped]
        [Display(Name = "Resim Seç")]
        public IFormFile? Resim { get; set; }

        [Display(Name = "Kategori")]
        [StringLength(100)]
        public string? Kategori { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;

        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}


























