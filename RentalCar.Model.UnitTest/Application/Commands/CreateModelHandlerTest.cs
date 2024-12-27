using FluentAssertions;
using Moq;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Handlers;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.UnitTest.Application.Commands;

public class CreateModelHandlerTest
{
    [Fact]
    public async void CreateModel_Executed_Return_String()
    {
        // Arrange
        var repositoryMock = new Mock<IModelRepository>();
        var loggerServiceMock = new Mock<ILoggerService>();
        var prometheusServiceMock = new Mock<IPrometheusService>();
        var rabbitMqServiceMock = new Mock<IRabbitMqService>();

        var createModelRequest = new CreateModelRequest
        {
            Name = "Teste",
            Year = 2022,
            Type = "SUV",
            IdCategory = "12345",
            IdManufacturer = "12345",
            Motor = 'G',
            Transmission = 'M'
        };

        var models = new Models
        {
            Id = "12345",
            Name = "Teste",
            Year = 2022,
            Type = "SUV",
            Motor = Motor.Diesel,
            Transmission = Transmission.Automatic
        };
        
        repositoryMock.Setup(x => x.IsModelExist(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        repositoryMock.Setup(x => x.Create(It.IsAny<Models>(), It.IsAny<CancellationToken>())).ReturnsAsync(models);
        
        var requestValidService = new RequestValidService(It.IsAny<string>(), It.IsAny<string>());
        
        var createModelHandler = new CreateModelHandler(repositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object, rabbitMqServiceMock.Object);
        
        // Act
        var result = await createModelHandler.Handle(createModelRequest, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
        
        repositoryMock.Verify(repo => repo.Create(It.IsAny<Models>(), It.IsAny<CancellationToken>()));
    }
}