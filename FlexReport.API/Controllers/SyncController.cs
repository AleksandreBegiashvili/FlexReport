using FlexReport.API.Models.Requests;
using FlexReport.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SyncController : ControllerBase
{
    private readonly IDbSchemaGenerator _dbSchemaGenerator;

    public SyncController(IDbSchemaGenerator dbSchemaGenerator)
    {
        _dbSchemaGenerator = dbSchemaGenerator;
    }

    [HttpPost]
    public IActionResult Sync([FromBody] SyncRequest request)
    {
        var dbSchema = _dbSchemaGenerator.Generate(request.DbConnectionString);

        return Ok(dbSchema);
    }
}