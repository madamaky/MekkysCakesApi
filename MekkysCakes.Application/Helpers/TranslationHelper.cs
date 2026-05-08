using MekkysCakes.Domain.Entities;

namespace MekkysCakes.Application.Helpers
{
    public class TranslationHelper
    {
        public static TTranslation? GetTranslation<TTranslation>(
            ITranslatableEntity<TTranslation> entity,
            string language, 
            string defaultLanguage = "en") where TTranslation : class, ITranslation
        {
            var translation = entity.Translations;
            if (translation == null || !translation.Any())
                return null;

            return translation.FirstOrDefault(t => t.Language == language) 
                ?? translation.FirstOrDefault(t => t.Language == defaultLanguage);
        }
    }
}