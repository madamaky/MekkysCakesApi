using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Application.Specifications.WishlistSpecifications
{
    public class WishlistWithItemsAndProductsSpecification : BaseSpecification<Wishlist, Guid>
    {
        public WishlistWithItemsAndProductsSpecification(string userEmail) : base(wishlist => wishlist.UserEmail == userEmail)
        {
            AddThenInclude(query => query
                .Include(wishlist => wishlist.Items.OrderByDescending(item => item.DateAdded))
                    .ThenInclude(item => item.Product)
                        .ThenInclude(product => product.ProductType)
                .Include(wishlist => wishlist.Items)
                    .ThenInclude(item => item.Product)
                        .ThenInclude(product => product.ProductTheme)
            );
        }
    }
}
