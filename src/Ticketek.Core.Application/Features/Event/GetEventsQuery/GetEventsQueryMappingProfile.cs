using AutoMapper;

namespace Ticketek.Core.Application.Features.Event.GetEventsQuery;

public class GetEventsQueryMappingProfile : Profile
{
    public GetEventsQueryMappingProfile()
    {
        CreateMap<Common.Models.EventLookup.Event, EventDto>();
    }
}