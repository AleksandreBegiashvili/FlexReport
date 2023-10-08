using FluentValidation;

namespace FlexReport.Application.Customers.Commands;

public class SynchronizeCustomerCommandValidator : AbstractValidator<SynchronizeCustomerCommand>
{
    public SynchronizeCustomerCommandValidator()
    {
        RuleFor(v => v.DbConnectionString)
            .Matches(@"^Server=tcp:([a-zA-Z0-9.-]+),\d+;Initial Catalog=([a-zA-Z0-9_]+);Persist Security Info=[a-zA-Z]+;User ID=[a-zA-Z0-9-]+;Password=[a-zA-Z0-9]+;MultipleActiveResultSets=[a-zA-Z]+;Encrypt=[a-zA-Z]+;TrustServerCertificate=[a-zA-Z]+;Connection Timeout=\d+;$")
            .WithMessage("Invalid connection string format.");
    }
}