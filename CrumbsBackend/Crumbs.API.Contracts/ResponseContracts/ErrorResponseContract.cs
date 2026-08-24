namespace Crumbs.API.Contracts.ResponseContracts
{
    /// <summary>
    /// Uniform foutformaat dat door de globale exception handler teruggegeven wordt.
    /// </summary>
    public class ErrorResponseContract
    {
        public string Message { get; set; } = string.Empty;
        public int Status { get; set; }
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
