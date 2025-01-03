using Microsoft.EntityFrameworkCore;
using Task = Intaker.TaskManagementSystem.DataAccess.Entities.Task;

namespace Intaker.TaskManagementSystem.DataAccess
{
    public class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
    {
        public required DbSet<Task> Tasks { get; set; }
    }
}
