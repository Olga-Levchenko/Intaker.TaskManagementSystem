using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.API.Entities
{
    public class Task
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        public TaskStatus Status { get; set; }
        public string? AssignedTo { get; set; }
    }
}
