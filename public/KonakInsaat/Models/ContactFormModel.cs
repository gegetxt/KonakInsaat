using System.ComponentModel.DataAnnotations;

namespace KonakInsaat.Models
{
    public class ContactFormModel
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen bu alanı doldurun.")]
        public string NameSurname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bu alanı doldurun.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bu alanı doldurun.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bu alanı doldurun.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bu alanı doldurun.")]
        [StringLength(1000, ErrorMessage = "Mesaj en fazla 1000 karakter olabilir.")]
        public string Message { get; set; } = string.Empty;
        public bool IsImportant { get; set; } = false;
        public bool IsDraft { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsSent { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
