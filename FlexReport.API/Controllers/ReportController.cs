using FlexReport.Application.Integrations.OpenAI;
using FlexReport.Domain.Entities;
using FlexReport.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace FlexReport.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly FlexReportDbContext _context;
    private readonly IOpenAIClient _openAIClient;

    public ReportController(FlexReportDbContext context, IOpenAIClient openAIClient)
    {
        _context = context;
        _openAIClient = openAIClient;
    }

    [HttpPost("GenerateQuery")]
    public async Task<ActionResult<string>> GenerateQuery(int customerId, string prompt)
    {
        var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId)
            ?? throw new Exception("Customer was not found");

        var schema = customer.DatabaseSchema;

        var queryResponse = await _openAIClient.SendMessage(schema, prompt);

        var report = new Report
        {
            CustomerId = customerId,
            Prompt = prompt,
            Query = queryResponse
        };
        _context.Reports.Add(report);
        await _context.SaveChangesAsync();

        return Ok(queryResponse);
    }
}
