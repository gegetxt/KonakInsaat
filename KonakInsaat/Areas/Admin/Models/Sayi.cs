using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Sayi
    {
        [Key]
        public int SayiID { get; set; }

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

        [Required(ErrorMessage = "Sayı alanı zorunludur")]
        [Display(Name = "Sayı")]
        public int SayiDegeri { get; set; }

        [Display(Name = "İkon")]
        [StringLength(100)]
        public string? Ikon { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}


























