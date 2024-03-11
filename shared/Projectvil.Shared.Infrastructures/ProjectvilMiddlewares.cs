using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Projectvil.Shared.Infrastructures.Middlewares;

namespace Projectvil.Shared.Infrastructures;

public static class ProjectvilMiddlewares
{
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ErrorHandlerMiddleware>();

    public static IApplicationBuilder UseTransaction<TContext>(this IApplicationBuilder builder)
        where TContext : DbContext => builder.UseMiddleware<TransactionMiddleware<TContext>>();
    
    public static IApplicationBuilder UseGlobalRoute(this IApplicationBuilder builder, string route) =>
        builder.UseMiddleware<GlobalRoutePrefixMiddleware>(route);

    public static IApplicationBuilder UseLocalization(this IApplicationBuilder builder) =>
        builder.UseMiddleware<LocalizationMiddleware>();
}