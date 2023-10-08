namespace FlexReport.Application.Reports.Commands;

public record ReportDto
{
    public int ReportId { get; init; }
    public required string QueryPreview { get; init; }
}