using System.Diagnostics.CodeAnalysis;
using InfoTrack.Services.Implementations;
using InfoTrack.Services.Implementations.SearchStrategies;
using InfoTrack.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.Services;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient("GoogleSearch");
        services.AddHttpClient("YahooSearch");
        
        services.AddScoped<ISearchService, SearchService>();
        services.AddTransient<GoogleSearchStrategy>();
        services.AddTransient<YahooSearchStrategy>();
        
        services.AddSingleton<ISearchStrategyResolver, SearchStrategyResolver>();

        return services;
    }
}