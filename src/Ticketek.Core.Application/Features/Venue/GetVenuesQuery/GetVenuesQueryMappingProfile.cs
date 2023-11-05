using AutoMapper;

namespace Ticketek.Core.Application.Features.Venue.GetVenuesQuery;

public class GetVenuesQueryMappingProfile : Profile
{
    public GetVenuesQueryMappingProfile()
    {
        CreateMap<Common.Models.EventLookup.Venue, VenueDto>();
    }
}