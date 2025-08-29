using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletApp.Application.Feature.User.Validators.Resource
{
    public class DeleteUserAccountResource
    {
        public const string UserNotFound = "Kullanıcı bulunamadı";
        public const string UserAndMailNotMatch = "Girilen email, giriş yapan kullanıcıya ait değil.";
        public const string InvalidPassword = "Geçersiz şifre.";
        public const string WhyAreYouLeaving = "Hesabınızı neden kapatmak istediğinizi bizimle paylaşın.";
        public const string SuccessMessage = "Hesabınız başarıyla silindi.";
    }
}
