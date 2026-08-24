using System.ComponentModel.DataAnnotations;

namespace Crumbs.API.Contracts.RequestContracts
{
    public class ResetPasswordRequestContract
    {
        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Token is verplicht.")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "Nieuw wachtwoord is verplicht.")]
        [MinLength(6, ErrorMessage = "Wachtwoord moet minstens 6 tekens bevatten.")]
        public string NewPassword { get; set; } = null!;
    }
}
