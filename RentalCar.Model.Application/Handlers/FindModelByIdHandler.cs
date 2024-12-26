using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Model.Application.Queries.Request;
using RentalCar.Model.Application.Queries.Response;
using RentalCar.Model.Core.Configs;
using RentalCar.Model.Core.MessageBus;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Core.Wrappers;
using RentalCar.Model.Infrastructure.Services;

namespace RentalCar.Model.Application.Handlers;

public class FindModelByIdHandler : IRequestHandler<FindModelByIdRequest, ApiResponse<FindModelByIdResponse>>
{
    private readonly IModelRepository _repository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IModelService _modelService;

    public FindModelByIdHandler(IModelRepository repository, ILoggerService loggerService, IPrometheusService prometheusService, IRabbitMqService rabbitMqService, IModelService modelService)
    {
        _repository = repository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
        _rabbitMqService = rabbitMqService;
        _modelService = modelService;
    }

    public async Task<ApiResponse<FindModelByIdResponse>> Handle(FindModelByIdRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "modelo";
        try
        {
            var model = await _repository.GetById(request.Id, cancellationToken);
            if (model is null)
            {
                _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                _prometheusService.AddFindByIdModelCounter(StatusCodes.Status404NotFound.ToString());
                return ApiResponse<FindModelByIdResponse>.Error(MessageError.NotFound(Objecto));
            }

            await _rabbitMqService.PublishMessage(model.IdCategory, RabbitQueue.FIND_CATEGORY_MODEL_REQUEST_QUEUE, cancellationToken);
            await _rabbitMqService.PublishMessage(model.IdManufacturer, RabbitQueue.FIND_MANUFACTURER_MODEL_REQUEST_QUEUE, cancellationToken);
            
            var category = await _modelService.GetService(model.IdCategory, RabbitQueue.FIND_CATEGORY_MODEL_RESPONSE_QUEUE, cancellationToken);
            var manufacturer = await _modelService.GetService(model.IdManufacturer, RabbitQueue.FIND_MANUFACTURER_MODEL_RESPONSE_QUEUE, cancellationToken);

            var result = new FindModelByIdResponse(model.Id, model.Name, EnunsServices.GetDescriptionMotor(model.Motor), 
                EnunsServices.GetDescriptionTransmission(model.Transmission), category, manufacturer,
                model.CreatedAt.ToShortDateString(), EnunsServices.GetDescriptionStatus(model.Status));
            
            _prometheusService.AddFindByIdModelCounter(StatusCodes.Status200OK.ToString());
            _loggerService.LogInformation(MessageError.CarregamentoSucesso(Objecto, 1));
            return ApiResponse<FindModelByIdResponse>.Success(result, MessageError.CarregamentoSucesso(Objecto));
        }
        catch (Exception ex)
        {
            _prometheusService.AddFindByIdModelCounter(StatusCodes.Status400BadRequest.ToString());
            _loggerService.LogError(MessageError.CarregamentoErro(Objecto, ex.Message));
            return ApiResponse<FindModelByIdResponse>.Error(MessageError.CarregamentoErro(Objecto));
        }
    }
}

