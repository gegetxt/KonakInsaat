using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class Marka
    {
        [Key]
        public int MarkaID { get; set; }

        [Display(Name = "Marka Adı")]
        [StringLength(200)]
        public string? MarkaAdi { get; set; }

        [Display(Name = "Resim Yolu")]
        public string? ResimYolu { get; set; }

        [NotMapped]
        [Display(Name = "Resim Seç")]
        public IFormFile? Resim { get; set; }

        [Display(Name = "Link")]
        [StringLength(500)]
        public string? Link { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;

        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;
    }
}


























