using Microsoft.EntityFrameworkCore.Diagnostics;
using MultiTenantSample.Application.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenantSample.Infrastructure.Persistence.Interceptors
{
    public class SetTenantInterceptor : DbConnectionInterceptor
    {
        private readonly Tenant tenant;

        public SetTenantInterceptor(Tenant tenant)
        {
            this.tenant = tenant;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
        {
            if (tenant.TenantID != null)
            {
                DbCommand _cmd = connection.CreateCommand();

                _cmd.CommandText = "sys.sp_set_session_context";
                _cmd.CommandType = CommandType.StoredProcedure;

                DbParameter _key = _cmd.CreateParameter();
                _key.ParameterName = "@key";
                _key.Value = "TenantId";

                DbParameter _value = _cmd.CreateParameter();
                _value.ParameterName = "@value";
                _value.Value = tenant.TenantID;

                _cmd.Parameters.Add(_key);
                _cmd.Parameters.Add(_value);

                await _cmd.ExecuteNonQueryAsync();

                await _cmd.DisposeAsync();
            }

            await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }
    }
}
