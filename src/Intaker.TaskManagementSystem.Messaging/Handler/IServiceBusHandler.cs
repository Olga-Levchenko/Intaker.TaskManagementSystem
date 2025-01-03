namespace Intaker.TaskManagementSystem.Messaging.Handler
{
    public interface IServiceBusHandler
    {
        Task Configure();
        Task SendMessage<T>(T message);
        Task ReceiveMessage<T>(Func<T, Task> action);
        Task StopReceivingMessages();
    }
}
