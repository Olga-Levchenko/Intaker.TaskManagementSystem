using Microsoft.EntityFrameworkCore;
using Task = Intaker.TaskManagementSystem.DataAccess.Entities.Task;

namespace Intaker.TaskManagementSystem.DataAccess
{
    public class TasksDbContext : DbContext
    {
        public required DbSet<Task> Tasks { get; set; }

        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
