namespace MekkysCakes.Domain.Entities
{
    public interface ITranslatableEntity<TTranslation> where TTranslation : class
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}