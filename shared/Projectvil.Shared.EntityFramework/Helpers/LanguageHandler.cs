using Microsoft.AspNetCore.Http;
using Projectvil.Shared.EntityFramework.Interfaces;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.Constants;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectvil.Shared.EntityFramework.Helpers;

public class LanguageHandler : ILanguageHandler, ITransientDependence
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LanguageHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public ELanguage GetLanguage()
    {
        var context = _httpContextAccessor.HttpContext;
        
        var userLangs = context.Request.Headers["Accept-Language"].ToString();
        var language = userLangs?.Split(',').FirstOrDefault();
         
        return language switch
        {
            ProjectivConstants.Language.Ru => ELanguage.Ru,
            ProjectivConstants.Language.En => ELanguage.En,
            _ => ELanguage.Ru
        };
    }

    public string GetLanguageString()
    {
        var context = _httpContextAccessor.HttpContext;
        
        var userLangs = context.Request.Headers["Accept-Language"].ToString();
        var language = userLangs?.Split(',').FirstOrDefault();

        return language;
    }
}