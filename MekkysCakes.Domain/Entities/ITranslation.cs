namespace MekkysCakes.Domain.Entities
{
    public interface ITranslation
    {
        string Language { get; set; }
        string Name { get; set; }
    }
}
