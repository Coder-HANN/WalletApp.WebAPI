using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Validatiors.Resource
{
    internal class DepositToProviderBankResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string InvaliedIban = "Geçersiz iban formatı";
        public const string BankAccountIsNotFound = "Ibana ait banka hesabı bulunamadı";
        public const string AmountMustBeGreaterThanZero = "Miktar sıfırdan büyük olmalı";
        public const string SuccessMessage = "Sağlayıcı bankaya para yatırma işlemi başarılı";

    }
}
