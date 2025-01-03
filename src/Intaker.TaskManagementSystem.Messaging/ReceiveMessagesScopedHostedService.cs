using Intaker.TaskManagementSystem.DataAccess.Entities;
using Intaker.TaskManagementSystem.Messaging.Messages.Events;
using Intaker.TaskManagementSystem.Messaging.Handler;
using Intaker.TaskManagementSystem.Messaging.Messages.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Task = System.Threading.Tasks.Task;
using TaskEntity = Intaker.TaskManagementSystem.DataAccess.Entities.Task;

namespace Intaker.TaskManagementSystem.Messaging
{
    public class ReceiveMessagesScopedHostedService(IServiceScopeFactory serviceScopeFactory, 
        IServiceBusHandler serviceBusHandler) : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly IServiceBusHandler _serviceBusHandler = serviceBusHandler;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _serviceBusHandler.Configure();

            var receiveMessageTasks = new List<Task>
                {
                    ReceiveCreateTaskCommand(),
                    ReceiveUpdateTaskStatusCommand()
                };

            await Task.WhenAll(receiveMessageTasks);
        }

        private async Task ReceiveCreateTaskCommand()
        {
            await _serviceBusHandler.ReceiveMessage<CreateTaskCommand>(async message =>
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                var taskEntity = new TaskEntity(message.Name, message.Description, message.Status, message.AssignedTo);
                await taskRepository.AddTask(taskEntity);
                await _serviceBusHandler.SendMessage(new TaskCreatedEvent(taskEntity.Id));
            });
        }

        private async Task ReceiveUpdateTaskStatusCommand()
        {
            await _serviceBusHandler.ReceiveMessage<UpdateTaskStatusCommand>(async message =>
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();
                var taskRepository = scope.ServiceProvider.GetRequiredService<ITaskRepository>();
                await taskRepository.UpdateTaskStatus(message.Id, message.Status);
                await _serviceBusHandler.SendMessage(new TaskStatusUpdatedEvent(message.Id));
            });
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            await _serviceBusHandler.StopReceivingMessages();
        }
    }
}
