using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Ticketek.Core.Application.Common.Exceptions;
using Ticketek.Core.Application.Common.Interfaces.Clients;
using Ticketek.Core.Application.Common.Models.EventLookup;

namespace Ticketek.Core.Application.Features.Event.GetEventsQuery;

public class GetEventsQuery : IRequest<GetEventsQueryResponse>
{
    public int VenueId { get; set; }

    public GetEventsQuery(int venueId)
    {
        VenueId = venueId;
    }
}

public class GetEventsQueryResponse
{
    public List<EventDto> Events { get; set; }

    public GetEventsQueryResponse(List<EventDto> events)
    {
        Events = events;
    }
}

public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, GetEventsQueryResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IEventLookupClient _eventLookupClient;
    private readonly IMemoryCache _memoryCache;

    public GetEventsQueryHandler(IEventLookupClient eventLookupClient, IMapper mapper, 
        ILogger<GetEventsQuery> logger, IMemoryCache memoryCache)
    {
        _eventLookupClient = eventLookupClient;
        _mapper = mapper;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<GetEventsQueryResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var lookUpResponse = await _eventLookupClient.GetAllEventsAndVenueDetails();
        
            var events = _mapper.Map<List<EventDto>>(lookUpResponse.Events.Where(x => x.VenueId == request.VenueId).ToList());

            var cachedResponse = _memoryCache.Get<EventLookupResponse>("lookupResponse");
            
            if (cachedResponse != null) return new GetEventsQueryResponse(events);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
            
            _memoryCache.Set("lookupResponse", lookUpResponse, cacheEntryOptions);

            
            return new GetEventsQueryResponse(events);
        }
        
        catch (TicketekApplicationException e)
        {
            // most likely something went wrong with the API call use our cache instead
            _logger.LogError(e, "An error occurred whilst fetching mapping JSON. Using cache instead.");
            var cachedResponse = _memoryCache.Get<EventLookupResponse>("lookupResponse");
            
            var events = _mapper.Map<List<EventDto>>(cachedResponse.Events.Where(x => x.VenueId == request.VenueId).ToList());

            return new GetEventsQueryResponse(events);
        }
    }
}