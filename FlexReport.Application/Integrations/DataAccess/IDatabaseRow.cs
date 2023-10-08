namespace FlexReport.Application.Integrations.DataAccess;

public interface IDatabaseRow
{
    IEnumerable<string?> GetValues();
}
