using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Models.Interfaces;
using Crumbs.Domain.Services.Exceptions;
using Crumbs.Domain.Services.Interfaces;
using Crumbs.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Request;
using System.Globalization;
using System.Net;
using System.Text;

namespace Crumbs.Domain.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IConfiguration _configuration;
        private readonly IPaymentClient _molliePaymentClient;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IConfiguration configuration,
            IPaymentClient molliePaymentClient,
            IEmailService emailService,
            UserManager<IdentityUser> userManager,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _configuration = configuration;
            _molliePaymentClient = molliePaymentClient;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<PaymentResponseContract> CreatePaymentAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException($"Order met id {orderId} werd niet gevonden.");

            var paymentRequest = new PaymentRequest
            {
                Amount = new Mollie.Api.Models.Amount("EUR",
                    order.Total.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)),
                Description = $"Crumbs bestelling #{orderId}",
                RedirectUrl = "https://yellow-water-0e98f6110.7.azurestaticapps.net/order/confirmation",
                WebhookUrl = _configuration["Mollie:WebhookUrl"],
            };

            var paymentResponse = await _molliePaymentClient.CreatePaymentAsync(paymentRequest);

            var payment = new PaymentEntity
            {
                OrderId = orderId,
                MolliePaymentId = paymentResponse.Id,
                Amount = order.Total,
                Status = "pending",
                Date = DateTime.UtcNow
            };

            await _paymentRepository.CreateAsync(payment);

            order.Status = "pending_payment";
            await _orderRepository.UpdateAsync(order);

            return new PaymentResponseContract
            {
                PaymentUrl = paymentResponse.Links.Checkout.Href,
                MolliePaymentId = paymentResponse.Id,
                Status = "pending"
            };
        }

        public async Task HandleWebhookAsync(string molliePaymentId)
        {
            var molliePayment = await _molliePaymentClient.GetPaymentAsync(molliePaymentId);

            var payment = await _paymentRepository.GetByMolliePaymentIdAsync(molliePaymentId);
            if (payment == null) return;

            var newStatus = molliePayment.Status.ToString().ToLower();
            var wasAlreadyPaid = payment.Status == "paid";
            payment.Status = newStatus;
            await _paymentRepository.UpdateAsync(payment);

            if (molliePayment.Status == "paid")
            {
                var order = await _orderRepository.GetByIdAsync(payment.OrderId);
                if (order == null) return;
                order.Status = "paid";
                await _orderRepository.UpdateAsync(order);

                // Enkel een bevestigingsmail sturen als de betaling nu pas "paid" wordt,
                // zodat herhaalde webhook-calls van Mollie niet voor dubbele mails zorgen.
                if (!wasAlreadyPaid)
                {
                    await SendOrderConfirmationEmailAsync(order);
                }
            }
        }

        /// <summary>
        /// Stuurt een orderbevestiging per e-mail naar de klant. Fouten hierbij
        /// (bv. SMTP niet geconfigureerd) worden gelogd maar gooien geen exception,
        /// zodat de Mollie-webhook altijd met 200 OK beantwoord wordt en de order
        /// gewoon als 'paid' blijft staan, ook als de mail niet verzonden kon worden.
        /// </summary>
        private async Task SendOrderConfirmationEmailAsync(OrderEntity order)
        {
            if (string.IsNullOrEmpty(order.UserId))
            {
                _logger.LogWarning("Geen UserId gekoppeld aan order {OrderId}, geen bevestigingsmail verstuurd.", order.Id);
                return;
            }

            var user = await _userManager.FindByIdAsync(order.UserId);
            if (user?.Email == null)
            {
                _logger.LogWarning("Geen e-mailadres gevonden voor gebruiker van order {OrderId}, geen bevestigingsmail verstuurd.", order.Id);
                return;
            }

            try
            {
                var subject = $"Bevestiging van je bestelling #{order.Id} - Crumbs & Co";
                var body = BuildOrderConfirmationEmailBody(order);
                await _emailService.SendEmailAsync(user.Email, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Versturen van orderbevestiging voor order {OrderId} naar {Email} is mislukt.", order.Id, user.Email);
            }
        }

        private static string BuildOrderConfirmationEmailBody(OrderEntity order)
        {
            var sb = new StringBuilder();
            sb.Append("<h2>Bedankt voor je bestelling bij Crumbs &amp; Co!</h2>");
            sb.Append($"<p>We hebben je betaling voor bestelling <strong>#{order.Id}</strong> ontvangen.</p>");
            sb.Append("<table style=\"width:100%; border-collapse: collapse; margin-top: 1rem;\">");
            sb.Append("<thead><tr>");
            sb.Append("<th style=\"text-align:left; border-bottom:1px solid #ddd; padding:0.5rem;\">Product</th>");
            sb.Append("<th style=\"text-align:center; border-bottom:1px solid #ddd; padding:0.5rem;\">Aantal</th>");
            sb.Append("<th style=\"text-align:right; border-bottom:1px solid #ddd; padding:0.5rem;\">Prijs</th>");
            sb.Append("<th style=\"text-align:right; border-bottom:1px solid #ddd; padding:0.5rem;\">Subtotaal</th>");
            sb.Append("</tr></thead><tbody>");

            foreach (var line in order.OrderLines)
            {
                var productName = WebUtility.HtmlEncode(line.Product?.Name ?? "Onbekend product");
                var subtotal = line.UnitPrice * line.Quantity;

                sb.Append("<tr>");
                sb.Append($"<td style=\"padding:0.5rem; border-bottom:1px solid #f0f0f0;\">{productName}</td>");
                sb.Append($"<td style=\"text-align:center; padding:0.5rem; border-bottom:1px solid #f0f0f0;\">{line.Quantity}</td>");
                sb.Append($"<td style=\"text-align:right; padding:0.5rem; border-bottom:1px solid #f0f0f0;\">{FormatPrice(line.UnitPrice)}</td>");
                sb.Append($"<td style=\"text-align:right; padding:0.5rem; border-bottom:1px solid #f0f0f0;\">{FormatPrice(subtotal)}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
            sb.Append("<p style=\"text-align:right; font-weight:bold; margin-top:1rem;\">");
            sb.Append($"Totaal: {FormatPrice(order.Total)}");
            sb.Append("</p>");
            sb.Append("<p>We laten je weten zodra je bestelling klaar is om af te halen.</p>");
            sb.Append("<p>Tot binnenkort!<br/>Crumbs &amp; Co</p>");

            return sb.ToString();
        }

        private static string FormatPrice(decimal amount) =>
            $"€ {amount.ToString("F2", CultureInfo.InvariantCulture)}";

        public async Task<string> GetPaymentStatusAsync(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            if (payment == null) return "no_payment";
            return payment.Status;
        }

        public async Task RefundAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new NotFoundException($"Order met id {orderId} werd niet gevonden.");

            if (order.Status != "paid" && order.Status != "in_production" && order.Status != "ready")
                throw new BadRequestException(
                    $"Een bestelling met status '{order.Status}' kan niet terugbetaald worden.");

            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            if (payment == null)
                throw new NotFoundException($"Geen betaling gevonden voor order {orderId}.");

            // NOTE: dit markeert de betaling/order enkel als 'refunded' in onze eigen database.
            // De effectieve terugstorting via Mollie's Refund API (IRefundClient.CreatePaymentRefundAsync)
            // moet hier nog aan toegevoegd worden vóór dit in een echte productieomgeving gebruikt wordt.
            payment.Status = "refunded";
            await _paymentRepository.UpdateAsync(payment);

            order.Status = "refunded";
            await _orderRepository.UpdateAsync(order);
        }
    }
}