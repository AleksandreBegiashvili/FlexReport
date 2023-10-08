namespace FlexReport.Application.Integrations.DataAccess;

public interface IDataAccess
{
    Task<IEnumerable<IDatabaseRow>> GetData(string connectionString, string query);
}
