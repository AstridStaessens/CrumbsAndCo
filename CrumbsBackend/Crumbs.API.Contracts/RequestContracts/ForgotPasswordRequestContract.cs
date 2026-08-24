using System.ComponentModel.DataAnnotations;

namespace Crumbs.API.Contracts.RequestContracts
{
    public class ForgotPasswordRequestContract
    {
        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; } = null!;
    }
}
