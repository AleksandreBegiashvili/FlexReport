using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;

namespace FlexReport.Application.Integrations.DataAccess;

public interface IDataAccess
{
    Task<GetDataResponse> GetData(GetDataRequest request);
}