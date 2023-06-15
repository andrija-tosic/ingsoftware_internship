using Polly;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace VacaYAY.Business.Services;

public class EmailService : IEmailService
{
    private readonly ISendGridClient _sendGridClient;

    private readonly IAsyncPolicy<Response> _retryPolicy =
        Policy<Response>
        .Handle<Exception>()
        .OrResult(r => !r.IsSuccessStatusCode)
        .WaitAndRetryForeverAsync(x => TimeSpan.FromSeconds(5));

    public EmailService(ISendGridClient sendGridClient)
    {
        _sendGridClient = sendGridClient;
    }

    public async Task<Response> SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
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

            return response;
        });
    }
}
