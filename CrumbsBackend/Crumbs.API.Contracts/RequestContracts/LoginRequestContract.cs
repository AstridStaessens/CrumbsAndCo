using System.ComponentModel.DataAnnotations;

namespace Crumbs.API.Contracts.RequestContracts
{
    public class LoginRequestContract
    {
        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        public string Password { get; set; } = null!;
    }
}
