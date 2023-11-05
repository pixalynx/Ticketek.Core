namespace Ticketek.Core.Application.Features.Event;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public int VenueId { get; set; }
}