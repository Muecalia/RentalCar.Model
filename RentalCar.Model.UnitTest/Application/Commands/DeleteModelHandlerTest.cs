using FluentAssertions;
using Moq;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Handlers;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.UnitTest.Application.Commands;

public class DeleteModelHandlerTest
{
    [Fact]
    public async void DeleteModel_Executed_Return_String()
    {
        // Arrange
        var repositoryMock = new Mock<IModelRepository>();
        var loggerServiceMock = new Mock<ILoggerService>();
        var prometheusServiceMock = new Mock<IPrometheusService>();
        
        var models = new Models
        {
            Id = "12345",
            Name = "Teste",
            Year = 2022,
            Type = "SUV",
            Motor = Motor.Diesel,
            Transmission = Transmission.Automatic
        };
        
        repositoryMock.Setup(x => x.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(models);
        repositoryMock.Setup(x => x.Delete(It.IsAny<Models>(), It.IsAny<CancellationToken>()));
        
        var deleteModelHandler = new DeleteModelHandler(repositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);
        
        // Act
        var result = await deleteModelHandler.Handle(new DeleteModelRequest("12345"), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
        
        repositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(repo => repo.Delete(It.IsAny<Models>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}