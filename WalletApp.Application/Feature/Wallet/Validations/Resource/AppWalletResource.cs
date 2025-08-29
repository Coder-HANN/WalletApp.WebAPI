using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Wallet.Validations.Resource
{
    public class AppWalletResource
    {
        public const string UserIsNotFound = "Kullanıcı bulunamadı";
        public const string WalletNameIsNotNull = "Cüzdan adı boş olamaz";
        public const string FailedMessage = "Cüzdan oluşturulamadı";
        public const string SuccessMessage = "Cüzdan başarıyla oluşturuldu";
    }
}
