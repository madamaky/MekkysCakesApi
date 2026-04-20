using MediatR;
using MekkysCakes.Services.Abstraction;

namespace MekkysCakes.Application.Features.Authentication.Queries.CheckEmail
{
    public class CheckEmailQueryHandler : IRequestHandler<CheckEmailQuery, bool>
    {
        private readonly IIdentityService _identityService;

        public CheckEmailQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
            => await _identityService.FindByEmailAsync(request.Email) is not null;
    }
}
