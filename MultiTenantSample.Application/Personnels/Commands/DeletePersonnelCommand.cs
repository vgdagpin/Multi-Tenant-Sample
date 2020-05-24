using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace MultiTenantSample.Application.Personnels.Commands
{
    public class DeletePersonnelCommand : IRequest
    {
        #region Public members
        public int ID { get; set; }
        #endregion

        #region Handler
        public class DeletePersonnelCommandHandler : AsyncRequestHandler<DeletePersonnelCommand>
        {
            private readonly IMediator mediator;
            private readonly IMultiTenantSampleDbContext dbContext;

            public DeletePersonnelCommandHandler(IMediator mediator, IMultiTenantSampleDbContext dbContext)
            {
                this.mediator = mediator;
                this.dbContext = dbContext;
            }

            protected override async Task Handle(DeletePersonnelCommand request, CancellationToken cancellationToken)
            {
                var _entry = await dbContext.TenantPersonnels.FindAsync(request.ID);

                if (_entry != null)
                {
                    dbContext.TenantPersonnels.Remove(_entry);
                }
            }
        }
        #endregion

        #region Validator
        public class DeletePersonnellCommandValidator : AbstractValidator<DeletePersonnelCommand>
        {
            public DeletePersonnellCommandValidator()
            {
                RuleFor(a => a.ID)
                    .NotEmpty().WithMessage("ID is required");
            }
        }
        #endregion
    }
}