using MekkysCakes.Domain.Entities;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Extensions
{
    public static class TranslationExtensions
    {
        public static LocalizedString ToLocalized<T>(this IEnumerable<T> translations, Func<T, string> fieldSelector) where T : class, ITranslation
        {
            var en = translations.FirstOrDefault(t => t.Language == "en");
            var ar = translations.FirstOrDefault(t => t.Language == "ar");

            return new LocalizedString
            {
                En = en is not null ? fieldSelector(en) : "",
                Ar = ar is not null ? fieldSelector(ar) : ""
            };
        }
    }
}
