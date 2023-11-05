using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketek.Core.Application.Common.AppSettings;

namespace Ticketek.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // we want to use this to cache the results of the events lookup service incase of outage
        services.AddMemoryCache(); 
        services.AddConfigs(config);
        
        return services;
    }
    
    private static void AddConfigs(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<Endpoints>(config.GetSection(nameof(Endpoints)));
    }
    
    private static void AddPipelineBehaviors(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}