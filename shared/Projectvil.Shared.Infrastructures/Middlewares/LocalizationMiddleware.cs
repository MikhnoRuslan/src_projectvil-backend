﻿using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Projectvil.Shared.Infrastructures.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate _next;

    public LocalizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureKey = context.Request.Headers["Accept-Language"];
        if (!string.IsNullOrEmpty(cultureKey))
        {
            if (DoesCultureExist(cultureKey))
            {
                var culture = new CultureInfo(cultureKey);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
        }
        await _next(context);
    }

    private static bool DoesCultureExist(string cultureName)
    {
        return CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Any(culture => string.Equals(culture.Name, cultureName,
                StringComparison.CurrentCultureIgnoreCase));
    }
}