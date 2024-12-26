using RentalCar.Model.Core.Enuns;

namespace RentalCar.Model.Core.Entities;

public class Models
{
    public Models()
    {
        IsDeleted = false;
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.UtcNow;
        Status = Status.Pending;
    }

    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    public string? IdManufacturer { get; set; }
    public string? IdCategory { get; set; }
    public Transmission Transmission { get; set; }
    public Motor Motor { get; set; }
    public string Type { get; set; }
    public Status Status { get; set; }
}