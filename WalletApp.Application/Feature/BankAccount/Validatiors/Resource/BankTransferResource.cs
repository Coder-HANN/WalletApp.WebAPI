using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Validatiors.Resource
{
    public class BankTransferResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string WalletIsNotFound = "Cüzdan bulunamadı";
        public const string WalletAmountIsNotEnough = "Cüzdan bakiyesi yetersiz";
        public const string BankAccountIsNotFound = "Banka hesabı bulunamadı";
        public const string BankTypeIsNotFound = "Banka türü bulunamadı";
        public const string SourceBankAccountIsNotFound = "Sağlayıcı banka hesabı bulunamadı";
        public const string ProviderBankAccountIsNotFound = "Uygun sağlayıcı banka hesabı bulunamadı";
        public const string SuccessMessage = "Transfer işlemi başarıyla gerçekleşti.";
    }
}
