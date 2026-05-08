using MekkysCakes.Domain.Contracts;
using Microsoft.AspNetCore.Http;

namespace MekkysCakes.Services
{
    public class LanguageContext : ILanguageContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly HashSet<string> SupportedLanguages = ["en", "ar"];

        public LanguageContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLanguage
        {
            get
            {
                // Extract first part of ar-EG,ar;q=0.9,en-US;q=0.8,en;q=0.7
                var lang = _httpContextAccessor.HttpContext?
                    .Request.Headers.AcceptLanguage
                    .FirstOrDefault()?
                    .Split(',')
                    .FirstOrDefault()?
                    .Trim();

                // Extract just the language code (e.g., "ar" from "ar-SA")
                if (!string.IsNullOrEmpty(lang) && lang.Contains('-'))
                    lang = lang.Split('-')[0];
                return SupportedLanguages.Contains(lang ?? "") ? lang! : DefaultLanguage;
            }
        }

        public string DefaultLanguage => "en";
    }
}
