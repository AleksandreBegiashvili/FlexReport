using FlexReport.Application.Reports.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ReportDto>> CreateReport(CreateReportCommand command) =>
        Ok(await _mediator.Send(command));

    [HttpPost("execute")]
    public async Task<ActionResult<ExecutedReportDto>> ExecuteReport(ExecuteReportCommand command) =>
        Ok(await _mediator.Send(command));
}