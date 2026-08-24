using Crumbs.API.Contracts.RequestContracts;
using Crumbs.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crumbs.API.Controllers
{
    /// <summary>
    /// Endpoints voor het contactformulier en het "op maat"-aanvraagformulier.
    /// Deze sturen enkel een e-mail naar de bakkerij en slaan niets op in de database.
    /// </summary>
    [ApiController]
    [Route("api/contact")]
    [AllowAnonymous]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult> SendContact([FromBody] ContactRequestContract contract)
        {
            await _service.SendContactRequestAsync(contract);
            return Ok(new { message = "Bericht succesvol verzonden." });
        }

        [HttpPost("op-maat")]
        public async Task<ActionResult> SendCustomOrder([FromBody] CustomOrderRequestContract contract)
        {
            await _service.SendCustomOrderRequestAsync(contract);
            return Ok(new { message = "Aanvraag succesvol verzonden." });
        }
    }
}
