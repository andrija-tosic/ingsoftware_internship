namespace VacaYAY.Business.Services;

public interface IEmailService
{
    void EnqueueEmail(string email, string subject, string htmlMessage, byte[]? attachment = null, string? attachmentName = null);
}
