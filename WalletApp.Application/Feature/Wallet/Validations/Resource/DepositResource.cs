using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Wallet.Validations.Resource
{
    public class DepositResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string BankAccountNotFound = "Banka hesabı bulunamadı";
        public const string AmountMustBeGreaterThanZero = "Tutar sıfırdan büyük olmalıdır";
        public const string InsufficientBalance = "Banka hesabında yeterli bakiye yok";
        public const string WalletNotFound = "Cüzdan bulunamadı";
        public const string InavliedIban = "Geçersiz iban formatı";
        public const string ProviderBankAccountNotFound = "Sağlayıcı banka hesabı bulunamadı";
        public const string SuccessMessage = "Para yatırma işlemi başarılı";
    }
}
