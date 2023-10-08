namespace FlexReport.Application.Models.Requests;

public record ExecuteReportRequest(int CustomerId, int ReportId, int Page, int PageSize);