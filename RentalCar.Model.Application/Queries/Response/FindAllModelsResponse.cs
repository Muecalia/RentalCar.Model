namespace RentalCar.Model.Application.Queries.Response;

public record FindAllModelsResponse(string Id, string Name, string Motor, string Transmission, string CreatedAt, string Status);

