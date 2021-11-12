using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductMicroservice.Dtos;
using ProductMicroservice.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductMicroservice.AsyncDataService
{
    public class MessageBusClientProduct : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageBusClientProduct> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClientProduct(IConfiguration configuration,ILogger<MessageBusClientProduct> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("MeesageBusClientProduct-->Starting creation of message bus client");
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

                _logger.LogInformation("MeesageBusClientProduct-->Connected to RabbitMQ");
            }
            catch (Exception e)
            {
                _logger.LogError($"MeesageBusClientProduct-->Could not connect to Message Bus: {e.Message}");
            }
        }
        public void PublishNewProduct(InventoryPostProductDto product)
        {
            var message = JsonSerializer.Serialize(product);
            if (_connection.IsOpen)
            {
                _logger.LogInformation("MeesageBusClientProduct-->RabbitMQ Connection is open");
                SendMessage(message);
            }
            else
            {
                _logger.LogWarning("MeesageBusClientProduct-->RabbitMQ connections closed, not sending");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                routingKey: "product_key",
                basicProperties: null,
                body: body);
            _logger.LogInformation($"MeesageBusClientProduct-->We have sent ${message}");
        }
        public void Dispose()
        {
            _logger.LogInformation("MeesageBusClientProduct-->MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("MeesageBusClientProduct-->RabbitMQ Connection Shutdown");
        }
    }
}
