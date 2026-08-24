using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Crumbs.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            IEmailService emailService,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterRequestContract contract)
        {
            var user = new IdentityUser
            {
                UserName = contract.Email,
                Email = contract.Email
            };

            try
            {
                var result = await _userManager.CreateAsync(user, contract.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    _logger.LogWarning("Registratie mislukt voor {Email}: {Errors}",
                        contract.Email, string.Join(", ", errors));
                    return RegisterResult.Failure(errors);
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "Klant");
                if (!roleResult.Succeeded)
                {
                    var errors = roleResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Rol toekennen mislukt voor {Email}: {Errors}",
                        contract.Email, string.Join(", ", errors));
                    return RegisterResult.Failure(errors);
                }

                var claimResult = await _userManager.AddClaimAsync(user, new Claim("name", contract.Name));
                if (!claimResult.Succeeded)
                {
                    var errors = claimResult.Errors.Select(e => e.Description).ToList();
                    _logger.LogError("Claim toevoegen mislukt voor {Email}: {Errors}",
                        contract.Email, string.Join(", ", errors));
                    return RegisterResult.Failure(errors);
                }

                return RegisterResult.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij registratie van {Email}.", contract.Email);
                return RegisterResult.Failure(new[] { "Er is een onverwachte fout opgetreden bij het registreren. Probeer het later opnieuw." });
            }
        }

        public async Task<AuthResponseContract?> LoginAsync(LoginRequestContract contract)
        {
            var user = await _userManager.FindByEmailAsync(contract.Email);
            if (user == null) return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, contract.Password);
            if (!isPasswordValid) return null;

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var role = roles.FirstOrDefault() ?? "Klant";
            var name = claims.FirstOrDefault(c => c.Type == "name")?.Value ?? user.Email!;

            var token = GenerateToken(user, role, name);

            return new AuthResponseContract
            {
                Token = token,
                Email = user.Email!,
                Name = name,
                Role = role
            };
        }

        public async Task RequestPasswordResetAsync(ForgotPasswordRequestContract contract)
        {
            var user = await _userManager.FindByEmailAsync(contract.Email);

            // Bewust geen foutmelding als de gebruiker niet bestaat: anders kan iemand
            // via dit endpoint controleren welke e-mailadressen geregistreerd zijn.
            if (user == null)
            {
                _logger.LogInformation("Wachtwoord-reset aangevraagd voor onbekend e-mailadres {Email}.", contract.Email);
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = UrlSafeBase64Encode(token);

            var frontendUrl = _configuration["Frontend:BaseUrl"] ?? "https://yellow-water-0e98f6110.7.azurestaticapps.net";
            var resetUrl = $"{frontendUrl}/reset-password?email={WebUtility.UrlEncode(contract.Email)}&token={encodedToken}";

            var body = $@"
                <h2>Wachtwoord opnieuw instellen - Crumbs &amp; Co</h2>
                <p>Je hebt een wachtwoordreset aangevraagd voor je account.</p>
                <p>Klik op de link hieronder om een nieuw wachtwoord in te stellen. Deze link is een beperkte tijd geldig.</p>
                <p><a href=""{resetUrl}"">{resetUrl}</a></p>
                <p>Heb je deze aanvraag niet gedaan? Dan kan je deze e-mail negeren.</p>
            ";

            await _emailService.SendEmailAsync(contract.Email, "Wachtwoord opnieuw instellen - Crumbs & Co", body);
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestContract contract)
        {
            var user = await _userManager.FindByEmailAsync(contract.Email);
            if (user == null) return false;

            string decodedToken;
            try
            {
                decodedToken = UrlSafeBase64Decode(contract.Token);
            }
            catch (FormatException)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, contract.NewPassword);
            return result.Succeeded;
        }

        private string GenerateToken(IdentityUser user, string role, string name)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
                new Claim("name", name)
            };

            var expiration = DateTime.UtcNow.AddDays(
                int.Parse(_configuration["JwtSettings:ExpirationInDays"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Codeert een string als URL-veilige Base64 (geen +, /, of padding =),
        /// zodat het token zonder problemen als query-parameter meegegeven kan worden.
        /// </summary>
        private static string UrlSafeBase64Encode(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }

        /// <summary>
        /// Decodeert een string die met <see cref="UrlSafeBase64Encode"/> gecodeerd werd.
        /// </summary>
        private static string UrlSafeBase64Decode(string value)
        {
            var base64 = value.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            var bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
