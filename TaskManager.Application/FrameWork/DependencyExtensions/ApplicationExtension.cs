using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.FrameWork.Behaviours;
using TaskManager.Application.FrameWork.Validators;

namespace TaskManager.Application.FrameWork.DependencyExtensions
{
    public static class ApplicationExtentions
    {
        public static void AppApplicationMediatR(this IServiceCollection services)
        { 
            services.AddMediatR(typeof(CQRS).Assembly);
            services.Scan(scan => scan.FromAssemblyOf<IValidationHandler>().AddClasses(c => c.AssignableTo<IValidationHandler>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.AddFluentValidation();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        }
    }
}
