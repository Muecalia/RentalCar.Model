using MediatR;
using RentalCar.Model.Application.Commands.Response;
using RentalCar.Model.Core.Wrappers;

namespace RentalCar.Model.Application.Commands.Request;
public class UpdateModelRequest : IRequest<ApiResponse<string>>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Year { get; set; } = 0;
    public string IdCategory { get; set; } = string.Empty;
    public string IdManufacturer { get; set; } = string.Empty;
    public char Motor { get; set; } = 'G';
    public char Transmission { get; set; } = 'M';
}

