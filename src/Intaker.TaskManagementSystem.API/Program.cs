using Intaker.TaskManagementSystem.DataAccess.Configuration;
using Intaker.TaskManagementSystem.Messaging.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new ApplicationException("Connection string 'DefaultConnection' is not found.");
}
builder.Services.AddDataAccess(connectionString);

var rabbitmqHostName = builder.Configuration.GetValue<string>("rabbitmqHostName");
if (string.IsNullOrEmpty(rabbitmqHostName))
{
    throw new ApplicationException("Rabbitmq host name is not found.");
}
builder.Services.AddMessaging(rabbitmqHostName);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
