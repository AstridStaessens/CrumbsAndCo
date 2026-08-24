using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseContract?> LoginAsync(LoginRequestContract contract);
        Task<RegisterResult> RegisterAsync(RegisterRequestContract contract);

        /// <summary>
        /// Genereert een wachtwoord-resettoken en stuurt dit per e-mail naar de gebruiker.
        /// Geeft altijd succesvol terug, ook als het e-mailadres niet bestaat,
        /// zodat niet te achterhalen is welke e-mailadressen geregistreerd zijn.
        /// </summary>
        Task RequestPasswordResetAsync(ForgotPasswordRequestContract contract);

        /// <summary>
        /// Stelt het wachtwoord opnieuw in op basis van het token dat per e-mail verstuurd werd.
        /// </summary>
        /// <returns>true als het wachtwoord succesvol gewijzigd werd.</returns>
        Task<bool> ResetPasswordAsync(ResetPasswordRequestContract contract);
    }
}
