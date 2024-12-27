using FluentAssertions;
using Moq;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Handlers;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.UnitTest.Application.Commands;

public class UpdadeModelHandlerTest
{
    [Fact]
    public async void UpdateModel_Executed_Return_String()
    {
        // Arrange
        var repositoryMock = new Mock<IModelRepository>();
        var loggerServiceMock = new Mock<ILoggerService>();
        var prometheusServiceMock = new Mock<IPrometheusService>();
        var rabbitMqServiceMock = new Mock<IRabbitMqService>();

        var updateModelRequest = new UpdateModelRequest
        {
            Id = "12345",
            Name = "Teste Atualizado",
            Year = 2023,
            Type = "SUV",
            Motor = 'G',
            Transmission = 'M'
        };

        var models = new Models
        {
            Id = "12345",
            Name = "Teste Atualizado",
            Year = 2023,
            Type = "SUV",
            Motor = Motor.Diesel,
            Transmission = Transmission.Automatic
        };
        
        repositoryMock.Setup(x => x.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(models);
        repositoryMock.Setup(x => x.Update(It.IsAny<Models>(), It.IsAny<CancellationToken>()));
        
        var updateModelHandler = new UpdadeModelHandler(repositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object, rabbitMqServiceMock.Object);

        // Act
        var result = await updateModelHandler.Handle(updateModelRequest, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
        
        repositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repo => repo.Update(It.IsAny<Models>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}