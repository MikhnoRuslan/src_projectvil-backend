using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Projectvil.Shared.EntityFramework.Helpers;
using Projectvil.Shared.EntityFramework.Interfaces;
using System.Linq.Dynamic.Core;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.Middlewares.CustomExceptions;

namespace Projectvil.Shared.EntityFramework.Repositories;

public class BaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid> 
    where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _entity;

    public BaseRepository(TContext context)
    {
        _context = context;
        _entity = _context.Set<TEntity>();
    }

    public virtual async Task<TContext> GetDbContextAsync()
    {
        return await Task.FromResult(_context);
    }

    public virtual Task<TEntity> GetAsync(Func<TEntity, bool> predicate,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<TEntity> FirsOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _entity.FirstOrDefaultAsync(predicate, cancellationToken);

    public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _entity.SingleOrDefaultAsync(predicate, cancellationToken);

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var model = await _entity.SingleOrDefaultAsync(predicate, cancellationToken);

        if (model == null)
            throw new EntityNotFoundException(typeof(TEntity));

        return model;
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _entity.AsQueryable();
        query = Include(query, includes);

        var model = await query.SingleOrDefaultAsync(predicate, cancellationToken);

        if (model == null)
            throw new EntityNotFoundException(typeof(TEntity));

        return model;
    }

    public virtual async Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default) =>
        await _entity.ToListAsync(cancellationToken);

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _entity.Where(predicate).ToListAsync(cancellationToken);

    public virtual async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _entity.AsQueryable();
        query = Include(query, includes);

        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    public virtual Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input, 
        ELanguage language,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        throw new NotImplementedException();
    }
    
    public virtual Task<List<TEntity>> GetListAsync( 
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        return await _entity
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);
    }
    
    public virtual async Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        return await _entity
            .Where(predicate)
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);
    }
    
    public virtual async Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes) 
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        var query = _entity.AsQueryable();
        query = Include(query, includes);
        
        return await query
            .Where(predicate)
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, 
        CancellationToken cancellationToken = default)
    {
        var savedEntity = await _entity.AddAsync(entity, cancellationToken);
        
        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);

        return savedEntity.Entity;
    }

    public virtual async Task InsertManyAsync([NotNull] IList<TEntity> entities, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        /*if (!entities.Any())
            throw new ArgumentNullException(nameof(entities));

        await _context.BulkInsertAsync(entities, cancellationToken: cancellationToken);*/
        
        if (!entities.Any())
            throw new ArgumentNullException(nameof(entities));

        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false,
        CancellationToken cancellationToken = default)
    {
        var updatedModel = _entity.Update(entity).Entity;
        
        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);

        return updatedModel;
    }

    public virtual async Task UpdateManyAsync([NotNull] IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (!entities.Any())
            throw new ArgumentNullException(nameof(entities));

        await _context.BulkUpdateAsync(entities, cancellationToken: cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid id, bool autoSave = false, CancellationToken cancellationToken = default)
    {
        var model = await _entity.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (model == null)
            throw new Exception();

        _context.Remove(model);
        
        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, 
        CancellationToken cancellationToken = default)
    {
        var model = await _entity.FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken: cancellationToken);

        if (model == null)
            throw new EntityNotFoundException(typeof(TEntity), entity.Id);

        _context.Remove(model);

        if (autoSave)
            await _context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteManyAsync([NotNull] IList<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (!entities.Any())
            throw new ArgumentNullException(nameof(entities));

        await _context.BulkDeleteAsync(entities, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        await _entity.AnyAsync(cancellationToken);

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        await _entity.AnyAsync(predicate, cancellationToken);

    public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        await _entity.CountAsync(cancellationToken);

    public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default) => await _entity.CountAsync(predicate, cancellationToken);
    
    public virtual Task<int> GetCountAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        throw new NotImplementedException();
    }
    
    public virtual Task<int> GetCountAsync(
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<List<TEntity>> GetListWithFiltersAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        var source = GetEntityWithFilters(input.Filter);

        return await source
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> GetListWithFiltersAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        var source = GetEntityWithFilters(input.Filter, includes);
        
        return await source
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<int> GetCountForFilteredAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        var source = GetEntityWithFilters(input.Filter);
        
        return await source
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .CountAsync(cancellationToken);
    }
    
    public virtual async Task<int> GetCountForFilteredAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto
    {
        var source = GetEntityWithFilters(input.Filter, includes);
        
        return await source
            .OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .CountAsync(cancellationToken);
    }

    public virtual async Task<List<TEntity>> FilterByAsync(
        string filters, CancellationToken cancellationToken = default) =>
        await GetEntityWithFilters(filters).ToListAsync(cancellationToken);

    public virtual async Task<List<TEntity>> FilterByAsync(
        string filters, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes) =>
        await GetEntityWithFilters(filters, includes).ToListAsync(cancellationToken);

    public IQueryable<TEntity> GetEntityWithFilters(string conditions, params Expression<Func<TEntity, object>>[] includes)
    {
        var source = _entity.AsQueryable();

        if (includes.Any())
            source = Include(source, includes);

        if (string.IsNullOrEmpty(conditions))
            return source;
        
        return FilterHelper.GetFiltered(source, conditions);
        
        /*foreach (var filter in filters)
        {
            var expression = FilterHelper.GetFilterExpression<TEntity>(filter);
            source = source.Where(expression);
        }*/

        //return source;
    }

    private IQueryable<TEntity> Include(IQueryable<TEntity> source, params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
        {
            source = source.Include(include);
        }

        return source;
    }
}