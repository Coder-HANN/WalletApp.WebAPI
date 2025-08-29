using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.Auth.Validators.Resource
{
    public class LoginResource
    {
        public const string EmailorPasswordRequired = "Email veya şifre boş olamaz.";
        public const string InvalidEmailOrPassword = "Geçersiz Email veya şifre.";
        public const string SuccessMessage = "Giriş başarılı.";
    }
}
