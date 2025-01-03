using System.Text;
using System.Text.Json;
using Intaker.TaskManagementSystem.Messaging.Messages;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Intaker.TaskManagementSystem.Messaging.Handler
{
    public class ServiceBusHandler : IServiceBusHandler, IDisposable
    {
        private const string QueueName = "tasks";
        private readonly ILogger<ServiceBusHandler> _logger;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private readonly IConnectionFactory _factory;
        private IConnection _connection;
        private IChannel _channel;
        private AsyncEventingBasicConsumer _consumer;

        public ServiceBusHandler(ILogger<ServiceBusHandler> logger, IConnectionFactory factory)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            _logger = logger;
            _factory = factory;
        }

        public async Task Configure()
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(QueueName, durable: false, exclusive: false, autoDelete: false);

            _consumer = new AsyncEventingBasicConsumer(_channel);
        }

        public async Task SendMessage<T>(T message)
        {
            var messageWrapper = new MessageWrapper
            {
                Type = typeof(T).FullName ?? string.Empty,
                Message = JsonSerializer.Serialize(message)
            };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageWrapper));

            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: QueueName, body);
        }

        public async Task ReceiveMessage<T>(Func<T, Task> action)
        {
            _consumer.ReceivedAsync += async (model, eventArgs) =>
            {
                try
                {
                    var body = eventArgs.Body.ToArray();
                    var messageWrapper = JsonSerializer.Deserialize<MessageWrapper>(body);

                    if (messageWrapper != null)
                    {
                        if (messageWrapper.Type != typeof(T).FullName)
                            return;

                        var message = JsonSerializer.Deserialize<T>(messageWrapper.Message);

                        if (message != null)
                        {
                            await action(message);
                            _logger.LogInformation($"Received message {messageWrapper.Message} of type {typeof(T)}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing message of type {typeof(T)}");
                    throw;
                }
            };

            await _channel.BasicConsumeAsync(QueueName, autoAck: true, consumer: _consumer);
        }

        public async Task StopReceivingMessages()
        {
            await _channel.BasicCancelAsync(_consumer.ConsumerTags[0]);
        }

        public void Dispose()
        {
            _channel.CloseAsync().Wait();
            _connection.CloseAsync().Wait();
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
