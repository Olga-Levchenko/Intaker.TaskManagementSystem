using Intaker.TaskManagementSystem.DataAccess.Entities;
using Intaker.TaskManagementSystem.Messaging.Messages.Commands;
using Intaker.TaskManagementSystem.Messaging.Handler;
using Microsoft.AspNetCore.Mvc;
using Task = Intaker.TaskManagementSystem.API.Entities.Task;
using TaskStatus = Intaker.TaskManagementSystem.Entities.Enums.TaskStatus;

namespace Intaker.TaskManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(ITaskRepository taskRepository, IServiceBusHandler serviceBusHandler) : ControllerBase
    {
        private readonly ITaskRepository _taskRepository = taskRepository;
        private readonly IServiceBusHandler _serviceBusHandler = serviceBusHandler;

        [HttpGet]
        [Route("tasks")]
        public async Task<IEnumerable<Task>> GetTasks()
        {
            var tasks = await _taskRepository.GetTasks();
            return tasks.Select(t => new Task
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Status = t.Status,
                AssignedTo = t.AssignedTo
            });
        }

        [HttpPost]
        [Route("tasks")]
        public async System.Threading.Tasks.Task AddTask(Task task)
        {
            await _serviceBusHandler.SendMessage(new CreateTaskCommand(task.Name, task.Description, task.Status, task.AssignedTo));
        }

        [HttpPut]
        [Route("tasks/{id}/{newStatus}")]
        public async System.Threading.Tasks.Task UpdateTaskStatus(int id, TaskStatus newStatus)
        {
            await _serviceBusHandler.SendMessage(new UpdateTaskStatusCommand(id, newStatus));
        }
    }
}
