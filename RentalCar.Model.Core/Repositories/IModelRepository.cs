using RentalCar.Model.Core.Entities;

namespace RentalCar.Model.Core.Repositories;

public interface IModelRepository
{
    Task<Models> Create(Models model, CancellationToken cancellationToken);
    Task Update(Models model, CancellationToken cancellationToken);
    Task UpdateStatus(CancellationToken cancellationToken);
    Task Delete(Models model, CancellationToken cancellationToken);
    Task<bool> IsModelExist(string name, CancellationToken cancellationToken);
    Task<Models?> GetById(string id, CancellationToken cancellationToken);
    Task<List<Models>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken);
}