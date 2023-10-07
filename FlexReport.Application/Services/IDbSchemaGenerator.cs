namespace FlexReport.Application.Services;

public interface IDbSchemaGenerator
{
    string Generate(string connectionString, string databaseName);
}