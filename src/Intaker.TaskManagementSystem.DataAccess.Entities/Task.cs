using System.Diagnostics.CodeAnalysis;
using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.DataAccess.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public TaskStatus Status { get; set; }
        public string? AssignedTo { get; set; }

        [SetsRequiredMembers]
        public Task(string name, string description, TaskStatus status, string? assignedTo)
        {
            Name = name;
            Description = description;
            Status = status;
            AssignedTo = assignedTo;
        }
    }
}
