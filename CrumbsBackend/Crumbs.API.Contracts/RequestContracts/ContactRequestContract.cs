using System.ComponentModel.DataAnnotations;

namespace Crumbs.API.Contracts.RequestContracts
{
    public class ContactRequestContract
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam mag maximaal 100 tekens bevatten.")]
        public string Naam { get; set; } = null!;

        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Bericht is verplicht.")]
        [StringLength(2000, ErrorMessage = "Bericht mag maximaal 2000 tekens bevatten.")]
        public string Bericht { get; set; } = null!;
    }
}
