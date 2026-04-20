using AutoMapper;
using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Application.Specifications.WishlistSpecifications;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.WishlistDTOs;

namespace MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist
{
    public class GetWishlistQueryHandler : IRequestHandler<GetWishlistQuery, Result<IEnumerable<WishlistItemDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetWishlistQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<WishlistItemDTO>>> Handle(GetWishlistQuery request, CancellationToken cancellationToken)
        {
            var spec = new WishlistWithItemsAndProductsSpecification(request.UserEmail);
            var wishlist = await _unitOfWork.GetRepository<Wishlist, Guid>().GetByIdAsync(spec);
            if (wishlist is null)
                return Result<IEnumerable<WishlistItemDTO>>.Ok([]);

            var wishlistItems = _mapper.Map<IEnumerable<WishlistItemDTO>>(wishlist.Items);
            return Result<IEnumerable<WishlistItemDTO>>.Ok(wishlistItems);
        }
    }
}
