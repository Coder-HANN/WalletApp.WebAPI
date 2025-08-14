using System.ComponentModel.DataAnnotations;
using MediatR;
using WalletApp.Application.DTOs.Auth;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Auth.Commands
{
    public class RegisterCommand : IRequest<ServiceResponse<RegisterResponseDTO>>
    {
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } 
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Gender { get; set; } = null!;  // TODO: Enum olarak tanımlanabilir
        public DateTime BirthDay { get; set; }
        public string Occupation { get; set; } = null!; // TODO: Enum olarak tanımlanabilir
        public string? Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}