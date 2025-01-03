using TaskEntity = Intaker.TaskManagementSystem.DataAccess.Entities.Task;
using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.DataAccess.Entities
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetTasks();
        System.Threading.Tasks.Task AddTask(TaskEntity task);
        System.Threading.Tasks.Task UpdateTaskStatus(int id, TaskStatus newStatus);
    }
}
