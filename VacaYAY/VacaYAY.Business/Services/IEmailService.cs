using SendGrid;

namespace VacaYAY.Business.Services;

public interface IEmailService
{
    Task<Response> SendEmailAsync(string email, string subject, string htmlMessage);
}
