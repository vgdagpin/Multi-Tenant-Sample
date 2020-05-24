using Microsoft.EntityFrameworkCore.Diagnostics;
using MultiTenantSample.Application.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantSample.Infrastructure.Persistence.Interceptors
{
    public class FilterTenantInterceptor : DbCommandInterceptor
    {
        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public override Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            command.CommandText = command.CommandText.Replace("[TenantId] = -999", "[TenantId] = CONVERT(INT, SESSION_CONTEXT(N'TenantId'))");

            return base.ReaderExecutingAsync(command, eventData, result);
        }
    }
}
