using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Sosyal
    {
        [Key]
        public int SosyalID { get; set; }

        [Required(ErrorMessage = "Platform adı zorunludur")]
        [Display(Name = "Platform Adı")]
        [StringLength(100)]
        public string PlatformAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Link zorunludur")]
        [Display(Name = "Link")]
        [StringLength(500)]
        public string Link { get; set; } = string.Empty;

        [Display(Name = "İkon Sınıfı")]
        [StringLength(100)]
        public string? IkonSinifi { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}


























