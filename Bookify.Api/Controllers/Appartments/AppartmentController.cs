using Bookify.Application.Appartments.SearchAppartments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Appartments
{
    [Route("api/appartments")]
    [ApiController]
    public class AppartmentController : ControllerBase
    {
        private readonly ISender _sender;

        public AppartmentController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAppertments (DateOnly startDate,  DateOnly endDate, CancellationToken cancellationToken)
        {
            var query = new SearchAppartmentsQuery(startDate, endDate);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
