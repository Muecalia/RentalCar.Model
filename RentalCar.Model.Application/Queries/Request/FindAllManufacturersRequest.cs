using MediatR;
using RentalCar.Model.Application.Queries.Response;
using RentalCar.Model.Core.Wrappers;

namespace RentalCar.Model.Application.Queries.Request;

public class FindAllModelsRequest(int pageNumber, int pageSize) : IRequest<PagedResponse<FindModelResponse>>
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}

