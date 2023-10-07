using FlexReport.Application.Models.Requests;

namespace FlexReport.Application.Services.Abstractions;

public interface ICustomerService
{
    Task<int> SynchronizeCustomer(SynchronizeCustomerRequest request);
}