namespace RentalCar.Model.Application.Queries.Response;

public record FindModelByIdResponse(string Id, string Name, string Motor, string Transmission, string Category, string Manufacturer, string CreatedAt, string Status);