using Crumbs.API.Contracts.RequestContracts;
using Crumbs.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Crumbs.Domain.Services
{
    /// <summary>
    /// Verwerkt contact- en "op maat"-aanvragen door deze rechtstreeks per e-mail
    /// naar de bakkerij te sturen. Deze aanvragen worden niet in de database opgeslagen.
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ContactService(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        private string OwnerEmail => _configuration["Contact:OwnerEmail"] ?? "astrid.staessens@student.hogent.be";

        public async Task SendContactRequestAsync(ContactRequestContract contract)
        {
            var subject = $"Nieuw contactbericht van {contract.Naam}";

            var body = $@"
                <h2>Nieuw contactbericht via Crumbs &amp; Co</h2>
                <p><strong>Naam:</strong> {Encode(contract.Naam)}</p>
                <p><strong>E-mail:</strong> {Encode(contract.Email)}</p>
                <p><strong>Bericht:</strong></p>
                <p>{Encode(contract.Bericht).Replace("\n", "<br/>")}</p>
            ";

            await _emailService.SendEmailAsync(OwnerEmail, subject, body);
        }

        public async Task SendCustomOrderRequestAsync(CustomOrderRequestContract contract)
        {
            var subject = $"Nieuwe aanvraag op maat van {contract.Naam} ({contract.Type})";

            var fotoRegel = string.IsNullOrWhiteSpace(contract.FileName)
                ? "<p><strong>Inspiratiefoto:</strong> geen</p>"
                : $"<p><strong>Inspiratiefoto:</strong> {Encode(contract.FileName)} (bijlage werd niet meegestuurd, vraag de klant deze opnieuw door te sturen indien nodig)</p>";

            var body = $@"
                <h2>Nieuwe 'op maat' aanvraag via Crumbs &amp; Co</h2>
                <p><strong>Naam:</strong> {Encode(contract.Naam)}</p>
                <p><strong>E-mail:</strong> {Encode(contract.Email)}</p>
                <p><strong>Telefoon:</strong> {Encode(contract.Telefoon ?? "-")}</p>
                <p><strong>Type bestelling:</strong> {Encode(contract.Type)}</p>
                <p><strong>Gewenste ophaaldatum:</strong> {Encode(contract.Datum)}</p>
                <p><strong>Wensen:</strong></p>
                <p>{Encode(contract.Wensen).Replace("\n", "<br/>")}</p>
                {fotoRegel}
            ";

            await _emailService.SendEmailAsync(OwnerEmail, subject, body);
        }

        private static string Encode(string value) => WebUtility.HtmlEncode(value);
    }
}
