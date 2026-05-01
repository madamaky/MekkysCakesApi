namespace MekkysCakes.Services.Abstraction
{
    public interface ICurrentUserService
    {
        string? Email { get; }
        string? UserId { get; }
        string? Token { get; }
    }
}
