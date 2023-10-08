using FlexReport.Application.Common.Interfaces.External;

namespace FlexReport.Application.Common.Models;

public record GetDataRequest(
    string ConnectionString,
    string Query,
    int Page,
    int PageSize);

public record GetDataResponse(
    IEnumerable<string?> Headers,
    IEnumerable<IDatabaseRow> Data,
    int TotalCount);