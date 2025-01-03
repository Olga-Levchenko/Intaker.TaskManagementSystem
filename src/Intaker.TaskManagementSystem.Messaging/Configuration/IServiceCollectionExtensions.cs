using Intaker.TaskManagementSystem.Messaging.Handler;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Intaker.TaskManagementSystem.Messaging.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static void AddMessaging(this IServiceCollection services, string rabbitmqHostName)
        {
            if (string.IsNullOrEmpty(rabbitmqHostName))
            {
                throw new ArgumentNullException(nameof(rabbitmqHostName));
            }

            var connectionFactory = new ConnectionFactory() { HostName = rabbitmqHostName };
            services.AddSingleton<IConnectionFactory>(connectionFactory);
            services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
            services.AddHostedService<ReceiveMessagesScopedHostedService>();
        }
    }
}
