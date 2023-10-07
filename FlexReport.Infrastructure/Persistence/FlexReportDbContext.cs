using FlexReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Infrastructure.Persistence;

public class FlexReportDbContext : DbContext
{
    public FlexReportDbContext(DbContextOptions<FlexReportDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Report> Reports { get; set; }

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
