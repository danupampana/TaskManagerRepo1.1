using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Handlers;
using TaskManager.Application.Models;
using TaskManager.Domain.Entities;

namespace TaskManager.NUnit.ApiTests.Commands
{
    using static Testing;
    public class AddTasksTest
    {
        [Test]
        public async Task AddTasks_DueDate_CanNotBePastDate()
        {
            //Arrange
            var model = new TaskModel
            {
                Name = "Task Job30",
                Description = "Task Job 30",
                DueDate = Convert.ToDateTime("05/20/2022"),
                StartDate = Convert.ToDateTime("05/16/2022"),
                EndDate = Convert.ToDateTime("05/20/2022"),
                Priority = "High",
                Status = "New"
            };

            //Act
            var mediator = Testing.serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>();
            var resposne = await mediator.Send(new AddTask.Command(model));

            //Assert
            resposne.Id.Should().Be(0);
            resposne.responseMessages.Should().HaveCount(1);
            resposne.responseCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task AddTasks_ShouldAddTask()
        {
            //Arrange
            var model = new TaskModel
            {
                Name = "Task Job30",
                Description = "Task Job 30",
                DueDate = Convert.ToDateTime("05/28/2022"),
                StartDate = Convert.ToDateTime("05/27/2022"),
                EndDate = Convert.ToDateTime("05/31/2022"),
                Priority = "High",
                Status = "New"
            };

            //Act
            var mediator = Testing.serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>();
            var resposne = await mediator.Send(new AddTask.Command(model));

            //Assert
            resposne.Id.Should().BeGreaterThan(0);
            resposne.responseCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task AddTasks_Check_MaxPriorityTaks()
        {
            var mediator = Testing.serviceProvider.CreateScope().ServiceProvider.GetService<IMediator>();

            //Arrange
            var qResponse = await mediator.Send(new GetAllTasks.Query(t => t.DueDate.Value.Date == Convert.ToDateTime("05/31/2022").Date));
            var existingCount = qResponse.tasks?.Count();
            for (int i = 0; i < (5 - (existingCount ?? 0)); i++)
            {
                await AddAsync(new TASK_DATA
                {
                    TASK_NAME = "Task Job" + i,
                    TASK_DESCRIPTION = "Task Job " + i,
                    TASK_DUEDATE = Convert.ToDateTime("05/31/2022"),
                    TASK_STARTDATE = Convert.ToDateTime("05/27/2022"),
                    TASK_ENDDATE = Convert.ToDateTime("05/31/2022"),
                    TASK_PRIORITY = "High",
                    TASK_STATUS = "New"
                });
            }

            var model = new TaskModel
            {
                Name = "Task Job30",
                Description = "Task Job 30",
                DueDate = Convert.ToDateTime("05/31/2022"),
                StartDate = Convert.ToDateTime("05/27/2022"),
                EndDate = Convert.ToDateTime("05/31/2022"),
                Priority = "High",
                Status = "New"
            };

            //Act
            var resposne = await mediator.Send(new AddTask.Command(model));

            //Assert
            resposne.Id.Should().Be(0);
            resposne.responseMessages.Should().HaveCount(1);
            resposne.responseCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
