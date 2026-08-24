using Crumbs.Domain.Services.Exceptions;
using Crumbs.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Crumbs.Domain.Services.Email
{
    /// <summary>
    /// Verstuurt e-mails via SMTP (Outlook/Office365).
    /// Configuratie wordt gelezen uit de "Smtp"-sectie in appsettings.json:
    ///
    /// "Smtp": {
    ///   "Host": "smtp.office365.com",
    ///   "Port": 587,
    ///   "User": "astrid.staessens@student.hogent.be",
    ///   "Password": "...",
    ///   "FromName": "Crumbs & Co"
    /// }
    /// </summary>
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var host = _configuration["Smtp:Host"];
            var portRaw = _configuration["Smtp:Port"];
            var user = _configuration["Smtp:User"];
            var password = _configuration["Smtp:Password"];
            var fromName = _configuration["Smtp:FromName"] ?? "Crumbs & Co";

            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogError("SMTP-instellingen ontbreken (Smtp:Host/User/Password). E-mail naar {To} met onderwerp '{Subject}' is NIET verzonden.", to, subject);
                throw new ExternalServiceException("E-mail kon niet verzonden worden: SMTP is niet correct geconfigureerd.");
            }

            var port = int.TryParse(portRaw, out var parsedPort) ? parsedPort : 587;

            using var message = new MailMessage
            {
                From = new MailAddress(user, fromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, password),
                EnableSsl = true
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Versturen van e-mail naar {To} met onderwerp '{Subject}' is mislukt.", to, subject);
                throw new ExternalServiceException("E-mail kon niet verzonden worden. Probeer het later opnieuw.");
            }
        }
    }
}
