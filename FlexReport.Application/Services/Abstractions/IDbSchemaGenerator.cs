namespace FlexReport.Application.Services.Abstractions;

public interface IDbSchemaGenerator
{
    string Generate(string connectionString);
}