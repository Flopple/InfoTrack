using System;
using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.AspNetCore;
using InfoTrack.Common.Settings;
using InfoTrack.Domain;
using InfoTrack.Domain.Requests.Validators;
using InfoTrack.Persistence;
using InfoTrack.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InfoTrack.Web.API;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args
        });

        builder.Configuration.AddConfiguration(configuration);

        builder.WebHost.ConfigureKestrel(options =>
        {
            var url = builder.Configuration["Kestrel:Endpoints:Http:Url"];
            if (!string.IsNullOrWhiteSpace(url))
            {
                options.ListenAnyIP(new Uri(url).Port,
                    listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
            }
        });

        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();

        Configure(app);

        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddFluentValidationAutoValidation();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLogging();

        services.Configure<InfoTrackSettings>(configuration.GetSection("InfoTrackSettings"));

        services
            .AddServices()
            .AddValidators()
            .AddPersistence(configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AppPolicy", policy =>
            {
                policy.WithOrigins(configuration["Cors:CorsOrigins"] ?? string.Empty)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    private static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("AppPolicy");

        app.MapControllers();
    }
}