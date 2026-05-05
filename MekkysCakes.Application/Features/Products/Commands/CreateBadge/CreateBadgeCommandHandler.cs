using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.CreateBadge
{
    public class CreateBadgeCommandHandler : IRequestHandler<CreateBadgeCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBadgeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CreateBadgeCommand request, CancellationToken cancellationToken)
        {
            var badge = new Badge { Name = request.Name };
            await _unitOfWork.GetRepository<Badge, int>().AddAsync(badge);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
