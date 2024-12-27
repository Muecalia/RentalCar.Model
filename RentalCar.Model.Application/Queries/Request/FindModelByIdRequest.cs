using MediatR;
using RentalCar.Model.Core.Wrappers;
using RentalCar.Model.Application.Queries.Response;

namespace RentalCar.Model.Application.Queries.Request
{
    public class FindModelByIdRequest(string id) : IRequest<ApiResponse<FindModelByIdResponse>>
    {
        public string Id { get; set; } = id;
    }
}
