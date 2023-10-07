namespace FlexReport.Application.Integrations.DataAccess;

public interface IDatabaseRecord
{
    IEnumerable<string> GetValues();
}
