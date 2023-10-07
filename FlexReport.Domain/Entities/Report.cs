namespace FlexReport.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string Query { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;

    public Customer? Customer { get; set; }
}
