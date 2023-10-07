namespace FlexReport.Application.Models.Requests;

public record CreateReportRequest(
    int CustomerId,
    string Prompt);
