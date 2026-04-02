using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Services.Specifications.WishlistSpecifications
{
    internal class WishlistWithItemsSpecification : BaseSpecification<Wishlist, Guid>
    {
        public WishlistWithItemsSpecification(string email)
        : base(w => w.UserEmail == email)
        {
            AddThenInclude(query => query
                .Include(wishlist => wishlist.Items)
            );
        }
    }
}
