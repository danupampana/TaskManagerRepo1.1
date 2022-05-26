using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Models;

namespace TaskManager.ClientApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> logger;
        private readonly IMediator mediator;

        public TasksController(IMediator mediator, ILogger<TasksController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var resposne = await mediator.Send(new Application.Handlers.GetAllTasks.Query());

            return Ok(resposne);
        }

        [HttpGet]
        [Route("GetTask/{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var resposne = await mediator.Send(new Application.Handlers.GetAllTasks.Query(t => t.Id == id));

            if (resposne.tasks != null && resposne.tasks.Any())
            {
                return Ok(resposne.tasks.FirstOrDefault());
            }

            return NotFound();
        }

        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddTask(TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resposne = await mediator.Send(new Application.Handlers.AddTask.Command(model));

            if (resposne.responseCode == System.Net.HttpStatusCode.OK)
            {
                return CreatedAtAction("GetTask", "Tasks", new { id = resposne.Id }, model);
            }
            else
            {
                return Ok(resposne);
            }
        }

        [HttpPut]
        [Route("UpdateTask")]
        public async Task<IActionResult> UpdateTask(TaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resposne = await mediator.Send(new Application.Handlers.UpdateTask.Command(model));

            return Ok(resposne);

        }
    }
}
