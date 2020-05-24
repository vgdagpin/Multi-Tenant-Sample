using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantSample.Application.Common;
using MultiTenantSample.Application.Common.Behaviours;
using System;
using System.Reflection;

namespace MultiTenantSample.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<Tenant>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}
