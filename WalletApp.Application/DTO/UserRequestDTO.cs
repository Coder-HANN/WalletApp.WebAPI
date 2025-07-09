using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.DTO
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "Bu kısım boş bırakılamaz.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Bu kısım boş bırakılamaz.")]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Bu kısım boş bırakılamaz.")]
        public string Name { get; set; } = null!;

    }
}
