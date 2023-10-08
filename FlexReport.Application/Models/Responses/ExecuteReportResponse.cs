namespace FlexReport.Application.Models.Responses;

public record ExecuteReportResponse(
    IEnumerable<string?> Headers,
    IEnumerable<IEnumerable<string?>> Data,
    int TotalCount);