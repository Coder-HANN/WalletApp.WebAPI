using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Payment.Validators.Resource
{
    public class PaymentResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string InstutionNameNotNull = "Kurum adı boş olamaz";
        public const string AmountMustBeGreaterThanZero = "Tutar 0' dan büyük olmalı";
        public const string WalletIsNotFound = "Cüzdan hesabı bulunamadı";
        public const string InsufficientBalance = "Yetersiz bakiye";
        public const string PaymentSuccess = "Ödeme başarıyla gerçekleştirildi";
    }
}
