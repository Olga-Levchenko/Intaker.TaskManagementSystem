namespace Intaker.TaskManagementSystem.Messaging.Messages.Events
{
    internal class TaskStatusUpdatedEvent
    {
        public int Id { get; set; }

        public TaskStatusUpdatedEvent(int id)
        {
            Id = id;
        }
    }
}
