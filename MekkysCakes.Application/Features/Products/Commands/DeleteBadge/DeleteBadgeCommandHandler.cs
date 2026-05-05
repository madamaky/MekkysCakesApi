using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.DeleteBadge
{
    public class DeleteBadgeCommandHandler : IRequestHandler<DeleteBadgeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBadgeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteBadgeCommand request, CancellationToken cancellationToken)
        {
            var badge = await _unitOfWork.GetRepository<Badge, int>().GetByIdAsync(request.Id);
            if (badge is null)
                return Error.NotFound("Badge.NotFound", $"The Badge With Id {request.Id} Was Not Found");

            _unitOfWork.GetRepository<Badge, int>().Delete(badge);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
