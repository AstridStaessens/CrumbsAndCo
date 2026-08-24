namespace Crumbs.Domain.Services.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Verstuurt een e-mail via de geconfigureerde SMTP-server.
        /// </summary>
        /// <param name="to">E-mailadres van de ontvanger.</param>
        /// <param name="subject">Onderwerp van de mail.</param>
        /// <param name="htmlBody">HTML-inhoud van de mail.</param>
        Task SendEmailAsync(string to, string subject, string htmlBody);
    }
}
