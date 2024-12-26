using MediatR;
using Microsoft.AspNetCore.Authorization;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Queries.Request;

namespace RentalCar.Model.API.Endpoints;

public static class ModelEndPoint
{
    public static void MapModelEndPoints(this IEndpointRouteBuilder route)
    {
        //Get All Models
        route.MapGet("/model", [Authorize(Roles = "Admin")] async (IMediator mediator, CancellationToken cancellationToken, int pageNumber = 1, int pageSize = 10) =>
        {
            var results = await mediator.Send(new FindAllModelsRequest(pageNumber, pageSize), cancellationToken);
            return Results.Ok(results);
        }).WithOpenApi();

        // Get Model By Id
        route.MapGet("/model/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new FindModelByIdRequest(id), cancellationToken);
            return result.Succeeded ? Results.Ok(result) : Results.NotFound(result.Message);
        }).WithOpenApi();

        // Create Model
        route.MapPost("/model", [Authorize(Roles = "Admin")] async (CreateModelRequest request, IMediator mediator, CancellationToken cancellationToken) => 
        {
            var result = await mediator.Send(request, cancellationToken);
            return result.Succeeded ? Results.Created("", result.Message) : Results.BadRequest(result.Message);
        }).WithOpenApi();

        // Update Model
        route.MapPut("/model/{id}", [Authorize(Roles = "Admin")] async (string id, UpdateModelRequest request, IMediator mediator, CancellationToken cancellationToken) =>
        {
            request.Id = id;
            var result = await mediator.Send(request, cancellationToken);
            return result.Succeeded ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
        }).WithOpenApi();

        // Delete Model
        route.MapDelete("/model/{id}", [Authorize(Roles = "Admin")] async (string id, IMediator mediator, CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(new DeleteModelRequest(id), cancellationToken);
            return result.Succeeded ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
        }).WithOpenApi();

    }
}