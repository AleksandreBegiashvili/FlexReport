using FlexReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Application.Common.Interfaces;

public interface IFlexReportDbContext
{
    DbSet<Customer> Customers { get; }

    DbSet<Report> Reports { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}