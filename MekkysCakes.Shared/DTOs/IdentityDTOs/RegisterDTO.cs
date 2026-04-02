using System.ComponentModel.DataAnnotations;

namespace MekkysCakes.Shared.DTOs.IdentityDTOs
{
    public record RegisterDTO([EmailAddress] string email, string displayName, string userName, string password, [Phone] string phoneNumber);
}
