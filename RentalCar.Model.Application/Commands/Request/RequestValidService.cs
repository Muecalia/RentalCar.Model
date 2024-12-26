namespace RentalCar.Model.Application.Commands.Request;

public class RequestValidService(string idModel, string idService)
{
    public string IdModel { get; set; } = idModel;
    public string IdService { get; set; } = idService;
}