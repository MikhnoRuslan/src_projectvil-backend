using Projectvil.Shared.EntityFramework.Translations;

namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface ILanguageHandler
{
    ELanguage GetLanguage();
    string GetLanguageString();
}