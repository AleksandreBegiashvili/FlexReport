namespace FlexReport.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseSchema { get; set; } = string.Empty;

    public ICollection<Report>? Reports { get; set; }
}
