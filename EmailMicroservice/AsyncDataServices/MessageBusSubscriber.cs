
using EmailService.EventProcessing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        

        public MessageBusSubscriber(IConfiguration configuration,IEventProcessor eventProcessor,ILogger<MessageBusSubscriber> logger)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            _logger = logger;
            InitializeRabbitMQ();
        }
        private void InitializeRabbitMQ()
        {

            _logger.LogInformation("MessageBusSubscriber--> IntializeRabbitMq started");
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName,
                    exchange: "trigger",
                    routingKey: "user_key");
                _logger.LogInformation("MessageBusSubscriber --> Listening on the Message Bus...");
               
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception e)
            {
                _logger.LogError($"MessageBusSubscriber -->Error happened in Command Service {e.Message}"); 
            }
            
        }
        private void RabbitMQ_ConnectionShutdown(object sender,ShutdownEventArgs e)
        {
            _logger.LogInformation("MessageBusSubscriber --> Connection shutdown");
        }
        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
            _logger.LogInformation("MessageBusSubscriber --> Disposing resources");
        }
        protected  override  Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                _logger.LogInformation("MessageBusSubscriber --> Event received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                 _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            _logger.LogInformation("MessageBusSubscriber --> Finishing task execution");
            return Task.CompletedTask;
        }
    }
}
