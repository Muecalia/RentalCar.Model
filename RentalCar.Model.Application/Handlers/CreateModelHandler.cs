using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Core.Configs;
using RentalCar.Model.Core.Entities;
using RentalCar.Model.Core.MessageBus;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Core.Wrappers;
using RentalCar.Model.Infrastructure.Services;

namespace RentalCar.Model.Application.Handlers;

public class CreateModelHandler : IRequestHandler<CreateModelRequest, ApiResponse<string>>
{
    private readonly IModelRepository _repository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;
    private readonly IRabbitMqService _rabbitMqService;

    public CreateModelHandler(IModelRepository repository, ILoggerService loggerService, IPrometheusService prometheusService, IRabbitMqService rabbitMqService)
    {
        _repository = repository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<ApiResponse<string>> Handle(CreateModelRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "modelo";
        const string Operacao = "registo";
        try
        {
            if (await _repository.IsModelExist(request.Name, cancellationToken))
            {
                _loggerService.LogWarning(MessageError.Conflito($"{Objecto} {request.Name}"));
                _prometheusService.AddNewModelCounter(StatusCodes.Status409Conflict.ToString());
                return ApiResponse<string>.Error(MessageError.Conflito(Objecto));
            }

            var newModel = new Models
            {
                Name = request.Name,
                Year = request.Year,
                Type = request.Type,
                Motor = EnunsServices.GetMotor(request.Motor),
                Transmission = EnunsServices.GetTransmission(request.Transmission)
            };

            var model = await _repository.Create(newModel, cancellationToken);
            
            var category = new RequestValidService(model.Id, request.IdCategory);
            var manufacturer = new RequestValidService(model.Id, request.IdManufacturer);
                
            await _rabbitMqService.PublishMessage(category, RabbitQueue.CATEGORY_MODEL_NEW_REQUEST_QUEUE, cancellationToken);
            await _rabbitMqService.PublishMessage(manufacturer, RabbitQueue.MANUFACTURER_MODEL_NEW_REQUEST_QUEUE, cancellationToken);

            _prometheusService.AddNewModelCounter(StatusCodes.Status201Created.ToString());
            return ApiResponse<string>.Success(Objecto, MessageError.OperacaoProcessamento(Objecto, Operacao));
        }
        catch (Exception ex)
        {
            _prometheusService.AddNewModelCounter(StatusCodes.Status400BadRequest.ToString());
            _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
            return ApiResponse<string>.Error(MessageError.OperacaoErro(Objecto, Operacao));
        }
    }
}
