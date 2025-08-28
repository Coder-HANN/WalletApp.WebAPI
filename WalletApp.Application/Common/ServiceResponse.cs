
using Braintree;
using WalletApp.Infrastructure.Services.Pagenation;

namespace WalletApp.Application.Feature.Wallet.Dtos
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public static ServiceResponse<T> Ok(T data, string? message = null) => new() { Success = true, Data = data, Message = message };
        public static ServiceResponse<T> Fail(string message) => new() { Success = false, Message = message };

        public static ServiceResponse<T> Ok (T data)
        {
            return new() { Success = true, Data = data };
        }
    }
}
