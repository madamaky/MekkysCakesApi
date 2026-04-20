using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(request.Id);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {request.Id} Is Not Found");

            _unitOfWork.GetRepository<Product, int>().Delete(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
