using FluentAssertions;
using Moq;
using RentalCar.Model.Application.Handlers;
using RentalCar.Model.Application.Queries.Request;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.UnitTest.Application.Queries;

public class FindAllModelsHandlerTest
{
    [Fact]
    public async void FindAllModels_Executed_Return_List_FindAllModelsResponse()
    {
        // Arrange
        var repositoryMock = new Mock<IModelRepository>();
        var loggerServiceMock = new Mock<ILoggerService>();
        var prometheusServiceMock = new Mock<IPrometheusService>();

        var models = new List<Models>
        {
            new Models
            {
                Id = "12345",
                Name = "Teste 1",
                Year = 2022,
                Type = "SUV",
                Motor = Motor.Diesel,
                Transmission = Transmission.Automatic
            },
            new Models
            {
                Id = "12345",
                Name = "Teste 2",
                Year = 2022,
                Type = "SUV",
                Motor = Motor.Diesel,
                Transmission = Transmission.Automatic
            },
            new Models
            {
                Id = "12345",
                Name = "Teste 3",
                Year = 2022,
                Type = "SUV",
                Motor = Motor.Diesel,
                Transmission = Transmission.Automatic
            }
        };

        repositoryMock.Setup(x => x.GetAll(It.IsAny<int>(), It.IsAny<int>(),It.IsAny<CancellationToken>())).ReturnsAsync(models);
        
        var findAllModelsHandler = new FindAllModelsHandler(repositoryMock.Object, loggerServiceMock.Object, prometheusServiceMock.Object);
        
        // Act
        var result = await findAllModelsHandler.Handle(new FindAllModelsRequest(1, 5), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Message.Should().NotBeNullOrEmpty();
        result.Datas.Count.Should().Be(models.Count);
        
        repositoryMock.Verify(repo => repo.GetAll(1, 5, It.IsAny<CancellationToken>()), Times.Once);
    }
}