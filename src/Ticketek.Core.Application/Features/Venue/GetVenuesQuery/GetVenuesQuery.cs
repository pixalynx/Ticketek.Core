using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Ticketek.Core.Application.Common.Exceptions;
using Ticketek.Core.Application.Common.Interfaces.Clients;
using Ticketek.Core.Application.Common.Models.EventLookup;

namespace Ticketek.Core.Application.Features.Venue.GetVenuesQuery;

public class GetVenuesQuery : IRequest<GetVenuesQueryResponse>
{
    
}

public class GetVenuesQueryResponse
{
    public List<VenueDto> Venues { get; set; }
    
    public GetVenuesQueryResponse(List<VenueDto> venues)
    {
        Venues = venues;
    }
}

public class GetVenuesQueryHandler : IRequestHandler<GetVenuesQuery, GetVenuesQueryResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IEventLookupClient _eventLookupClient;
    private readonly IMemoryCache _memoryCache;

    public GetVenuesQueryHandler(ILogger<GetVenuesQueryHandler> logger, IMapper mapper, 
        IEventLookupClient eventLookupClient, IMemoryCache memoryCache)
    {
        _logger = logger;
        _mapper = mapper;
        _eventLookupClient = eventLookupClient;
        _memoryCache = memoryCache;
    }

    public async Task<GetVenuesQueryResponse> Handle(GetVenuesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var lookUpResponse = await _eventLookupClient.GetAllEventsAndVenueDetails();
        
            var venues = _mapper.Map<List<VenueDto>>(lookUpResponse.Venues.ToList());
            
            //check if we have a cached response
            
            var cachedResponse = _memoryCache.Get<EventLookupResponse>("lookupResponse");
            
            // if cachedResponse is empty then add new cache entry
            if (cachedResponse != null) return new GetVenuesQueryResponse(venues);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            
            _memoryCache.Set("lookupResponse", lookUpResponse, cacheEntryOptions);

            return new GetVenuesQueryResponse(venues);
        }
        
        catch (TicketekApplicationException e)
        {
            // most likely something went wrong with the API call use our cache instead
            _logger.LogError(e, "An error occurred whilst fetching mapping JSON. Using cache instead.");
            var cachedResponse = _memoryCache.Get<EventLookupResponse>("lookupResponse");
            
            var venues = _mapper.Map<List<VenueDto>>(cachedResponse.Venues.ToList());

            return new GetVenuesQueryResponse(venues);
        }
    }
}