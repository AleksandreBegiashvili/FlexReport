namespace FlexReport.Application.Integrations.DataAccess;

public interface IDataAccess
{
    Task<IEnumerable<IDatabaseRecord>> GetData(string connectionString, string query);
}
