using MekkysCakes.Domain.Entities.WishlistModule;

namespace MekkysCakes.Services.Specifications.WishlistSpecifications
{
    internal class WishlistByEmailSpecification : BaseSpecification<Wishlist, Guid>
    {
        public WishlistByEmailSpecification(string userEmail) : base(wishlist => wishlist.UserEmail == userEmail)
        {
        }
    }
}
