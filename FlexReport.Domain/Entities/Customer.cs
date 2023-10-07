namespace FlexReport.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ConnectionString { get; set; }
    public required string DatabaseSchema { get; set; }

    public virtual ICollection<Report>? Reports { get; set; }
}
