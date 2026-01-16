using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Keyword
    {
        [Key]
        public int KeywordID { get; set; }

        [Required(ErrorMessage = "Sayfa adı zorunludur")]
        [Display(Name = "Sayfa Adı")]
        [StringLength(100)]
        public string SayfaAdi { get; set; } = string.Empty;

        [Display(Name = "Title (TR)")]
        [StringLength(200)]
        public string? Title { get; set; }

        [Display(Name = "Title (EN)")]
        [StringLength(200)]
        public string? Title_EN { get; set; }

        [Display(Name = "Title (RU)")]
        [StringLength(200)]
        public string? Title_RU { get; set; }

        [Display(Name = "Description (TR)")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Display(Name = "Description (EN)")]
        [StringLength(500)]
        public string? Description_EN { get; set; }

        [Display(Name = "Description (RU)")]
        [StringLength(500)]
        public string? Description_RU { get; set; }

        [Display(Name = "Keywords (TR)")]
        [StringLength(500)]
        public string? Keywords { get; set; }

        [Display(Name = "Keywords (EN)")]
        [StringLength(500)]
        public string? Keywords_EN { get; set; }

        [Display(Name = "Keywords (RU)")]
        [StringLength(500)]
        public string? Keywords_RU { get; set; }
    }
}


























