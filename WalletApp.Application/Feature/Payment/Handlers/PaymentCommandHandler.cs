using MediatR;
using WalletApp.Application.Feature.Payment.DTO;
using WalletApp.Application.Feature.Wallet.Dtos;
using WalletApp.Application.Services.EntitiesRepositories;
using WalletApp.Domain.Entities;

namespace WalletApp.Application.Feature.Payment.Handler
{
    public class PaymentCommandHandler : IRequestHandler<PaymentRequestDTO, ServiceResponse<PaymentResponseDTO>>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        
        public PaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IWalletRepository walletRepository,
            ITransactionRepository transactionRepository)
        {
            _paymentRepository = paymentRepository;
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }


        public async Task<ServiceResponse<PaymentResponseDTO>> Handle(PaymentRequestDTO request, CancellationToken cancellationToken)
        {

            // ➤ Kontrol: Institution boş mu?
            if (string.IsNullOrWhiteSpace(request.Institution))
                return ServiceResponse<PaymentResponseDTO>.Fail("Kuruluş adı boş olamaz.");

            // ➤ Kontrol: Amount negatif mi?
            if (request.Amount <= 0)
                return ServiceResponse<PaymentResponseDTO>.Fail("Ödeme miktarı pozitif olmalıdır.");

            // ➤ Cüzdan kontrolü
            var wallet = await _walletRepository.GetAsync(w => w.Id == request.AppWalletId);
            if (wallet == null)
                return ServiceResponse<PaymentResponseDTO>.Fail("Cüzdan bulunamadı.");

            // ➤ Bakiye kontrolü
            if (wallet.TotalBalance < request.Amount)
            {
                return ServiceResponse<PaymentResponseDTO>.Fail("Yetersiz bakiye.");
            }
            // ➤ Transaction oluştur
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = wallet.Id,
                Amount = -request.Amount,
                Description = request.Description ?? $"{request.Institution} ödemesi",
                CreatedDate = DateTime.UtcNow
            };
            await _transactionRepository.AddAsync(transaction);
            

            // ➤ Payment oluştur
            var payment = new AppPayment
            {
                Id = Guid.NewGuid(),
                TransactionId = transaction.Id,
                Amount = request.Amount,
                Institution = request.Institution,
                CreatedDate = DateTime.UtcNow
            };
            await _paymentRepository.AddAsync(payment);
           

            // ➤ Bakiye güncelle
            wallet.TotalBalance -= request.Amount;
            await _walletRepository.UpdateAsync(wallet);
            await _walletRepository.SaveChangesAsync();

            // ➤ Response
            var response = new PaymentResponseDTO
            {
                AppPaymentId = payment.Id,
                Institution = request.Institution,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow
            };

            return ServiceResponse<PaymentResponseDTO>.Ok(response, "Ödeme başarıyla gerçekleşti");
        }
    }
}
