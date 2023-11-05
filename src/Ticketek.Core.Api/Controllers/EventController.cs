using Microsoft.AspNetCore.Mvc;
using Ticketek.Core.Application.Features.Event.GetEventsQuery;
using Ticketek.Core.Application.Features.Venue.GetVenuesQuery;

namespace Ticketek.Core.Api.Controllers;

public class EventController : ApiControllerBase
{
    [HttpGet ("{venueId}/events")]
    public async Task<IActionResult> GetEvents([FromRoute] int venueId)
    {
        var response = await Mediator.Send(new GetEventsQuery(venueId));

        return Ok(response);
    }
    
    [HttpGet("venues")]
    public async Task<IActionResult> GetVenues()
    {
        var response = await Mediator.Send(new GetVenuesQuery());

        return Ok(response);
    }
}