using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.Messaging.Messages.Commands
{
    public class UpdateTaskStatusCommand(int id, TaskStatus status)
    {
        public int Id { get; set; } = id;
        public TaskStatus Status { get; set; } = status;
    }
}
