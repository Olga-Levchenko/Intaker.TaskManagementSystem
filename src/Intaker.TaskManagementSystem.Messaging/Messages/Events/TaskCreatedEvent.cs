namespace Intaker.TaskManagementSystem.Messaging.Messages.Events
{
    internal class TaskCreatedEvent
    {
        public int Id { get; set; }

        public TaskCreatedEvent(int id)
        {
            Id = id;
        }
    }
}
