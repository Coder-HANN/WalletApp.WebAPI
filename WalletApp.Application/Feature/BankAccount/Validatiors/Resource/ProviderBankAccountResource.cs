using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Validatiors.Resource
{
    public class ProviderBankAccountResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string InvaliedIban = "Geçersiz iban formatı";
        public const string BankAccountIsAvailable = "Ibana ait banka hesabı zaten mevcut";
        public const string SuccessMessage = "Provider banka hesabı başarıyla eklendi";
    }
}
