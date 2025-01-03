using Intaker.TaskManagementSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.DataAccess
{
    public class TaskRepository(TasksDbContext tasksDbContext) : ITaskRepository
    {
        private readonly TasksDbContext _tasksDbContext = tasksDbContext;

        public async Task<IEnumerable<Entities.Task>> GetTasks()
        {
            return await _tasksDbContext.Tasks.AsNoTracking().ToArrayAsync();
        }

        public async System.Threading.Tasks.Task AddTask(Entities.Task task)
        {
            await _tasksDbContext.Tasks.AddAsync(task);
            await _tasksDbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskStatus(int id, TaskStatus newStatus)
        {
            var task = await _tasksDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id) 
                ?? throw new ApplicationException($"Could not update task with id {id} status. Task is not found.");
            task.Status = newStatus;
            await _tasksDbContext.SaveChangesAsync();
        }
    }
}
