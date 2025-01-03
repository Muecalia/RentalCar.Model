﻿using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Commands.Response;
using RentalCar.Model.Core.Configs;
using RentalCar.Model.Core.MessageBus;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Core.Wrappers;
using RentalCar.Model.Infrastructure.Services;

namespace RentalCar.Model.Application.Handlers;

public class UpdadeModelHandler : IRequestHandler<UpdateModelRequest, ApiResponse<string>>
    {
        private readonly IModelRepository _repository;
        private readonly ILoggerService _loggerService;
        private readonly IPrometheusService _prometheusService;
        private readonly IRabbitMqService _rabbitMqService;

        public UpdadeModelHandler(IModelRepository repository, ILoggerService loggerService, IPrometheusService prometheusService, IRabbitMqService rabbitMqService)
        {
            _repository = repository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
            _rabbitMqService = rabbitMqService;
        }

        public async Task<ApiResponse<string>> Handle(UpdateModelRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "modelo";
            const string Operacao = "editar";
            try
            {
                var model = await _repository.GetById(request.Id, cancellationToken);
                if (model == null)
                {
                    _prometheusService.AddUpdateModelCounter(StatusCodes.Status404NotFound.ToString());
                    _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                    return ApiResponse<string>.Error(MessageError.NotFound(Objecto));
                }

                model.Name = request.Name;
                model.Year = request.Year;
                model.Type = request.Type;
                model.Motor = EnunsServices.GetMotor(request.Motor);
                model.Transmission = EnunsServices.GetTransmission(request.Transmission);

                await _repository.Update(model, cancellationToken);
                
                _prometheusService.AddUpdateModelCounter(StatusCodes.Status200OK.ToString());
                
                var category = new RequestValidService(model.Id, request.IdCategory);
                var manufacturer = new RequestValidService(model.Id, request.IdManufacturer);
                
                await _rabbitMqService.PublishMessage(category, RabbitQueue.CATEGORY_MODEL_UPDATE_REQUEST_QUEUE, cancellationToken);
                await _rabbitMqService.PublishMessage(manufacturer, RabbitQueue.MANUFACTURER_MODEL_UPDATE_REQUEST_QUEUE, cancellationToken);
                
                return ApiResponse<string>.Success(Objecto, MessageError.OperacaoSucesso(Objecto, Operacao));
            }
            catch (Exception ex)
            {
                _prometheusService.AddUpdateModelCounter(StatusCodes.Status400BadRequest.ToString());
                _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
                return ApiResponse<string>.Error(MessageError.OperacaoErro(Objecto, Operacao));
                //throw;
            }
        }
    }

