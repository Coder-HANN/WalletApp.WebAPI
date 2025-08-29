using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Validatiors.Resource
{
    public class BankDepositResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string TargetBankNotFound = "Hedef banka bulunamadı";
        public const string AmountMustBeGreaterThanZero = "Miktar sıfırdan büyük olmalı";
        public const string SuccessMessage = "Para yatırma işlemi başarılı";
    }
}
