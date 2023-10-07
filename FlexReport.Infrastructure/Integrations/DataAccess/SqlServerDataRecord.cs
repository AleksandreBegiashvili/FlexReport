using FlexReport.Application.Integrations.DataAccess;

namespace FlexReport.Infrastructure.Integrations.DataAccess;

public class SqlServerDataRecord : IDatabaseRecord
{
    private readonly List<string> _rowValues;

    public SqlServerDataRecord(List<string> rowValues)
    {
        _rowValues = rowValues;
    }

    public IEnumerable<string> GetValues() => _rowValues;
}
