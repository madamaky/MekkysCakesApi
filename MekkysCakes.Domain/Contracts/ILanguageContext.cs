namespace MekkysCakes.Domain.Contracts
{
    public interface ILanguageContext
    {
        string CurrentLanguage { get; }
        string DefaultLanguage => "en";
    }
}
