using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories;

public class StatusRepository : PetprojectBaseRepository<Status>, IStatusRepository, ITransientDependence
{
    public StatusRepository(PetProjectsDbContext context) : base(context)
    {
    }

    public override async Task<Status> GetAsync(Func<Status, bool> predicate, ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var statuses = context.Statuses.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();
        
        var models = await statuses.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Status = x, Translation = y })
            .Select(x => new 
            {
                x.Status.Id,
                NameTranslation = x.Translation.Translate
            })
            .ToListAsync(cancellationToken);
        
        return models.Select(x => new Status(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).FirstOrDefault(predicate);
    }

    public override async Task<List<Status>> GetListAsync(ELanguage language, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var statuses = context.Statuses.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await statuses.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Status = x, Translation = y })
            .Select(x => new 
            {
                x.Status.Id,
                NameTranslation = x.Translation.Translate
            })
            .ToListAsync(cancellationToken);

        return models.Select(x => new Status(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).ToList();
    }

    public override async Task<List<Status>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var statuses = context.Statuses.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await statuses.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Status = x, Translation = y })
            .Select(x => new 
            {
                x.Status.Id,
                NameTranslation = x.Translation.Translate
            }).OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);

        return models.Select(x => new Status(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).ToList();
    }

    public override async Task<int> GetCountAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var statuses = context.Statuses.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var result = await statuses.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Status = x, Translation = y })
            .Where(x => x.Translation.Language == language)
            .Select(x => x.Status)
            .CountAsync(cancellationToken);

        return result;
    }
}