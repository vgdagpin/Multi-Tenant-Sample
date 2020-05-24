using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MultiTenantSample.Application.Personnels.Queries.Models;
using MultiTenantSample.Domain.Entities;

namespace MultiTenantSample.Application.Personnels.Commands
{
    public class CreatePersonnelCommand : IRequest<TenantPersonnel>
    {
        #region Public members
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        #endregion

        #region Handler
        public class CreatePersonnelCommandHandler : IRequestHandler<CreatePersonnelCommand, TenantPersonnel>
        {
            private readonly IMediator mediator;
            private readonly IMultiTenantSampleDbContext dbContext;

            public CreatePersonnelCommandHandler(IMediator mediator, IMultiTenantSampleDbContext dbContext)
            {
                this.mediator = mediator;
                this.dbContext = dbContext;
            }

            public async Task<TenantPersonnel> Handle(CreatePersonnelCommand request, CancellationToken cancellationToken)
            {
                var _retVal = new TenantPersonnel
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    DOB = request.Birthdate,
                    Active = true,
                    PrefixId = 1,
                    GenderFk = new Gender
                    {
                        Name = request.Gender
                    }
                };

                dbContext.TenantPersonnels.Add(_retVal);

                return _retVal;
            }
        }
        #endregion

        #region Validator
        public class CreatePersonnelCommandValidator : AbstractValidator<CreatePersonnelCommand>
        {
            public CreatePersonnelCommandValidator()
            {
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