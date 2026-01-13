using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class OfficeLocation
    {
        [Key]
        public int OfficeLocationID { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Ofis Adı")]
        public string Ad { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [StringLength(50)]
        [Display(Name = "Telefon")]
        public string? Telefon { get; set; }

        [StringLength(100)]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [StringLength(500)]
        [Display(Name = "Harita URL")]
        public string? HaritaUrl { get; set; }

        [Display(Name = "Resim Yolu")]
        public string? ResimYolu { get; set; }

        [StringLength(50)]
        [Display(Name = "Tip (Merkez / Satış vb.)")]
        public string? Tip { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; } = 1;

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}














