namespace OMSV1.Infrastructure.Interfaces;
public interface IEmailService
{
    Task SendEmailAsync(string from, string to, string subject, string body, byte[] pdfData = null);
    Task SendEmailToMultipleRecipientsAsync(string from, string[] recipients, string subject, string body, string pdfPath = null);
}
