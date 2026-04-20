using MekkysCakes.Domain.Entities.WishlistModule;

namespace MekkysCakes.Application.Specifications.WishlistSpecifications
{
    public class WishlistByEmailSpecification : BaseSpecification<Wishlist, Guid>
    {
        public WishlistByEmailSpecification(string userEmail) : base(wishlist => wishlist.UserEmail == userEmail)
        {
        }
    }
}
