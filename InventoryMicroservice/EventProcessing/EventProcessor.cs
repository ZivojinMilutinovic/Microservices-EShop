using AutoMapper;
using InventoryMicroservice.Dtos;
using InventoryMicroservice.Data;
using InventoryMicroservice.Models;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InventoryService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory,IMapper mapper,ILogger<EventProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task ProcessEvent(string message)
        {
            _logger.LogInformation("EventProcessor--> Starting Event processing");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.ProductCreated:
                    await CreateProduct(message);
                break;
                case EventType.ProductDeleted:
                    await DeleteProduct(message);
                    break;
                case EventType.ProductUpdated:
                    await UpdateProduct(message);
                    break;
                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("-->Detrmine Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "ProductCreated":
                    _logger.LogInformation("EventProcessor--> Create Product event detected");
                    return EventType.ProductCreated;
                case "ProductDeleted":
                    _logger.LogInformation("EventProcessor--> Delete product event detected");
                    return EventType.ProductDeleted;
                case "ProductUpdated":
                    _logger.LogInformation("EventProcessor--> Update product event detected");
                    return EventType.ProductUpdated;
                default:
                    _logger.LogInformation("EventProcessor--> Event could not be determined");
                    return EventType.Undertermined;
            }
        }
        private async Task CreateProduct(string productCreatedMessage)
        {
            _logger.LogInformation("EventProcessor--> Create Product method started");
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IInventoryRepo>();
                var productDto = JsonSerializer.Deserialize<ProductDto>(productCreatedMessage);

                try
                {
                    
                    await repo.CreateProductForInventory(productDto);
                    _logger.LogInformation("EventProcessor--> Creating product method finished");

                }
                catch (Exception ex)
                {
                    _logger.LogError($"EventProcessor--> Could not create a Product {ex.Message}");
                }
            }
        }
        private  Task DeleteProduct(string productCreatedMessage)
        {
            return Task.CompletedTask;
        }
        private  Task UpdateProduct(string productCreatedMessage)
        {
            return Task.CompletedTask;
        }
   
    }
    enum EventType
    {
        ProductCreated,
        ProductDeleted,
        ProductUpdated,
        Undertermined
    }
}
