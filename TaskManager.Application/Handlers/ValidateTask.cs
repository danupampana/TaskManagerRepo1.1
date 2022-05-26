using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.Handlers
{
    public class ValidateTask
    {
        public class TaskResposne : BaseResponse
        {
            public bool IsValid { get; set; }
        }

        public record Query(TaskModel model) : IRequest<TaskResposne>;

        public class Handler : IRequestHandler<Query, TaskResposne>
        {
            private readonly IMediator mediator;
            private readonly TaskDbContext context;
            private readonly IConfiguration configuration;

            public Handler(IMediator mediator, TaskDbContext context, IConfiguration configuration)
            {
                this.mediator = mediator;
                this.context = context;
                this.configuration = configuration;
            }

            public async Task<TaskResposne> Handle(Query request, CancellationToken cancellationToken)
            {
                List<string> messages = new List<string>();
                TaskResposne resposne = new();
                try
                {
                    var model = request.model;

                    string maxHighPriorityTasks = configuration.GetSection("AppSettings").GetValue("MaxHighPriorityTasksCount", "100");

                    if ((request.model.DueDate.Value - DateTime.Now).Days < 0)
                    {
                        messages.Add("Due date cannot be in the past.");
                    }

                    var resTQ = await mediator.Send(new GetAllTasks.Query(t => t.Priority == TaskPriorityList.High && t.Status != TaskStatusList.Finished));

                    if (resTQ.tasks != null && resTQ.tasks.Any())
                    {
                        var items = resTQ.tasks.ToList();
                        items = items.Where(t => (t.DueDate.Value - model.DueDate.Value).Days == 0).ToList();

                        if (items != null && items.Count() >= Convert.ToInt32(maxHighPriorityTasks))
                            messages.Add("system already has " + maxHighPriorityTasks + " priority tasks on the same due date, please try later.");
                    }

                    resposne.IsValid = messages.Count() > 0 ? false : true;
                    resposne.Success(messages);
                    return await Task.FromResult(resposne);
                }
                catch (AggregateException ae)
                {
                    foreach (var e in ae.Flatten().InnerExceptions)
                    {
                        messages.Add(e.Message);
                    }
                    resposne.Failure(messages, System.Net.HttpStatusCode.InternalServerError);
                    return resposne;
                }
                catch (Exception ex)
                {
                    messages.Add(ex.Message);
                    resposne.Failure(messages, System.Net.HttpStatusCode.InternalServerError);
                    return resposne;
                }
            }
        }
    }
}
