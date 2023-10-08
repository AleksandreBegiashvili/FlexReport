using FluentValidation;

namespace FlexReport.Application.Reports.Commands;

public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
{
    public CreateReportCommandValidator()
    {
        RuleFor(v => v.Prompt)
            .MinimumLength(10).WithMessage("Prompt should be larger.");
    }
}