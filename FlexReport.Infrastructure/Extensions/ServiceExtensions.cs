﻿using FlexReport.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlexReport.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlexReportDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("FlexReportDb"));
        });

        return services;
    }
}