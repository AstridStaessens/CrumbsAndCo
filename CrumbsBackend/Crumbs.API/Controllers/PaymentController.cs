using Crumbs.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crumbs.API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("create/{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> CreatePayment(int orderId)
        {
            var result = await _service.CreatePaymentAsync(orderId);
            return Ok(result);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> Webhook([FromForm] string id)
        {
            await _service.HandleWebhookAsync(id);
            return Ok();
        }

        [HttpGet("status/{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> GetStatus(int orderId)
        {
            var status = await _service.GetPaymentStatusAsync(orderId);
            return Ok(new { status });
        }

        [HttpPost("refund/{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult> Refund(int orderId)
        {
            await _service.RefundAsync(orderId);
            return Ok(new { message = "Bestelling werd gemarkeerd als terugbetaald." });
        }
    }
}