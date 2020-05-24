using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantSample.Application;
using MultiTenantSample.Application.Common;
using MultiTenantSample.Infrastructure.Persistence;
using MultiTenantSample.Infrastructure.Persistence.Interceptors;
using System;

namespace MultiTenantSample.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MultiTenantSampleDbContext>((svc, options) =>
            {
                options.UseSqlServer
                (
                    connectionString: configuration.GetConnectionString("MultiTenantSampleDBConStr"),
                    sqlServerOptionsAction: opt =>
                    {
                        opt.MigrationsAssembly("MultiTenantSample.DbMigration");
                    }
                );

                options.AddInterceptors(new SetTenantInterceptor(svc.GetRequiredService<Tenant>()));
                options.AddInterceptors(new FilterTenantInterceptor());
            });

            services.AddScoped<IMultiTenantSampleDbContext>(provider => provider.GetService<MultiTenantSampleDbContext>());
            services.AddScoped<DbContext>(provider => provider.GetService<MultiTenantSampleDbContext>());

            return services;
        }
    }
}
