using FlexReport.Application.Common.Exceptions;
using FlexReport.Application.Common.Interfaces;
using FlexReport.Application.Common.Interfaces.External;
using FlexReport.Domain.Entities;
using MediatR;

namespace FlexReport.Application.Reports.Commands;

public record CreateReportCommand : IRequest<ReportDto>
{
    public int CustomerId { get; init; }
    public required string Prompt { get; init; }
}

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, ReportDto>
{
    private readonly IFlexReportDbContext _flexReportDbContext;
    private readonly IOpenAIClient _openAIClient;

    public CreateReportCommandHandler(IFlexReportDbContext flexReportDbContext, IOpenAIClient openAiClient)
    {
        _flexReportDbContext = flexReportDbContext;
        _openAIClient = openAiClient;
    }

    public async Task<ReportDto> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var customer = _flexReportDbContext.Customers.FirstOrDefault(c => c.Id == request.CustomerId)
                       ?? throw new NotFoundException("Customer was not found");

        var schema = customer.DatabaseSchema;

        var queryResponse = await _openAIClient.SendMessage(schema, request.Prompt);

        var report = new Report
        {
            CustomerId = request.CustomerId,
            Prompt = request.Prompt,
            Query = queryResponse
        };

        _flexReportDbContext.Reports.Add(report);
        await _flexReportDbContext.SaveChangesAsync(CancellationToken.None);

        return new ReportDto
        {
            ReportId = report.Id,
            QueryPreview = queryResponse
        };
    }
}