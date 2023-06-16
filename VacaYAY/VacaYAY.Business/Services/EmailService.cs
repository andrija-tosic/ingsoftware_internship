using Hangfire;
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

    public void EnqueueEmail(string email, string subject, string htmlMessage)
    {
        BackgroundJob.Enqueue(() => SendEmail(email, subject, htmlMessage));
    }

    [AutomaticRetry(Attempts = int.MaxValue, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public async Task SendEmail(string email, string subject, string htmlMessage)
    {
        var message = new SendGridMessage
        {
            From = new EmailAddress("andrija.tosic@ingsoftware.com"),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = $"<pre>{htmlMessage}</pre>"
        };

        message.AddTo(new EmailAddress(email));

        var response = await _sendGridClient.SendEmailAsync(message);
    }
}
