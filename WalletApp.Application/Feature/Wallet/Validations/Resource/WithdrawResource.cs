using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Wallet.Validations.Resource
{
    public class WithdrawResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string WalletIsNotFound = "Cüzdan bulunamadı";
        public const string WalletAmountIsNotEnough = "Cüzdan bakiyesi yetersiz";
        public const string BankAccountIsNotFound = "Banka hesabı bulunamadı";
        public const string InvaliedIban = "Geçersiz iban formatı";
        public const string ProviderBankAccountIsNotFound = "Uygun sağlayıcı banka hesabı bulunamadı";
        public const string ProviderBankAmountIsNotEnough = "Sağlayıcı banka hesabı bakiyesi yetersiz";
        public const string SuccessMessage = "Para çekme işlemi başarıyla gerçekleşti.";
    }
}
