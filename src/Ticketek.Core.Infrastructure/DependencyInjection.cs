using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketek.Core.Application.Common.Interfaces.Clients;
using Ticketek.Core.Infrastructure.Clients;

namespace Ticketek.Core.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IEventLookupClient, EventLookupClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("Endpoints:EventLookupService").Value);
        });

        return services;
    }
}