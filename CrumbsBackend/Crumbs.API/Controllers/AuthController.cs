using Crumbs.API.Contracts.RequestContracts;
using Crumbs.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Crumbs.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestContract contract)
        {
            var result = await _service.RegisterAsync(contract);
            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(" ", result.Errors) });

            return Ok(new { message = "Registratie succesvol." });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestContract contract)
        {
            var result = await _service.LoginAsync(contract);
            if (result == null) return Unauthorized("Ongeldige inloggegevens.");
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestContract contract)
        {
            await _service.RequestPasswordResetAsync(contract);
            // Altijd hetzelfde antwoord, ongeacht of het e-mailadres bestaat.
            return Ok(new { message = "Als dit e-mailadres bekend is, hebben we een resetlink verzonden." });
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestContract contract)
        {
            var result = await _service.ResetPasswordAsync(contract);
            if (!result) return BadRequest("Wachtwoord kon niet gewijzigd worden. De link is mogelijk verlopen of ongeldig.");
            return Ok(new { message = "Wachtwoord succesvol gewijzigd." });
        }
    }
}
