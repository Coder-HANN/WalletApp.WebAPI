using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Auth.Validators.Resource
{
    public class RegisterResource
    {
        public const string RegisteredEmail = "Email adresi zaten kayıtlı.";
        public const string InvalidEmailFormat = "Geçersiz email formatı.";
        public const string SuccessMessage = "Kayıt başarılı.";
        public const string ErrorMessage = "Kayıt sırasında bir hata oluştu.";
    }
}
