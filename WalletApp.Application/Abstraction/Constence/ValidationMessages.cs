
namespace WalletApp.Application.Abstraction.Constence
{
    public class ValidationMessages
    {
        public const string EmailRequired = "Email boş olamaz.";
        public const string EmailInvalid = "Geçerli bir Eposta adreisi girin.";
        public const string PasswordRequired = "Şifre boş olamaz.";
        public const string PasswordMinLength = "Şifre en az 6 karakter olmalıdır.";
        public const string NameRequired = "İsim boş olamaz.";
        public const string BirthDayRequired = "Doğum günü boş olamaz.";
        public const string BirthDayInvalid = "Geçerli bir tarih girin.";
        public const string PhoneNumberRequired = "Telefon numarası boş olamaz.";
        public const string PhoneNumberInvalid = "Geçerli bir telefon numarası girin.";
        public const string OccupationRequired = "Lütfen meslek seçimi yapınız.";
        public const string OccupationMaxLength = "Meslek en fazla 100 karakter olmalıdır.";
        public const string HereRequired = "Bu kısım boş olamaz.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
    }
}
