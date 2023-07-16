#undef ENABLE_EMAILS

using Hangfire;
using Microsoft.AspNetCore.StaticFiles;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VacaYAY.Business.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;

    public EmailService(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
    }

    public void EnqueueEmail(string email, string subject, string htmlMessage, byte[]? attachment,
        string? attachmentName = null)
    {
#if ENABLE_EMAILS
        BackgroundJob.Enqueue(() => SendEmail(email, subject, htmlMessage, attachment, attachmentName));
#endif
    }

    [AutomaticRetry(Attempts = int.MaxValue, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task SendEmail(string email, string subject, string htmlMessage, byte[]? attachment,
        string? attachmentName)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress("andrija.tosic@ingsoftware.com"),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = $"<pre>{htmlMessage}</pre>"
        };

        if (!string.IsNullOrWhiteSpace(attachmentName) && attachment is not null)
        {
            string? contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(attachmentName, out contentType);

            message.AddAttachment(attachmentName, Convert.ToBase64String(attachment), contentType);
        }

        message.AddTo(new EmailAddress(email));

        var response = await _sendGridClient.SendEmailAsync(message);
    }
}
