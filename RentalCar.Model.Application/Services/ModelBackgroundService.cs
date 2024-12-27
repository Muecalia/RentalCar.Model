using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentalCar.Model.Application.Commands.Request;
using RentalCar.Model.Core.Enuns;
using RentalCar.Model.Core.MessageBus;
using RentalCar.Model.Core.Repositories;
using RentalCar.Model.Core.Services;

namespace RentalCar.Model.Application.Services;

public class ModelBackgroundService : BackgroundService
{
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILoggerService _loggerService;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ModelBackgroundService(IRabbitMqService rabbitMqService, ILoggerService loggerService, IServiceScopeFactory serviceScopeFactory)
    {
        _rabbitMqService = rabbitMqService;
        _loggerService = loggerService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected async override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var scopedFactory = _serviceScopeFactory.CreateScope();
            var scopedService = scopedFactory.ServiceProvider.GetService<IModelRepository>();
            
            await CreateServices(scopedService, cancellationToken);
            await UpdateServices(scopedService, cancellationToken);
            
            await scopedService.UpdateStatus(cancellationToken);
            
            Console.WriteLine("Updated Model Processing");
            
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }
    
    private async Task CreateServices(IModelRepository repository, CancellationToken cancellationToken) 
    {
        const string Objecto = "Criar a chave estrangeira da categoria e do fornecedor";

        //Criar a conexão
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);

        //Criar o canal
        using var channel = await connection.CreateChannelAsync(cancellationToken:cancellationToken);
        try
        {
            var requestCategory = new RequestValidService(String.Empty, string.Empty);
            
            //Aceder a fila
            await channel.QueueDeclareAsync(RabbitQueue.CATEGORY_MODEL_NEW_RESPONSE_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(RabbitQueue.MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE, true, false, false, null, cancellationToken: cancellationToken);


            //Garantir que seja enviado ao consumidor uma mensagem em cada processamento
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            //Definição do consumo das mensagens recebidas
            var consumerCategory = new AsyncEventingBasicConsumer(channel);

            consumerCategory.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //Converter a message para o tipo de retorno
                var request = JsonSerializer.Deserialize<RequestValidService>(message);

                var category = await repository.GetById(request.IdModel, cancellationToken);
                
                if (category != null)
                {
                    category.IdCategory = request.IdService;
                    await repository.Update(category, cancellationToken);
                }
                
            };
            await channel.BasicConsumeAsync(queue: RabbitQueue.CATEGORY_MODEL_NEW_RESPONSE_QUEUE, autoAck: true, consumer: consumerCategory, cancellationToken: cancellationToken);
            
            var consumerManufacturer = new AsyncEventingBasicConsumer(channel);
            consumerManufacturer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //Converter a message para o tipo de retorno
                var request = JsonSerializer.Deserialize<RequestValidService>(message);
                Console.WriteLine($"request -> Id Manufacturer: {request.IdService} - Id Model: {request.IdModel}");
                
                var result = await repository.GetById(request.IdModel, cancellationToken);
                
                if (result is not null)
                {
                    Console.WriteLine($"result -> Name: {result.Name} - Id Model: {result.Id}");
                    result.IdManufacturer = request.IdService;
                    await repository.Update(result, cancellationToken);
                }
            };

            //Iniciar o consumo de mensagens numa file
            await channel.BasicConsumeAsync(queue: RabbitQueue.MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE, autoAck: true, consumer: consumerManufacturer, cancellationToken: cancellationToken);

            //Aguardar o processamento da mensagem
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(Objecto, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
    
    private async Task UpdateServices(IModelRepository repository, CancellationToken cancellationToken)
    {
        const string Objecto = "Actualizar a chave estrangeira da categoria e do fornecedor";

        //Criar a conexão
        using var connection = await _rabbitMqService.CreateConnection(cancellationToken);

        //Criar o canal
        using var channel = await connection.CreateChannelAsync(cancellationToken:cancellationToken);
        try
        {
            //Aceder a fila
            await channel.QueueDeclareAsync(RabbitQueue.CATEGORY_MODEL_FIND_RESPONSE_QUEUE, true, false, false, null, cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(RabbitQueue.MANUFACTURER_MODEL_UPDATE_RESPONSE_QUEUE, true, false, false, null, cancellationToken: cancellationToken);

            //Garantir que seja enviado ao consumidor uma mensagem em cada processamento
            await channel.BasicQosAsync(0, 1, false, cancellationToken);

            //Definição do consumo das mensagens recebidas
            var consumerCategory = new AsyncEventingBasicConsumer(channel);

            consumerCategory.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //Converter a message para o tipo de retorno
                var request = JsonSerializer.Deserialize<RequestValidService>(message);

                var models = await repository.GetById(request.IdModel, cancellationToken);
                
                if (models is not null && !string.Equals(models.IdCategory, request.IdService))
                {
                    models.IdCategory = request.IdService;
                    await repository.Update(models, cancellationToken);
                }
                
            };
            await channel.BasicConsumeAsync(queue: RabbitQueue.CATEGORY_MODEL_NEW_RESPONSE_QUEUE, autoAck: true, consumer: consumerCategory, cancellationToken: cancellationToken);
            
            var consumerManufacturer = new AsyncEventingBasicConsumer(channel);
            consumerManufacturer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                //Converter a message para o tipo de retorno
                var request = JsonSerializer.Deserialize<RequestValidService>(message);
                Console.WriteLine($"request -> Id Manufacturer: {request.IdService} - Id Model: {request.IdModel}");
                
                var models = await repository.GetById(request.IdModel, cancellationToken);
                
                if (models is not null && !string.Equals(models.IdManufacturer, request.IdService))
                {
                    Console.WriteLine($"result -> Name: {models.Name} - Id Model: {models.Id}");
                    models.IdManufacturer = request.IdService;
                    await repository.Update(models, cancellationToken);
                }
            };

            //Iniciar o consumo de mensagens numa file
            await channel.BasicConsumeAsync(queue: RabbitQueue.MANUFACTURER_MODEL_NEW_RESPONSE_QUEUE, autoAck: true, consumer: consumerManufacturer, cancellationToken: cancellationToken);

            //Aguardar o processamento da mensagem
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        }
        catch (Exception ex)
        {
            _loggerService.LogError(Objecto, ex);
            throw;
        }
        finally
        {
            await _rabbitMqService.CloseConnection(connection, channel, cancellationToken);
        }
    }
    
}