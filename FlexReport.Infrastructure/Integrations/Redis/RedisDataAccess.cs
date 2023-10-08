using FlexReport.Application.Integrations.DataAccess;
using StackExchange.Redis;

namespace FlexReport.Infrastructure.Integrations.Redis;

public class RedisDataAccess : IDataAccess
{
    public RedisDataAccess()
    {

    }

    public Task<IEnumerable<IDatabaseRow>> GetData(string connectionString, string query)
    {
        var redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
        var redisDb = redisConnection.GetDatabase();

        var hash = new HashEntry[] {
            new HashEntry("name", "John"),
            new HashEntry("surname", "Smith"),
            new HashEntry("company", "Redis"),
            new HashEntry("age", "29"),
        };

        redisDb.HashSet("user-session:123", hash);

        var hashFields = redisDb.HashGetAll("user-session:123");
        Console.WriteLine(String.Join("; ", hashFields));

        return null;
    }
}
