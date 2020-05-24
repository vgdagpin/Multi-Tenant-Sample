using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MultiTenantSample.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenantSample.Middlewares
{
    public class DetermineTenantMiddleware
    {
        private readonly RequestDelegate next;

        public DetermineTenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var _tenantInstance = context.RequestServices.GetRequiredService<Tenant>();
            var _tenantIdData = context.Request.Headers["TenantId"];

            if (!string.IsNullOrEmpty(_tenantIdData) && int.TryParse(_tenantIdData, out int _tenandId))
                _tenantInstance.TenantID = _tenandId;
            else
            {
                // throw new Exception("Invalid or missing tenant");
            }

            await next(context);
        }
    }
}
