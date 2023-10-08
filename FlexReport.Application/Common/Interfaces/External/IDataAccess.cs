using FlexReport.Application.Common.Models;

namespace FlexReport.Application.Common.Interfaces.External;

public interface IDataAccess
{
    Task<GetDataResponse> GetData(GetDataRequest request);
}