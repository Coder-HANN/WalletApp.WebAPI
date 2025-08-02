using System.ComponentModel.DataAnnotations;
using MediatR;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.User.Dtos
{
    public class RegisterRequestDTO : IRequest<ServiceResponse<RegisterResponseDTO>>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateTime BirthDay { get; set; } // kontrol olmalı 18 yaş için 
        public string Occupation { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? VerificationCode { get; set; }
    }
}
