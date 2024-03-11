using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Projectvil.Shared.Infrastructures.Middlewares;

public class TransactionMiddleware<TContext> where TContext : DbContext
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, TContext dbContext)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            await _next(context);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            Console.WriteLine("Test rollback");
            await transaction.RollbackAsync();
            throw;
        }
    }
}