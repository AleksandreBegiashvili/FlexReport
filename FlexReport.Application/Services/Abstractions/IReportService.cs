using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;

namespace FlexReport.Application.Services.Abstractions;

public interface IReportService
{
    Task<CreateReportResponse> CreateReport(CreateReportRequest request);
    Task<IEnumerable<IEnumerable<string>>> ExecuteReport(int customerId, int reportId);
}
