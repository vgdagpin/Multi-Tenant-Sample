using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MultiTenantSample.Domain.Entities;

namespace MultiTenantSample.Application.Personnels.Commands
{
    public class UpdatePersonnelCommand : IRequest<TenantPersonnel>
    {
        #region Public members
        public int ID { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        #endregion

        #region Handler
        public class UpdatePersonnelCommandHandler : IRequestHandler<UpdatePersonnelCommand, TenantPersonnel>
        {
            private readonly IMediator mediator;
            private readonly IMultiTenantSampleDbContext dbContext;

            public UpdatePersonnelCommandHandler(IMediator mediator, IMultiTenantSampleDbContext dbContext)
            {
                this.mediator = mediator;
                this.dbContext = dbContext;
            }

            public async Task<TenantPersonnel> Handle(UpdatePersonnelCommand request, CancellationToken cancellationToken)
            {
                var _entry = await dbContext.TenantPersonnels
                    .Include(a => a.GenderFk)
                    .SingleOrDefaultAsync(a => a.Id == request.ID);

                if (_entry == null)
                {
                    return null;
                }

                if (request.FirstName != null) _entry.FirstName = request.FirstName;
                if (request.MiddleName != null) _entry.MiddleName = request.MiddleName;
                if (request.LastName != null) _entry.LastName = request.LastName;
                if (request.Gender != null) _entry.GenderFk.Name = request.Gender;

                return _entry;
            }
        }
        #endregion

        #region Validator
        public class UpdatePersonnelCommandValidator : AbstractValidator<UpdatePersonnelCommand>
        {
            public UpdatePersonnelCommandValidator()
            {
                RuleFor(a => a.ID)
                    .NotEmpty().WithMessage("ID is required");


                RuleFor(a => a.FirstName)
                    .NotNull().WithMessage("Firstname is required")
                    .MaximumLength(50).WithMessage("Maximum character for FirstName is 50");

                RuleFor(a => a.MiddleName)
                    .MaximumLength(50).WithMessage("Maximum character for MiddleName is 50");

                RuleFor(a => a.LastName)
                    .NotNull().WithMessage("LastName is required")
                    .MaximumLength(50).WithMessage("Maximum character for LastName is 50");

                RuleFor(a => a.Gender)
                    .NotNull().WithMessage("Gender is required")
                    .MaximumLength(25).WithMessage("Maximum character for Gender is 25");
            }
        }
        #endregion
    }
}