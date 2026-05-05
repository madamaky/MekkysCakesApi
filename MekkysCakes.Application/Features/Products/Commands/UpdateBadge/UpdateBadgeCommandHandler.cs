using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateBadge
{
    public class UpdateBadgeCommandHandler : IRequestHandler<UpdateBadgeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBadgeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateBadgeCommand request, CancellationToken cancellationToken)
        {
            var badge = await _unitOfWork.GetRepository<Badge, int>().GetByIdAsync(request.Id);
            if (badge is null)
                return Error.NotFound("Badge.NotFound", $"The Badge With Id {request.Id} Was Not Found");

            badge.Name = request.Name;
            _unitOfWork.GetRepository<Badge, int>().Update(badge);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
