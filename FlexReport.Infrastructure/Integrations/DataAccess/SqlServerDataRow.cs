using FlexReport.Application.Common.Interfaces.External;

namespace FlexReport.Infrastructure.Integrations.DataAccess;

public class SqlServerDataRow : IDatabaseRow
{
    private readonly List<string?> _rowValues;

    public SqlServerDataRow(List<string?> rowValues)
    {
        _rowValues = rowValues;
    }

    public IEnumerable<string?> GetValues() => _rowValues;
}
