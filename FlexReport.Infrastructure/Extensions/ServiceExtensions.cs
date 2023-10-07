﻿using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Infrastructure.Integrations.OpenAI;
using FlexReport.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlexReport.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlexReportDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("FlexReportDb"));
        });

        services.AddSingleton<IOpenAIClient, OpenAIClient>();

        return services;
    }
}
