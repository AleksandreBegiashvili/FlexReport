namespace FlexReport.Application.Common.Interfaces.External;

public interface IDatabaseRow
{
    IEnumerable<string?> GetValues();
}
