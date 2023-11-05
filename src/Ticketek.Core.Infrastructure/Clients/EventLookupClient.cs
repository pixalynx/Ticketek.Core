using System.Text.Json;
using Microsoft.Extensions.Logging;
using Ticketek.Core.Application.Common.Exceptions;
using Ticketek.Core.Application.Common.Interfaces.Clients;
using Ticketek.Core.Application.Common.Models.EventLookup;

namespace Ticketek.Core.Infrastructure.Clients;

public class EventLookupClient : IEventLookupClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    public EventLookupClient(ILogger<EventLookupClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<EventLookupResponse> GetAllEventsAndVenueDetails()
    {
        var response = await _httpClient.GetAsync(EventLookupApiConsts.EventAndVenuesEndpoint);
        var contentResponse = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<EventLookupResponse>(contentResponse) ?? throw new InvalidOperationException();
        }
        
        _logger.LogError(
            "An error occurred whilst fetching mapping JSON. Response: {0}",
            contentResponse);

        throw new TicketekApplicationException("Unable to fetch event and venue details.");
    }
}