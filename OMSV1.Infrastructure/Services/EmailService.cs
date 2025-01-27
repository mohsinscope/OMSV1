using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _settings = emailSettings.Value;
        _logger = logger;
    }

  public async Task SendEmailAsync(string from, string to, string subject, string body, byte[] pdfData = null)
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Sender", from));
    message.To.Add(new MailboxAddress("Recipient", to));
    message.Subject = subject;

    var bodyBuilder = new BodyBuilder { TextBody = body };

    if (pdfData != null)
    {
        bodyBuilder.Attachments.Add("MonthlyExpensesReport.pdf", pdfData, new ContentType("application", "pdf"));
    }

    message.Body = bodyBuilder.ToMessageBody();

    using var smtpClient = new SmtpClient();
    try
    {
        await smtpClient.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password);
        await smtpClient.SendAsync(message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error sending email");
        throw;
    }
    finally
    {
        await smtpClient.DisconnectAsync(true);
    }
}


    public async Task SendEmailToMultipleRecipientsAsync(string from, string[] recipients, string subject, string body, string pdfPath = null)
    {
        try
        {
            var message = CreateEmailMessage(from, recipients, subject, body, pdfPath);
            await SendAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending an email to multiple recipients");
            throw;
        }
    }

    private MimeMessage CreateEmailMessage(string from, string[] recipients, string subject, string body, string pdfPath)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender", from));

        foreach (var recipient in recipients)
        {
            message.To.Add(new MailboxAddress("Recipient", recipient));
        }

        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { TextBody = body };
        if (!string.IsNullOrEmpty(pdfPath))
        {
            bodyBuilder.Attachments.Add(pdfPath);
        }

        message.Body = bodyBuilder.ToMessageBody();
        return message;
    }

    private async Task SendAsync(MimeMessage message)
    {
        using var smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtpClient.SendAsync(message);
            _logger.LogInformation("Email sent successfully to {Recipients}", string.Join(", ", message.To));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            throw;
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
}
