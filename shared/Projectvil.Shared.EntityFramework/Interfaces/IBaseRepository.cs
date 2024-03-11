using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Projectvil.Shared.EntityFramework.Translations;

namespace Projectvil.Shared.EntityFramework.Interfaces;

public interface IBaseRepository<TEntity>
    where TEntity : class, IEntity<Guid>
{
    Task<TEntity> GetAsync(Func<TEntity, bool> predicate,
        ELanguage language,
        CancellationToken cancellationToken = default);

    Task<TEntity> FirsOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input, 
        ELanguage language,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<List<TEntity>> GetListAsync(
        ELanguage language,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includes);

    Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<List<TEntity>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<TEntity> InsertAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    Task InsertManyAsync([NotNull] IList<TEntity> entities,
        bool autoSave = false,
        CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    Task UpdateManyAsync([NotNull] IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, bool autoSave, CancellationToken cancellationToken = default);

    Task DeleteAsync([NotNull] TEntity entity, bool autoSave = false, CancellationToken cancellationToken = default);

    Task DeleteManyAsync([NotNull] IList<TEntity> entities, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    
    Task<int> GetCountAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    public Task<int> GetCountAsync(
        ELanguage language,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListWithFiltersAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<List<TEntity>> GetListWithFiltersAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<int> GetCountForFilteredAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;


    Task<int> GetCountForFilteredAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
        where TPagedAndSortiedAndFilteredResultDto : class, IPagedAndSortiedAndFilteredRequestDto;

    Task<List<TEntity>> FilterByAsync(
        string filters, CancellationToken cancellationToken = default);

    Task<List<TEntity>> FilterByAsync(
        string filters, CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes);
}