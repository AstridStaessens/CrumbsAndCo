using Crumbs.API.Contracts.RequestContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface IContactService
    {
        /// <summary>
        /// Verstuurt een algemene contactaanvraag per e-mail naar de bakkerij.
        /// </summary>
        Task SendContactRequestAsync(ContactRequestContract contract);

        /// <summary>
        /// Verstuurt een "op maat" bestelaanvraag per e-mail naar de bakkerij.
        /// </summary>
        Task SendCustomOrderRequestAsync(CustomOrderRequestContract contract);
    }
}
