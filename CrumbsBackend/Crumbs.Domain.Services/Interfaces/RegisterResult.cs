namespace Crumbs.Domain.Services.Interfaces
{
    /// <summary>
    /// Resultaat van een registratiepoging. Bevat bij falen de specifieke
    /// foutmeldingen van ASP.NET Identity (bv. wachtwoordeisen), zodat de
    /// frontend de gebruiker kan tonen wat er precies mis is.
    /// </summary>
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public List<string> Errors { get; set; } = [];

        public static RegisterResult Success() => new() { Succeeded = true };

        public static RegisterResult Failure(IEnumerable<string> errors) => new()
        {
            Succeeded = false,
            Errors = errors.ToList()
        };
    }
}
