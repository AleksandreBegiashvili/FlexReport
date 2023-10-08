namespace FlexReport.Application.Customers.Commands;

public record SynchronizeCustomerDto
{
    public int CustomerId { get; init; }
}