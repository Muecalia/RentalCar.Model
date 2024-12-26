using RentalCar.Model.Core.Entities;

namespace RentalCar.Model.Core.Repositories;

public interface IRedisRepository
{
    Task CreateService<T>(string key, T value);
    Task<string> GetService<T>(string key);
}