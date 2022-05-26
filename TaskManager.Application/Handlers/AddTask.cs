

using TaskManager.Application.FrameWork.Validators;

namespace TaskManager.Application.Handlers
{
    public class AddTask
    {
        public class TaskResposne : BaseResponse
        {
            public int Id { get; set; }
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
                    TASK_DATA entity = new TASK_DATA
                    {
                        TASK_NAME = model.Name,
                        TASK_DESCRIPTION = model.Description,
                        TASK_DUEDATE = model.DueDate.Value,
                        TASK_STARTDATE = model.StartDate,
                        TASK_ENDDATE = model.EndDate,
                        TASK_PRIORITY = model.Priority,
                        TASK_STATUS = model.Status
                    };

                    await context.TaskData.AddAsync(entity);
                    await context.SaveChangesAsync();
                    model.Id = entity.TASK_ID;
                    resposne.Id = model.Id;
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
                        var resValidator = await mediator.Send(new ValidateTask.Query(request.model));

                        if (!resValidator.IsValid)
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
