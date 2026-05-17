namespace MekkysCakes.Shared.DTOs
{
    public record LocalizedString
    {
        public string En { get; init; } = string.Empty;
        public string Ar { get; init; } = string.Empty;
    }
}
