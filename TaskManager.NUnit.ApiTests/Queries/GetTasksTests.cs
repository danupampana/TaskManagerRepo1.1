using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Handlers;
using TaskManager.Application.Models;
using TaskManager.Domain.Entities;

namespace TaskManager.NUnit.ApiTests.Queries
{
    using static Testing;
    public class GetTasksTests
    {
        [Test]
        public async Task GetAllTasks_ShouldResturnAllTasks()
        {
            //Arrange
            await AddAsync(new TASK_DATA
            {
                TASK_NAME = "Task Job30",
                TASK_DESCRIPTION = "Task Job 30",
                TASK_DUEDATE = Convert.ToDateTime("05/31/2022"),
                TASK_STARTDATE = Convert.ToDateTime("05/27/2022"),
                TASK_ENDDATE = Convert.ToDateTime("05/31/2022"),
                TASK_PRIORITY = "High",
                TASK_STATUS = "New"
            });

            //Act
            //using var scope = Testing.scopeFactory.CreateScope();
            var mediator = Testing.serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>();
            var resposne = await mediator.Send(new GetAllTasks.Query());
            var tasks = resposne.tasks?.ToList();

            //Assert
            tasks.Should().NotBeNull();
            tasks.Should().HaveCountGreaterThanOrEqualTo(3);
        }
    }
}
