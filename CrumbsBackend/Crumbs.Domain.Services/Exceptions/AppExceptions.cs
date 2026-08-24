namespace Crumbs.Domain.Services.Exceptions
{
    /// <summary>
    /// Basisklasse voor alle voorspelbare applicatiefouten.
    /// De globale exception handler weet hoe hij hiermee om moet gaan.
    /// </summary>
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message) { }
    }

    /// <summary>
    /// Gooien wanneer een entiteit (order, product, ...) niet gevonden is. Resulteert in HTTP 404.
    /// </summary>
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// Gooien bij een ongeldige aanvraag die niet via DataAnnotations afgevangen kan worden
    /// (bv. onvoldoende voorraad, ongeldige statusovergang, ...). Resulteert in HTTP 400.
    /// </summary>
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message) { }
    }

    /// <summary>
    /// Gooien wanneer een actie niet toegelaten is voor de huidige gebruiker/staat,
    /// ondanks dat de gebruiker wel geauthenticeerd is. Resulteert in HTTP 403.
    /// </summary>
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message) : base(message) { }
    }

    /// <summary>
    /// Gooien wanneer een externe service (Mollie, SMTP, ...) een fout teruggeeft.
    /// Resulteert in HTTP 502.
    /// </summary>
    public class ExternalServiceException : AppException
    {
        public ExternalServiceException(string message) : base(message) { }
    }
}
