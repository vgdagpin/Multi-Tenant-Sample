Sample application for Multi-Tenancy system (column based)

There are 3 types of implementation for Multi-Tenancy System
1. **Column based** > TenantID is stored in column per entity (*shared database, shared IO, shared storage*)
2. **Schema based** > Each tenant has its own schema (*shared database, shared IO, shared storage*)
3. **Database based** > Each tenant has its own database, the EF interceptor will change the database on the fly.
___

Below is the example process for Multi-Tenancy application for (**Column Based**)

**Process**

1. Ajax send request (GET, POST, PUT , DELETE) 
2. Include header for TenantID (based from the selection)
``` Javascript
$.ajaxSetup({
    headers: {
        TenantId: /*get value from dropdown*/
    }
});

$.get('/Personnel')
    .done(function (r) {
        // fill the UI table with result from **r**
    })
    .fail(function (err) {
        console.log(err);
    });
```

3. Middleware (**DetermineTenantMiddleware.cs**) will get the TenantID and put it to scoped service (**Tenant.cs**)
``` CSharp
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
```
4. Validate/Process request with CQRS (Command/Query)
``` CSharp
public class GetPersonnelQuery : IRequest<IQueryable<PersonnelBO>>
{
    #region Public members

    #endregion

    #region Handler
    public class GetPersonnelQueryHandler : RequestHandler<GetPersonnelQuery, IQueryable<PersonnelBO>>
    {
        private readonly IMediator mediator;
        private readonly IMultiTenantSampleDbContext dbContext;

        public GetPersonnelQueryHandler(IMediator mediator, IMultiTenantSampleDbContext dbContext)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
        }

        protected override IQueryable<PersonnelBO> Handle(GetPersonnelQuery request)
        {
            return dbContext.TenantPersonnels
                .Include(a => a.GenderFk)
                .Select(a => new PersonnelBO
                {
                    ID = a.Id,
                    FirstName = a.FirstName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    BirthDate = a.DOB,
                    Gender = a.GenderFk.Name
                });
        }
    }
    #endregion
}
```
5. Set database session for TenantID via EF Core interceptor (**SetTenantInterceptor.cs**)
``` CSharp
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
```
6. EF Core will handle the rest :)
   > See **TenantPersonnel_Configuration.cs** and **FilterTenantInterceptor.cs** for helper on how to query with TenantId filter
