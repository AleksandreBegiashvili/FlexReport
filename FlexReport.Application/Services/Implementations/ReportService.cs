using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Integrations.DataAccess;
using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;
using FlexReport.Application.Services.Abstractions;
using FlexReport.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlexReport.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IFlexReportDbContext _flexReportDbContext;
    private readonly IOpenAIClient _openAIClient;
    private readonly IDataAccess _dataAccess;

    public ReportService(
        IFlexReportDbContext flexReportDbContext,
        IOpenAIClient openAIClient,
        IDataAccess dataAccess)
    {
        _flexReportDbContext = flexReportDbContext;
        _openAIClient = openAIClient;
        _dataAccess = dataAccess;
    }

    public async Task<CreateReportResponse> CreateReport(CreateReportRequest request)
    {
        var customer = _flexReportDbContext.Customers.FirstOrDefault(c => c.Id == request.CustomerId)
            ?? throw new Exception("Customer was not found");

        var schema = customer.DatabaseSchema;

        var queryResponse = await _openAIClient.SendMessage(schema, request.Prompt);

        var report = new Report
        {
            CustomerId = request.CustomerId,
            Prompt = request.Prompt,
            Query = queryResponse
        };

        _flexReportDbContext.Reports.Add(report);
        var newReportId = await _flexReportDbContext.SaveChangesAsync(CancellationToken.None);

        return new CreateReportResponse(newReportId, queryResponse);
    }

    public async Task<IEnumerable<IEnumerable<string>>> ExecuteReport(int customerId, int reportId)
    {
        var customer = _flexReportDbContext.Customers
            .Include(c => c.Reports)
            .FirstOrDefault(c => c.Id == customerId)
            ?? throw new Exception("Customer was not found");

        var report = customer.Reports!.FirstOrDefault(r => r.Id == reportId)
            ?? throw new Exception("Report not found for the specified customer");

        var result = await _dataAccess.GetData(customer.ConnectionString, report.Query);

        return result.Select(res => res.GetValues());
    }
}
