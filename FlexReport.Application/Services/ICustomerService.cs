using FlexReport.Application.Models.Requests;

namespace FlexReport.Application.Services;

public interface ICustomerService
{
    Task<int> SynchronizeCustomer(SynchronizeCustomerRequest request);
}