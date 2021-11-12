using AutoMapper;
using EmailMicroservice.Data;
using EmailMicroservice.Dtos;
using EmailMicroservice.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmailService.EventProcessing
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
            _logger.LogInformation("EventProcessor--> Starting event processing");
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.UserCreated:
                   await sendEmailToUser(message);
                break;

                default:
                    break;
            }
        }
        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation("EventProcessor--> Determing event type");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "UserCreated":
                    _logger.LogInformation("EventProcessor--> User Created event detected");
                    return EventType.UserCreated;
                case "ShipmentCreated":
                    _logger.LogInformation("EventProcessor--> Shipment Created event detected");
                    return EventType.ShipmentCreated;
                default:
                    _logger.LogInformation("EventProcessor-->  Could not Determine event type");
                    return EventType.Undertermined;
            }
        }
        private async Task sendEmailToUser(string userCreatedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IEmailRepo>();
                var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                var userDto = JsonSerializer.Deserialize<UserDto>(userCreatedMessage);

                try
                {
                    var userEmail = _mapper.Map<EmailUser>(userDto);
                    var createdEmailUser=await repo.AddEmailUser(userEmail);
                    await emailSender.SendWelcomeEmailAsync(createdEmailUser.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"EventProcessor-->Could not add EmailUser to email list {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        UserCreated,
        ShipmentCreated,
        Undertermined
    }
}
