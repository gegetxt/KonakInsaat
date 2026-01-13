using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonakInsaat.Areas.Admin.Models
{
    public class ProjeResim
    {
        [Key]
        public int ResimID { get; set; }

        [Required]
        public int ProjeID { get; set; }

        [Display(Name = "Resim Yolu")]
        public string ResimYolu { get; set; } = string.Empty;

        [Display(Name = "Başlık")]
        [StringLength(200)]
        public string? Baslik { get; set; }

        [Display(Name = "Sıra")]
        public int Sira { get; set; }

        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("ProjeID")]
        public virtual Proje? Proje { get; set; }
    }
}


























