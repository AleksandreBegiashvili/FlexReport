namespace FlexReport.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public required string Query { get; set; }
    public required string Prompt { get; set; }

    public virtual Customer? Customer { get; set; }
}
