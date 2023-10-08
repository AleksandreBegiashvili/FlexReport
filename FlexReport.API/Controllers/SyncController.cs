using FlexReport.Application.Customers.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/sync")]
[ApiController]
public class SyncController : ControllerBase
{
    private readonly IMediator _mediator;

    public SyncController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("customer")]
    public async Task<IActionResult> SynchronizeCustomer(SynchronizeCustomerCommand command) =>
        Ok(await _mediator.Send(command));
}