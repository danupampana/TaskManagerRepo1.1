using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Configuration;
using TaskManager.Application.FrameWork.DependencyExtensions;
using TaskManager.Domain.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region "Logging"
string LogsPath = builder.Environment.ContentRootPath + builder.Configuration.GetSection("Serilog").GetValue<string>("LogsPath");
var configuration = builder.Configuration.AddJsonFile("appsettings.json").Build();
builder.Host.UseSerilog(new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File($"{LogsPath}\\{DateTime.Today.Date.Year}\\{DateTime.Today.Date.Month}\\{DateTime.Today.Date.Day}\\log.txt")
    .CreateLogger());
#endregion

#region "DbContext"
builder.Services.AddDbContext<TaskDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDbContextConnection"))
    .EnableSensitiveDataLogging(true);
}, ServiceLifetime.Transient);
#endregion

#region "MediatR Services"
builder.Services.AppApplicationMediatR();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasks API V1");
        c.RoutePrefix = String.Empty; });
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
