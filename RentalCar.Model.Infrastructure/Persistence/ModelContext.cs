using Microsoft.EntityFrameworkCore;
using RentalCar.Model.Core.Entities;

namespace RentalCar.Model.Infrastructure.Persistence;

public class ModelContext : DbContext
{
    public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }
    
    public DbSet<Models> Models { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Models>(e => 
        {
            e.HasKey(c => c.Id);
            
            e.Property<string>(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);
            
            e.Property<string>(c => c.IdCategory)
                .HasMaxLength(50);
            
            e.Property<string>(c => c.IdManufacturer)
                .HasMaxLength(50);
            
            e.Property<string>(c => c.Type)
                .HasMaxLength(50);
            
            e.HasIndex(c => c.Name).IsUnique();
            e.HasIndex(c => c.IdCategory);
            e.HasIndex(c => c.IdManufacturer);
        });

        base.OnModelCreating(builder);
    }
}