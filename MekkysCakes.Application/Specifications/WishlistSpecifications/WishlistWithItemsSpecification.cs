using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Application.Specifications.WishlistSpecifications
{
    public class WishlistWithItemsSpecification : BaseSpecification<Wishlist, Guid>
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
