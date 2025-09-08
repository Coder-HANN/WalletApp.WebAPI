using MediatR;
using WalletApp.Application.DTOs.Action;
using WalletApp.Application.Feature.Wallet.Dtos;

namespace WalletApp.Application.Feature.Action.Command
{
    public class ActionCommand : IRequest<ServiceResponse<ActionResponseDTO>>
    {
        public string Remark { get; set; }
        public bool IsTransfer { get; set; }
        public decimal Amount { get; set; }
    }
}
