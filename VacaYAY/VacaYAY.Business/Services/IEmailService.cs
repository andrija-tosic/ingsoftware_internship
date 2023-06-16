namespace VacaYAY.Business.Services;

public interface IEmailService
{
    void EnqueueEmail(string email, string subject, string htmlMessage);
    Task SendEmail(string email, string subject, string htmlMessage);
}
