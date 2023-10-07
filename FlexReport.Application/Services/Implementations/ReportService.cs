using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;
using FlexReport.Application.Services.Abstractions;
using FlexReport.Domain.Entities;

namespace FlexReport.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IFlexReportDbContext _flexReportDbContext;
    private readonly IOpenAIClient _openAIClient;

    public ReportService(
        IFlexReportDbContext flexReportDbContext,
        IOpenAIClient openAIClient)
    {
        _flexReportDbContext = flexReportDbContext;
        _openAIClient = openAIClient;
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
}
