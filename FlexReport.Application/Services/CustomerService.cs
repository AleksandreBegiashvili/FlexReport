using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Models.Requests;
using FlexReport.Domain.Entities;

namespace FlexReport.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IDbSchemaGenerator _dbSchemaGenerator;
    private readonly IFlexReportDbContext _flexReportDbContext;

    public CustomerService(IFlexReportDbContext flexReportDbContext,
        IDbSchemaGenerator dbSchemaGenerator)
    {
        _flexReportDbContext = flexReportDbContext;
        _dbSchemaGenerator = dbSchemaGenerator;
    }

    public async Task<int> SynchronizeCustomer(SynchronizeCustomerRequest request)
    {
        var dbSchema = _dbSchemaGenerator.Generate(request.DbConnectionString);

        var customer = new Customer
        {
            Name = request.Name,
            ConnectionString = request.DbConnectionString,
            DatabaseSchema = dbSchema
        };

        await _flexReportDbContext.Customers.AddAsync(customer);
        await _flexReportDbContext.SaveChangesAsync(CancellationToken.None);
            
        return customer.Id;
    }
}