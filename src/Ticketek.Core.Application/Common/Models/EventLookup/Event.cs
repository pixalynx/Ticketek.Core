using System.Text.Json.Serialization;

namespace Ticketek.Core.Application.Common.Models.EventLookup;

public class Event
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("venueId")]
    public int VenueId { get; set; }
}