using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Common.Interfaces.External;
using FlexReport.Infrastructure.Configuration;
using FlexReport.Infrastructure.Integrations.DataAccess;
using FlexReport.Infrastructure.Integrations.OpenAI;
using FlexReport.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI_API;

namespace FlexReport.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlexReportDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("FlexReportDb"));
        });

        services.AddSingleton<IOpenAIAPI, OpenAIAPI>(_ => new OpenAIAPI(new APIAuthentication(configuration["OpenAI:ApiKey"])));
        services.AddSingleton<IOpenAIClient, OpenAIClient>();
        services.AddScoped<IDataAccess, SqlServerDataAccess>();
        services.AddScoped<IFlexReportDbContext>(provider => provider.GetRequiredService<FlexReportDbContext>());

        services.Configure<OpenAIConfiguration>(options => configuration.GetSection("OpenAI").Bind(options));

        return services;
    }
}