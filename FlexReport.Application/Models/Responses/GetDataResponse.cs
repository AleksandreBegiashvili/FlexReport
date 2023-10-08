using FlexReport.Application.Integrations.DataAccess;

namespace FlexReport.Application.Models.Responses;

public record GetDataResponse(IEnumerable<IDatabaseRow> Data, int TotalCount);