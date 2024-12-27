using FluentAssertions;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;

namespace RentalCar.Model.UnitTest.Core.Entities;

public class ModelTest
{
    [Fact]
    public void CreateModel()
    {
        // Arrange & Act
        var model = new Models
        {
            Id = "12345",
            Name = "Teste",
            Motor = Motor.Diesel,
            Transmission = Transmission.Automatic,
            IdCategory = "1234",
            IdManufacturer = "125632",
            Status = Status.Created,
            CreatedAt = DateTime.Now
        };
        
        // Assert
        
        model.Should().NotBeNull();
        model.Id.Should().NotBeNullOrEmpty();
        model.Name.Should().NotBeNullOrEmpty();
        model.CreatedAt.ToShortDateString().Should().Be(DateTime.Now.ToShortDateString());
    }
}