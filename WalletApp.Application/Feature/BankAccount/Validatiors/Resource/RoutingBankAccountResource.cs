using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.BankAccount.Validatiors.Resource
{
    public class RoutingBankAccountResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string SourceBankAccountIsNotFound = "Kaynak provider banka hesabı bulunamadı";
        public const string RoutingAvailable = "Tüm bankalar için yönlendirme mevcut";
        public const string RoutingAvailableForThisBank = "Bu banka için zaten yönlendirme mevcut";
        public const string SuccessMessage = "Banka yönlendirme başarıyla yapıldı";
    }
}
