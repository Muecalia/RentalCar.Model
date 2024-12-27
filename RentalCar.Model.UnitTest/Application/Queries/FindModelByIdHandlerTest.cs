using FluentAssertions;
using Moq;
using RentalCar.Model.Application.Handlers;
using RentalCar.Model.Application.Queries.Request;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.UnitTest.Application.Queries;

public class FindModelByIdHandlerTest
{
    [Fact]
    public async void FindModelById_Executed_Return_FindModelByIdResponse()
    {
        // Arrange
        var repositoryMock = new Mock<IModelRepository>();
        var loggerServiceMock = new Mock<ILoggerService>();
        var prometheusServiceMock = new Mock<IPrometheusService>();
        var rabbitMqServiceMock = new Mock<IRabbitMqService>();
        var modelServiceMock = new Mock<IModelService>();

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
        
        var findModelByIdHandler = new FindModelByIdHandler(repositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object, rabbitMqServiceMock.Object, modelServiceMock.Object);
        var findModelByIdRequest = new FindModelByIdRequest("12345");
        
        // Act
        var result = await findModelByIdHandler.Handle(findModelByIdRequest, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Message.Should().NotBeNullOrEmpty();
        result.Succeeded.Should().BeTrue();
        
        repositoryMock.Verify(repo => repo.GetById(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }
}