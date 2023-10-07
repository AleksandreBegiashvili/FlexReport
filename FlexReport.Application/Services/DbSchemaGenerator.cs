using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace FlexReport.Application.Services;

public class DbSchemaGenerator : IDbSchemaGenerator
{
    public string Generate(string connectionString)
    {
        using var sqlConnection = new SqlConnection(connectionString);

        var server = new Server(new ServerConnection(sqlConnection));

        var databaseName = ExtractDbName(connectionString);

        var database = server.Databases[databaseName];

        var scripter = new Scripter(server)
        {
            Options =
            {
                ScriptDrops = false,
                WithDependencies = true,
                Indexes = true,
                DriAllConstraints = true
            }
        };

        var urns = database.Tables
            .OfType<Table>()
            .Where(t => !t.IsSystemObject)
            .Select(t => t.Urn)
            .ToArray();

        var dbSchema = string.Join('\n', scripter.Script(urns).OfType<string>());

        return dbSchema;
    }

    private static string ExtractDbName(string connectionString)
        => new SqlConnectionStringBuilder(connectionString).InitialCatalog;
}