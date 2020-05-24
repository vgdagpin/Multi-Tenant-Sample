using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantSample.Application.Personnels.Commands;
using MultiTenantSample.Application.Personnels.Queries;
using MultiTenantSample.Application.Personnels.Queries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiTenantSample.Controllers.API
{
    [ApiController]
    [Route("[controller]")]
    public class PersonnelController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly DbContext dbContext;

        public PersonnelController(IMediator mediator, DbContext dbContext)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<PersonnelBO>> Get()
        {
            var _query = await mediator.Send(new GetPersonnelQuery());

            return await _query.ToListAsync();
        }

        [HttpPost]
        public async Task<int> Create(CreatePersonnelCommand command)
        {
            var _entry = await mediator.Send(command);

            // called savechanges outside CQRS to omit manual Transaction in code
            // SaveChanges is already transactional
            await dbContext.SaveChangesAsync();

            return _entry.Id;
        }

        [HttpPut]
        public async Task<int> Update(UpdatePersonnelCommand command)
        {
            var _entry = await mediator.Send(command);

            if (_entry == null)
            {
                throw new Exception("Entry not found or no access to this resource");
            }

            // called savechanges outside CQRS to omit manual Transaction in code
            // SaveChanges is already transactional
            await dbContext.SaveChangesAsync();

            return _entry.Id;
        }

        [HttpDelete]
        public async Task Update(DeletePersonnelCommand command)
        {
            await mediator.Send(command);

            // called savechanges outside CQRS to omit manual Transaction in code
            // SaveChanges is already transactional
            await dbContext.SaveChangesAsync();
        }
    }
}
