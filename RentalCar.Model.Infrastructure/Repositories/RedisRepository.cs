using System.Text.Json;
using RentalCar.Model.Core.Repositories;
using StackExchange.Redis;

namespace RentalCar.Model.Infrastructure.Repositories;

public class RedisRepository : IRedisRepository
{
    private readonly IDatabase _redis;

    public RedisRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task CreateService<T>(string key, T value)
    {
        var expirationTime = DateTime.Now.AddDays(2);
        await _redis.StringSetAsync(key, JsonSerializer.Serialize(value));
        await _redis.KeyExpireAsync(key, expirationTime);
    }

    public async Task<string> GetService<T>(string key)
    {;
        var json = await _redis.StringGetAsync(key);

        var result = JsonSerializer.Deserialize<string>(json);
        
        return result ?? string.Empty;
    }
}