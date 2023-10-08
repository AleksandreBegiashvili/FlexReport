namespace FlexReport.Application.Common.Interfaces;

public interface IDbSchemaGenerator
{
    string Generate(string connectionString);
}