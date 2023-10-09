using FlexReport.Application.Common.Interfaces;
using FlexReport.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Application.Customers.Commands;

public record SynchronizeCustomerCommand : IRequest<SynchronizeCustomerDto>
{
    public required string Name { get; set; }
    public required string DbConnectionString { get; set; }
}

public class SynchronizeCustomerCommandHandler : IRequestHandler<SynchronizeCustomerCommand, SynchronizeCustomerDto>
{
    private readonly IFlexReportDbContext _flexReportDbContext;
    private readonly IDbSchemaGenerator _dbSchemaGenerator;

    public SynchronizeCustomerCommandHandler(IFlexReportDbContext flexReportDbContext, IDbSchemaGenerator dbSchemaGenerator)
    {
        _flexReportDbContext = flexReportDbContext;
        _dbSchemaGenerator = dbSchemaGenerator;
    }

    public async Task<SynchronizeCustomerDto> Handle(SynchronizeCustomerCommand request, CancellationToken cancellationToken)
    {
        var dbSchema = _dbSchemaGenerator.Generate(request.DbConnectionString);

        var existsUser = _flexReportDbContext.Customers.Any(c => c.Name == request.Name);
        
        if (existsUser)
        {
            throw new InvalidOperationException("Customer already exists");
        }
        
        var customer = new Customer
        {
            Name = request.Name,
            ConnectionString = request.DbConnectionString,
            DatabaseSchema = dbSchema
        };

        await _flexReportDbContext.Customers.AddAsync(customer, cancellationToken);
        await _flexReportDbContext.SaveChangesAsync(cancellationToken);

        return new SynchronizeCustomerDto
        {
            CustomerId = customer.Id
        };
    }
}