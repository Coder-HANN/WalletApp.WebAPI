using System.ComponentModel.DataAnnotations;
using WalletApp.Application.Abstraction.Constence;

namespace WalletApp.Application.DTO
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = ValidationMessages.HereRequired)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = ValidationMessages.HereRequired)]
        public string Password { get; set; } = null!;
    }
}
