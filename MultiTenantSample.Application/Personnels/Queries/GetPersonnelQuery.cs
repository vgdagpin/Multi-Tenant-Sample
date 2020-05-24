using System;
using System.Linq;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiTenantSample.Application.Common;
using MultiTenantSample.Application.Personnels.Queries.Models;

namespace MultiTenantSample.Application.Personnels.Queries
{
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
                        DOB = a.DOB,
                        Gender = a.GenderFk.Name
                    });
            }
        }
        #endregion
    }
}