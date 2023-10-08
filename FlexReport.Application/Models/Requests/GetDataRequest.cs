namespace FlexReport.Application.Models.Requests;

public record GetDataRequest(string ConnectionString, string Query, int Page, int PageSize);