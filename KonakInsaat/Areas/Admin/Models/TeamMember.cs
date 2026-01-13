using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Areas.Admin.Models
{
    public class TeamMember
    {
        [Key]
        public int TeamMemberID { get; set; }

        [Required, StringLength(150)]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;

        [StringLength(150)]
        [Display(Name = "Pozisyon")]
        public string? Pozisyon { get; set; }

        [Display(Name = "Fotoğraf Yolu")]
        public string? FotoYolu { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; } = 1;

        [Display(Name = "Aktif")]
        public bool Aktif { get; set; } = true;
    }
}














