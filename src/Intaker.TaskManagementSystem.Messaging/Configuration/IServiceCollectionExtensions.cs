using Intaker.TaskManagementSystem.Messaging.Handler;
using Microsoft.Extensions.DependencyInjection;

namespace Intaker.TaskManagementSystem.Messaging.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static void AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
            services.AddHostedService<ReceiveMessagesScopedHostedService>();
        }
    }
}
