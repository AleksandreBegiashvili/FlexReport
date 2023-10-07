using FlexReport.Application.Common.Interfaces;
using FlexReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Infrastructure.Persistence;

public class FlexReportDbContext : DbContext, IFlexReportDbContext
{
    public FlexReportDbContext(DbContextOptions<FlexReportDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Report> Reports => Set<Report>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Report>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.CustomerId);
    }
}