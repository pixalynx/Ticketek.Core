using Ticketek.Core.Application.Common.Models.EventLookup;

namespace Ticketek.Core.Application.Common.Interfaces.Clients;

public interface IEventLookupClient
{
    Task<EventLookupResponse> GetAllEventsAndVenueDetails();
}