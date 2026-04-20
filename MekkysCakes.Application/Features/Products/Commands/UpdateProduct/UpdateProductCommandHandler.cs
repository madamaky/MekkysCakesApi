using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(request.Id);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.Id} Was Not Found");

            var type = await _unitOfWork.GetRepository<ProductType, int>().GetByIdAsync(request.TypeId);
            if (type is null)
                return Error.NotFound("ProductType.NotFound", $"The Product Type With Id {request.TypeId} Was Not Found");

            var theme = await _unitOfWork.GetRepository<ProductTheme, int>().GetByIdAsync(request.ThemeId);
            if (theme is null)
                return Error.NotFound("ProductTheme.NotFound", $"The Product Theme With Id {request.ThemeId} Was Not Found");

            product.Name = request.Name;
            product.Description = request.Description;
            product.PictureUrl = request.PictureUrl;
            product.Price = request.Price;
            product.ThemeId = request.ThemeId;
            product.TypeId = request.TypeId;

            _unitOfWork.GetRepository<Product, int>().Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
