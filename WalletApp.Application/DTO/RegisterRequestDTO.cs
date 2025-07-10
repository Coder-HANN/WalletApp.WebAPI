using WalletApp.Application.Abstraction.Constence;
using System.ComponentModel.DataAnnotations;

namespace WalletApp.Application.DTO
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = ValidationMessages.EmailRequired) ]
        [EmailAddress(ErrorMessage = ValidationMessages.EmailInvalid)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
        [MinLength(6, ErrorMessage = ValidationMessages.PasswordMinLength)]
        public string Password { get; set; } = null;

        [Required(ErrorMessage = ValidationMessages.NameRequired)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.BirthDayRequired)]
        [DataType(DataType.Date, ErrorMessage = ValidationMessages.BirthDayInvalid)]
        public DateTime BirthDay { get; set; } 

        [Required(ErrorMessage = ValidationMessages.OccupationRequired)]
        [StringLength(100, ErrorMessage = ValidationMessages.OccupationMaxLength)]
        public string Occupation { get; set; } = null!;

        [Required(ErrorMessage = ValidationMessages.PhoneNumberRequired)]
        [Phone(ErrorMessage = ValidationMessages.PhoneNumberInvalid)]
        public string PhoneNumber { get; set; } = null!;

    }
}
