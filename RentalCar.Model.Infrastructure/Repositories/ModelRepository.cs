using Microsoft.EntityFrameworkCore;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Infrastructure.Persistence;

namespace RentalCar.Model.Infrastructure.Repositories;

public class ModelRepository : IModelRepository
{
    private readonly ModelContext _context;

    public ModelRepository(ModelContext context)
    {
        _context = context;
    }

    public async Task<Models> Create(Models model, CancellationToken cancellationToken)
    {
        _context.Models.Add(model);
        await _context.SaveChangesAsync(cancellationToken);
        return model;
    }

    public async Task UpdateStatus(CancellationToken cancellationToken)
    {
        await _context.Models
            .Where(m => !string.IsNullOrEmpty(m.IdCategory) && !string.IsNullOrEmpty(m.IdManufacturer) && m.Status == Status.Pending)
            .ExecuteUpdateAsync(s => s.SetProperty(m => m.Status, Status.Created), cancellationToken);
    }

    public async Task Delete(Models model, CancellationToken cancellationToken)
    {
        model.DeletedAt = DateTime.UtcNow;
        _context.Update(model);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Models>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Models
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Models?> GetById(string id, CancellationToken cancellationToken)
    {
        return await _context.Models.FirstOrDefaultAsync(c => !c.IsDeleted && string.Equals(c.Id, id), cancellationToken);
    }

    public async Task<bool> IsModelExist(string name, CancellationToken cancellationToken)
    {
        return await _context.Models.AnyAsync(c => string.Equals(c.Name, name), cancellationToken);
    }

    public async Task Update(Models model, CancellationToken cancellationToken)
    {
        model.UpdatedAt = DateTime.UtcNow;
        _context.Models.Update(model);
        await _context.SaveChangesAsync(cancellationToken);
    }
}