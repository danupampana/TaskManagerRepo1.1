using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.FrameWork.Validators;

namespace TaskManager.Application.FrameWork.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
       where TResponse : BaseResponse, new()
    {
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> logger;
        private readonly IValidationHandler<TRequest> validationHandler;

        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }
        public ValidationBehaviour(ILogger<ValidationBehaviour<TRequest, TResponse>> logger, IValidationHandler<TRequest> validationHandler)
        {
            this.logger = logger;
            this.validationHandler = validationHandler;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().Name;
            if (validationHandler == null)
            {
                logger.LogInformation($"{requestName} does not have a validation handler configured");
                return await next();
            }

            var results = await validationHandler.ValidateAsync(request);
            if (!results.IsSuccessful)
            {
                logger.LogWarning($"Validation failed for {requestName}. Error: {Message(results.messages)}");
                return new TResponse { responseCode = System.Net.HttpStatusCode.BadRequest, responseMessages = results.messages };
            }

            logger.LogInformation($"Validation successful for request {requestName}");
            return await next();
        }

        private string Message(ICollection<string> messages)
        {
            string message = "";

            for (int i = 0; i < messages.Count; i++)
            {
                message += messages.ElementAt(i) + ", ";
            }
            return message;
        }
    }
}
