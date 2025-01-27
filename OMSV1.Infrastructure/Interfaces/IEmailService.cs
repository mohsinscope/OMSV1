public interface IEmailService
{
    Task SendEmailAsync(string from, string to, string subject, string body, byte[] pdfData = null);
}
