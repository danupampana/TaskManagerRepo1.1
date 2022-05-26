
namespace TaskManager.Application.Handlers
{
    public class GetAllTasks
    {
        public class TaskResposne : BaseResponse
        {
            public IEnumerable<TaskModel> tasks { get; set; }
        }

        public record Query(Expression<Func<TaskModel, bool>> predicate = null) : IRequest<TaskResposne>;

        public class Handler: IRequestHandler<Query, TaskResposne>
        {
            private readonly TaskDbContext context;

            public Handler(TaskDbContext context)
            {
                this.context = context;
            }

            public async Task<TaskResposne> Handle(Query request, CancellationToken cancellationToken)
            {
                List<string> messages = new List<string>();
                TaskResposne resposne = new();
                try
                {
                    IEnumerable<TaskModel> result = context.TaskData.Select(t => new TaskModel
                    {
                        Id = t.TASK_ID,
                        Name = t.TASK_NAME,
                        Description = t.TASK_DESCRIPTION,
                        DueDate = t.TASK_DUEDATE,
                        StartDate = t.TASK_STARTDATE,
                        EndDate = t.TASK_ENDDATE,
                        Priority = t.TASK_PRIORITY,
                        Status = t.TASK_PRIORITY
                    });

                    if(request.predicate!=null)
                    {
                        result = result.AsQueryable().Where(request.predicate);
                    }

                    resposne.tasks = result;
                    resposne.Success(messages);
                    return await Task.FromResult(resposne);
                }
                catch (AggregateException ae)
                {
                    foreach(var e in ae.Flatten().InnerExceptions)
                    {
                        messages.Add(e.Message);
                    }
                    resposne.Failure(messages, System.Net.HttpStatusCode.InternalServerError);
                    return resposne;
                }
                catch(Exception ex)
                {
                    messages.Add(ex.Message);
                    resposne.Failure(messages, System.Net.HttpStatusCode.InternalServerError);
                    return resposne;
                }
            }
        }
    }
}
