using System.Text.Json.Serialization;

namespace Ticketek.Core.Application.Common.Models.EventLookup;

public class EventLookupResponse
{
    [JsonPropertyName("events")]
    public List<Event> Events { get; set; }
    [JsonPropertyName("venues")]
    public List<Venue> Venues { get; set; }

    public EventLookupResponse()
    {
        Events = new List<Event>();
        Venues = new List<Venue>();
    }
}