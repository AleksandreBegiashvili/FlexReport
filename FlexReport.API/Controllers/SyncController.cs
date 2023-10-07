using FlexReport.API.Models.Requests;
using FlexReport.Application.Models.Requests;
using FlexReport.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/sync")]
[ApiController]
public class SyncController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public SyncController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost("customer")]
    public IActionResult SynchronizeCustomer([FromBody] SyncRequest request)
    {
        var customerId = _customerService.SynchronizeCustomer(
            new SynchronizeCustomerRequest(
                request.Name,
                request.DbConnectionString));

        return Ok(new
        {
            CusomterId = customerId
        });
    }
}