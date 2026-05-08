namespace MekkysCakes.Application.Features.Products.Commands
{
    public record ProductTranslationInput(string Language, string Name, string Description);
    public record NameTranslationInput(string Language, string Name);
    public record DeliveryMethodTranslationInput(string Language, string ShortName, string Description, string DeliveryTime);
}