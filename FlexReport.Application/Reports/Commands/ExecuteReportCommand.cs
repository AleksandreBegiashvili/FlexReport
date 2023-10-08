using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Common.Interfaces.External;
using FlexReport.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Application.Reports.Commands;

public record ExecuteReportCommand : IRequest<ExecutedReportDto>
{
    public int CustomerId { get; init; }
    public int ReportId { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}

public class ExecuteReportCommandHandler : IRequestHandler<ExecuteReportCommand, ExecutedReportDto>
{
    private readonly IDataAccess _dataAccess;
    private readonly IFlexReportDbContext _flexReportDbContext;

    public ExecuteReportCommandHandler(IDataAccess dataAccess, IFlexReportDbContext flexReportDbContext)
    {
        _dataAccess = dataAccess;
        _flexReportDbContext = flexReportDbContext;
    }

    public async Task<ExecutedReportDto> Handle(ExecuteReportCommand request, CancellationToken cancellationToken)
    {
        var customer = _flexReportDbContext.Customers
                           .Include(c => c.Reports)
                           .FirstOrDefault(c => c.Id == request.CustomerId)
                       ?? throw new Exception("Customer was not found");

        var report = customer.Reports!.FirstOrDefault(r => r.Id == request.ReportId)
                     ?? throw new Exception("Report not found for the specified customer");

        var result = await _dataAccess.GetData(new GetDataRequest(
            customer.ConnectionString,
            report.Query,
            request.Page,
            request.PageSize));

        return new ExecutedReportDto
        {
            Headers = result.Headers,
            Data = result.Data.Select(res => res.GetValues()),
            TotalCount = result.TotalCount
        };
    }
}