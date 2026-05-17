namespace MekkysCakes.Domain.Entities
{
    public interface ITranslatableEntity<TTranslation> where TTranslation : class, ITranslation
    {
        ICollection<TTranslation> Translations { get; set; }
    }
}