using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.FrameWork.DependencyExtensions;
using TaskManager.Domain.Context;

namespace TaskManager.NUnit.ApiTests
{
    [SetUpFixture]
    public class Testing
    {
        //private static IConfiguration _configuration;
        //public static IServiceScopeFactory scopeFactory;

        public static IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            webAppFactory.CreateDefaultClient();
            serviceProvider = webAppFactory.Services;

            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", true, true)
            //    .AddEnvironmentVariables();

            //_configuration = builder.Build();

            //var services = new ServiceCollection();

            //services.AddDbContext<TaskDbContext>(options =>
            //{
            //    options.UseSqlServer(_configuration.GetConnectionString("TaskDbContextConnection"))
            //    .EnableSensitiveDataLogging(true);
            //}, ServiceLifetime.Transient);

            //services.AppApplicationMediatR();

            //scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            //using var scope = scopeFactory.CreateScope();
            //var context = scope.ServiceProvider.GetService<TaskDbContext>();
            var context = serviceProvider.CreateScope().ServiceProvider.GetService<TaskDbContext>();
            context.Add(entity);
            context.SaveChangesAsync();
        }

        //public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        //{
        //    using var scope = scopeFactory.CreateScope();
        //    var mediator = scope.ServiceProvider.GetService<IMediator>();
        //    return await mediator.Send(request);
        //}
    }

}
