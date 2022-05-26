using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManager.Application.FrameWork.Behaviours
{
    public class LoggingBehaviour<TRequest, TReponse> : IPipelineBehavior<TRequest, TReponse>
        where TRequest : IRequest<TReponse>
        where TReponse : BaseResponse, new()
    {
        private ILogger<LoggingBehaviour<TRequest, TReponse>> logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TReponse>> logger)
        {
            this.logger = logger;
        }

        public Task<TReponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TReponse> next)
        {
            logger.LogInformation($"{request.GetType().Name} is starting");
            var timer = Stopwatch.StartNew();
            var response = next();
            timer.Stop();
            logger.LogInformation($"{request.GetType().Name} is finished in {timer.ElapsedMilliseconds} milliseconds");

            return response;

        }
    }

}
