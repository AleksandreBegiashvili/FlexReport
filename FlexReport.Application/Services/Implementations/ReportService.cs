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
    private readonly IDataAccess _dataAccess;
    private readonly IFlexReportDbContext _flexReportDbContext;
    private readonly IOpenAIClient _openAIClient;

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
        await _flexReportDbContext.SaveChangesAsync(CancellationToken.None);

        return new CreateReportResponse(report.Id, queryResponse);
    }

    public async Task<ExecuteReportResponse> ExecuteReport(ExecuteReportRequest request)
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

        return new ExecuteReportResponse(
            result.Headers,
            result.Data.Select(res => res.GetValues()),
            result.TotalCount);
    }
}