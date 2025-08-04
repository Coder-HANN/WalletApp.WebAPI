namespace WalletApp.Infrastructure.Services.EmailServices
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "live.smtp.mailtrap.io";
        public int SmtpPort { get; set; } = 587;
        public string SenderEmail { get; set; } = "hello@demomailtrap.co";
        public string SenderName { get; set; } = "api";
        public string Password { get; set; } = "dddef722f498c4eed5924a3605142a2c";
        public bool EnableSsl { get; set; } = true;
    }

}