using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using InfoTrack.Domain.Requests.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace InfoTrack.Domain;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateSearchResultRequestValidator>();

        return services;
    }
}