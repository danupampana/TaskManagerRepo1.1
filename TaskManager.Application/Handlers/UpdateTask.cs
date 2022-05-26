using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.FrameWork.Validators;

namespace TaskManager.Application.Handlers
{
    public class UpdateTask
    {
        public class TaskResposne : BaseResponse
        {
            public int rowsEffected { get; set; }
        }

        public record Command(TaskModel model) : IRequest<TaskResposne>;

        public class Handler : IRequestHandler<Command, TaskResposne>
        {
            private readonly TaskDbContext context;
            public Handler(TaskDbContext context)
            {
                this.context = context;
            }

            public async Task<TaskResposne> Handle(Command request, CancellationToken cancellationToken)
            {
                List<string> messages = new List<string>();
                TaskResposne resposne = new();
                try
                {
                    var model = request.model;
                    TASK_DATA entity = context.TaskData.FirstOrDefault(t => t.TASK_ID == model.Id);
                    if (entity != null)
                    {
                        entity.TASK_NAME = model.Name;
                        entity.TASK_DESCRIPTION = model.Description;
                        entity.TASK_DUEDATE = model.DueDate.Value;
                        entity.TASK_STARTDATE = model.StartDate;
                        entity.TASK_ENDDATE = model.EndDate;
                        entity.TASK_PRIORITY = model.Priority;
                        entity.TASK_STATUS = model.Status;

                        context.TaskData.Attach(entity);
                        resposne.rowsEffected = await context.SaveChangesAsync();
                    }

                    resposne.Success(messages);
                    return resposne;
                }
                catch (Exception ex)
                {
                    messages.Add(ex.Message);
                    resposne.Failure(messages, HttpStatusCode.InternalServerError);
                    return resposne;
                }
            }

            public class Validator : IValidationHandler<Command>
            {
                private readonly IMediator mediator;
                private readonly TaskDbContext context;
                public Validator(IMediator mediator, TaskDbContext context)
                {
                    this.mediator = mediator;
                    this.context = context;
                }

                public async Task<ValidationResults> ValidateAsync(Command request)
                {
                    List<string> messages = new();
                    try
                    {
                        var response = await mediator.Send(new GetAllTasks.Query(t => t.Id == request.model.Id));
                        if (response.tasks == null || !response.tasks.Any())
                        {
                            messages.Add("Task with id " + request.model.Id + " does not exist.");
                        }

                        var resValidator = await mediator.Send(new ValidateTask.Query(request.model));

                        if(!resValidator.IsValid)
                        {
                            messages.AddRange(resValidator.responseMessages);
                        }

                        if (messages.Count() > 0)
                            return ValidationResults.Fail(messages);
                        else
                            return ValidationResults.Success();
                    }
                    catch (Exception ex)
                    {
                        messages.Add(ex.Message);
                        return ValidationResults.Fail(messages);
                    }
                }
            }
        }
    }
}
