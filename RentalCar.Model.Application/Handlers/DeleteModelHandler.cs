using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Application.Commands.Response;
using RentalCar.Model.Core.Configs;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Core.Wrappers;

namespace RentalCar.Model.Application.Handlers;

public class DeleteModelHandler : IRequestHandler<DeleteModelRequest, ApiResponse<string>>
{
    private readonly IModelRepository _repository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;

    public DeleteModelHandler(IModelRepository repository, ILoggerService loggerService, IPrometheusService prometheusService)
    {
        _repository = repository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
    }

    public async Task<ApiResponse<string>> Handle(DeleteModelRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "modelo";
        const string Operacao = "eliminar";
        try
        {
            var model = await _repository.GetById(request.Id, cancellationToken);
            if (model == null)
            {
                _loggerService.LogWarning(MessageError.NotFound(Objecto, request.Id));
                _prometheusService.AddDeleteModelCounter(StatusCodes.Status404NotFound.ToString());
                return ApiResponse<string>.Error(MessageError.NotFound(Objecto));
            }

            await _repository.Delete(model, cancellationToken);

            //var result = new InputModelResponse(Model.Id, Model.Name);
            _prometheusService.AddDeleteModelCounter(StatusCodes.Status204NoContent.ToString());
            return ApiResponse<string>.Success(Objecto, MessageError.OperacaoSucesso(Objecto, Operacao));
        }
        catch (Exception ex)
        {
            _prometheusService.AddDeleteModelCounter(StatusCodes.Status400BadRequest.ToString());
            _loggerService.LogError(MessageError.OperacaoErro(Objecto, Operacao, ex.Message));
            return ApiResponse<string>.Error(MessageError.OperacaoErro(Objecto, Operacao));
            //throw;
        }
    }
}

