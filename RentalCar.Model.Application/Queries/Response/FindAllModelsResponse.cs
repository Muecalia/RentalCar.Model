namespace RentalCar.Model.Application.Queries.Response;

public record FindModelResponse(string Id, string Name, string Motor, string Transmission, string CreatedAt, string Status);

