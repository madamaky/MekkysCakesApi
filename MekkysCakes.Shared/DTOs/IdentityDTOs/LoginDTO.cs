using System.ComponentModel.DataAnnotations;

namespace MekkysCakes.Shared.DTOs.IdentityDTOs
{
    public record LoginDTO([EmailAddress] string email, string password);
}
