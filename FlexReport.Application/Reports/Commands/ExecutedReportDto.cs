namespace FlexReport.Application.Reports.Commands;

public record ExecutedReportDto
{
    public required IEnumerable<string?> Headers { get; init; } 
    public required IEnumerable<IEnumerable<string?>> Data { get; init; }
    public int TotalCount { get; init; }
}