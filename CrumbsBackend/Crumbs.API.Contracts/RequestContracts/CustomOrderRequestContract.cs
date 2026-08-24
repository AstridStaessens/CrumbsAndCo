using System.ComponentModel.DataAnnotations;

namespace Crumbs.API.Contracts.RequestContracts
{
    public class CustomOrderRequestContract
    {
        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam mag maximaal 100 tekens bevatten.")]
        public string Naam { get; set; } = null!;

        [Required(ErrorMessage = "E-mail is verplicht.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        [StringLength(200)]
        public string Email { get; set; } = null!;

        [StringLength(30)]
        public string? Telefoon { get; set; }

        [Required(ErrorMessage = "Type bestelling is verplicht.")]
        [StringLength(50)]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "Gewenste ophaaldatum is verplicht.")]
        public string Datum { get; set; } = null!;

        [Required(ErrorMessage = "Wensen zijn verplicht.")]
        [StringLength(2000, ErrorMessage = "Wensen mogen maximaal 2000 tekens bevatten.")]
        public string Wensen { get; set; } = null!;

        [StringLength(255)]
        public string? FileName { get; set; }
    }
}
