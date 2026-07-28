namespace PersonRegistrationMvc.Services
{
    public class DevEmailSender : IEmailSender
    {
        private readonly string _outputPath;

        public DevEmailSender()
        {
            _outputPath = Path.Combine(AppContext.BaseDirectory, "dev-emails");
            Directory.CreateDirectory(_outputPath);
        }

        public Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss}_{toEmail.Replace("@", "_at_")}.html";
            var filePath = Path.Combine(_outputPath, fileName);
            File.WriteAllText(filePath, $"<h3>To: {toEmail}</h3><h3>Subject: {subject}</h3><hr/>{htmlMessage}");
            return Task.CompletedTask;
        }
    }
}