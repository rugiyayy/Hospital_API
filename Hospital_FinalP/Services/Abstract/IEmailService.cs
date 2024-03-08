namespace Hospital_FinalP.Services.Abstract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string fromDisplayName, string to, string subject, string body, string? from = null);
    }
}
