
using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Katalog
    {
        public int KatalogID { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir.")]
        public string Baslik { get; set; } = string.Empty;

        [Display(Name = "PDF Link")]
        public string? Link { get; set; }
    }
}