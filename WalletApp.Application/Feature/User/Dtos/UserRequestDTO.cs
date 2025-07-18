using System.ComponentModel.DataAnnotations;
using WalletApp.Application.Feature.Constence;

namespace WalletApp.Application.Feature.User.Dtos
{
    public class UserRequestCommand
    {
        [Required(ErrorMessage = ValidationMessages.HereRequired)]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = ValidationMessages.HereRequired)]
        public string Password { get; set; } = null!;
    }
}
