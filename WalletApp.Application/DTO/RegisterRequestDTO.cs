using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Email boş olamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre boş olamaz.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; } = null;

        [Required(ErrorMessage = "İsim boş olamaz.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Doğum günü boş olamaz.")]
        [DataType(DataType.Date, ErrorMessage = "Geçerli bir tarih girin.")]
        public DateTime BirthDay { get; set; } 

        [Required(ErrorMessage = "Lütfen meslek seçimi yapınız.")]
        [StringLength(100, ErrorMessage = "Meslek en fazla 100 karakter olmalıdır.")]
        public string Occupation { get; set; } = null!;

        [Required(ErrorMessage = "Telefon numarası boş olamaz.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string PhoneNumber { get; set; } = null!;

    }
}
