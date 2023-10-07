using FlexReport.API.Models.Requests;
using FlexReport.Application.Models.Requests;
using FlexReport.Application.Models.Responses;
using FlexReport.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/report")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateReportResponse>> CreateReport(CreateReportRequest request)
    {
        var createReportRequest = new CreateReportRequest(request.CustomerId, request.Prompt);
        var result = await _reportService.CreateReport(createReportRequest);

        return Ok(result);
    }

    [HttpPost("Execute")]
    public async Task<ActionResult<object>> ExecuteReport(ExecuteReportRequest request)
    {
        var result = await _reportService.ExecuteReport(request.CustomerId, request.ReportId);

        return Ok(result);
    }
}
