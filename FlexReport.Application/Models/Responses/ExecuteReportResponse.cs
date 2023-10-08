namespace FlexReport.Application.Models.Responses;

public record ExecuteReportResponse(IEnumerable<IEnumerable<string>> Data, int TotalCount);