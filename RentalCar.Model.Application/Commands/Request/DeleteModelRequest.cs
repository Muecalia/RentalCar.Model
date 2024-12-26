using MediatR;
using RentalCar.Model.Application.Commands.Response;
using RentalCar.Model.Core.Wrappers;

namespace RentalCar.Model.Application.Commands.Request;
public class DeleteModelRequest(string id) : IRequest<ApiResponse<string>>
{
    public string Id { get; set; } = id;
}

