using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Models
{
    public class PageContent
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Sayfa adı zorunludur")]
        [StringLength(200, ErrorMessage = "Sayfa adı en fazla 200 karakter olabilir")]
        public string PageName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Alan adı zorunludur")]
        [StringLength(100, ErrorMessage = "Alan adı en fazla 100 karakter olabilir")]
        public string FieldName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "İçerik zorunludur")]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Language { get; set; } = "tr-TR";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}

