using Intaker.TaskManagementSystem.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Intaker.TaskManagementSystem.DataAccess.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static void AddDataAccess(this IServiceCollection services, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ITaskRepository, TaskRepository>();
        }
    }
}
