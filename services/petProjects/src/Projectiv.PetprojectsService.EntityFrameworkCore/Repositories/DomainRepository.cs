using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain.Interfaces;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;
using Projectvil.Shared.EntityFramework.Models;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories;

public class DomainRepository : PetprojectBaseRepository<Domain.Models.ProjectCard.Domain>, IDomainRepository, ITransientDependence
{
    public DomainRepository(PetProjectsDbContext context) : base(context)
    {
    }
    
    public override async Task<Domain.Models.ProjectCard.Domain> GetAsync(Func<Domain.Models.ProjectCard.Domain, bool> predicate, 
        ELanguage language, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var domains = context.Domains.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();
        
        var models = await domains.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Domain = x, Translation = y })
            .Select(x => new 
            {
                x.Domain.Id,
                NameTranslation = x.Translation.Translate
            })
            .ToListAsync(cancellationToken);
        
        return models.Select(x => new Domain.Models.ProjectCard.Domain(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).FirstOrDefault(predicate);
    }
    
    public override async Task<List<Domain.Models.ProjectCard.Domain>> GetListAsync(ELanguage language, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var domains = context.Domains.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await domains.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Domain = x, Translation = y })
            .Select(x => new 
            {
                x.Domain.Id,
                NameTranslation = x.Translation.Translate
            })
            .ToListAsync(cancellationToken);

        return models.Select(x => new Domain.Models.ProjectCard.Domain(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).ToList();
    }
    
    public override async Task<List<Domain.Models.ProjectCard.Domain>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var domains = context.Domains.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await domains.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Domain = x, Translation = y })
            .Select(x => new 
            {
                x.Domain.Id,
                NameTranslation = x.Translation.Translate
            }).OrderBy(input.Sorting ?? nameof(Entity<Guid>.Id))
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);

        return models.Select(x => new Domain.Models.ProjectCard.Domain(x.Id)
        {
            NameTranslation = x.NameTranslation
        }).ToList();
    }

    public override async Task<int> GetCountAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var domains = context.Domains.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var result = await domains.Join(translations,
                x => x.NameTranslationId,
                y => y.Id,
                (x, y) => new { Domain = x, Translation = y })
            .Where(x => x.Translation.Language == language)
            .Select(x => x.Domain)
            .CountAsync(cancellationToken);

        return result;
    }
}