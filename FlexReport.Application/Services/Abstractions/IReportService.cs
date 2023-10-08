using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;

namespace FlexReport.Application.Services.Abstractions;

public interface IReportService
{
    Task<CreateReportResponse> CreateReport(CreateReportRequest request);
    Task<ExecuteReportResponse> ExecuteReport(ExecuteReportRequest request);
}