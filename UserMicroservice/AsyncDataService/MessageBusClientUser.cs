using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserMicroservice.Dtos;
using UserMicroservice.Models;

namespace UserMicroservice.AsyncDataService
{
    public class MessageBusClientUser : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusClientUser> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClientUser(IConfiguration configuration, ILogger<MessageBusClientUser> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("MeesageBusClientUser-->Starting creation of message bus client");
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                _logger.LogInformation("MeesageBusClientUser-->Connected to RabbitMQ");
            }
            catch (Exception e)
            {
                _logger.LogError($"MeesageBusClientUser-->Could not connect to Message Bus: {e.Message}");
            }
        }
        public void PublishNewUser(UserPublishedDto user)
        {
            var message = JsonSerializer.Serialize(user);
            if (_connection.IsOpen)
            {
                _logger.LogInformation("MeesageBusClientUser-->RabbitMQ Connection is open");
                SendMessage(message);
            }
            else
            {
                _logger.LogWarning("MeesageBusClientUser-->RabbitMQ connections closed, not sending");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                routingKey: "user_key",
                basicProperties: null,
                body: body);
            _logger.LogInformation($"MeesageBusClientUser-->We have sent ${message}");
        }
        public void Dispose()
        {
            _logger.LogInformation("MeesageBusClientUser-->MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("MeesageBusClientUser-->RabbitMQ Connection Shutdown");
        }
    }
}
