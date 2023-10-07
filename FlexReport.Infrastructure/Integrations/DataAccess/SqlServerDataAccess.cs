using FlexReport.Application.Integrations.DataAccess;
using Microsoft.Data.SqlClient;

namespace FlexReport.Infrastructure.Integrations.DataAccess;

public class SqlServerDataAccess : IDataAccess
{
    public async Task<IEnumerable<IDatabaseRecord>> GetData(string connectionString, string query)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();

        using var command = new SqlCommand(query, connection);
        using var dataReader = await command.ExecuteReaderAsync();

        var data = new List<SqlServerDataRecord>();

        while (await dataReader.ReadAsync())
        {
            var columnCount = dataReader.VisibleFieldCount;
            var rowValues = new List<string>();

            for (var i = 0; i < columnCount; i++)
            {
                var fieldValue = dataReader.GetFieldValue<object>(i);
                var stringRepresentation = fieldValue.ToString();
                rowValues.Add(stringRepresentation);
            }

            data.Add(new SqlServerDataRecord(rowValues));
            rowValues = new List<string>();
        }

        return data;
    }
}
