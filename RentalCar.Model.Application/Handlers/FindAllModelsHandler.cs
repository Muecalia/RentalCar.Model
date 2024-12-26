using MediatR;
using Microsoft.AspNetCore.Http;
using RentalCar.Model.Application.Queries.Request;
using RentalCar.Model.Application.Queries.Response;
using RentalCar.Model.Core.Configs;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;
using RentalCar.Model.Core.Wrappers;
using RentalCar.Model.Infrastructure.Services;

namespace RentalCar.Model.Application.Handlers;

public class FindAllModelsHandler : IRequestHandler<FindAllModelsRequest, PagedResponse<FindModelResponse>>
{
    private readonly IModelRepository _repository;
    private readonly ILoggerService _loggerService;
    private readonly IPrometheusService _prometheusService;

    public FindAllModelsHandler(IModelRepository repository, ILoggerService loggerService, IPrometheusService prometheusService)
    {
        _repository = repository;
        _loggerService = loggerService;
        _prometheusService = prometheusService;
    }

    public async Task<PagedResponse<FindModelResponse>> Handle(FindAllModelsRequest request, CancellationToken cancellationToken)
    {
        const string Objecto = "modelos";
        try
        {
            var models = await _repository.GetAll(request.PageNumber, request.PageSize, cancellationToken);
            _prometheusService.AddFindAllModelsCounter(StatusCodes.Status200OK.ToString());
            var results = models.Select(model => new FindModelResponse(model.Id, model.Name, EnunsServices.GetDescriptionMotor(model.Motor), 
                EnunsServices.GetDescriptionTransmission(model.Transmission), model.CreatedAt.ToShortDateString(), EnunsServices.GetDescriptionStatus(model.Status))).ToList();
            
            return new PagedResponse<FindModelResponse>(results, request.PageNumber, request.PageSize, results.Count, MessageError.CarregamentoSucesso(Objecto, results.Count));
        }
        catch (Exception ex)
        {
            _prometheusService.AddFindAllModelsCounter(StatusCodes.Status500InternalServerError.ToString());
            _loggerService.LogError(MessageError.CarregamentoErro(Objecto, ex.Message));
            return new PagedResponse<FindModelResponse>(MessageError.CarregamentoErro(Objecto));
            //throw;
        }
    }

}
