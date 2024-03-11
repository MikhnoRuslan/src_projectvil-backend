using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Projectiv.PetprojectsService.Domain.Models.ProjectCard;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.BaseRepository;
using Projectiv.PetprojectsService.EntityFrameworkCore.Repositories.Interfaces;
using Projectvil.Shared.EntityFramework.Helpers;
using Projectvil.Shared.EntityFramework.Translations;
using Projectvil.Shared.Infrastructures.DI.Interfaces;

namespace Projectiv.PetprojectsService.EntityFrameworkCore.Repositories;

public class ProjectRepository : PetprojectBaseRepository<Project>, IProjectRepository, ITransientDependence
{
    public ProjectRepository(PetProjectsDbContext context) : base(context)
    {
    }

    public override async Task<Project> GetAsync(Func<Project, bool> predicate, ELanguage language, CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var projects = context.Projects.AsQueryable();
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await projects
            .Include(i => i.Status)
            .Include(i => i.Domain)
            .Join(translations,
                x => x.Status.NameTranslationId,
                y => y.Id,
                (x, y) => new { Project = x, StatusTranslation = y })
            .Join(translations,
                x => x.Project.Domain.NameTranslationId,
                y => y.Id,
                (x, y) => new { x.Project, x.StatusTranslation, DomainTranslation = y })
            .Select(x => new
            {
                x.Project.Id,
                x.Project.UserId,
                x.Project.Name,
                x.Project.Description,
                x.Project.DomainId,
                DomainName = x.DomainTranslation.Translate,
                x.Project.StatusId,
                StatusName = x.StatusTranslation.Translate,
                x.Project.ProjectUrl,
                x.Project.GitUrl,
                x.Project.ImageId
            }).ToListAsync(cancellationToken);

        return models.Select(x => new Project(x.Id)
        {
            UserId = x.UserId,
            Name = x.Name,
            Description = x.Description,
            StatusId = x.StatusId,
            StatusName = x.StatusName,
            DomainId = x.DomainId,
            DomainName = x.DomainName,
            ProjectUrl = x.ProjectUrl,
            GitUrl = x.GitUrl,
            ImageId = x.ImageId
        }).FirstOrDefault(predicate);
    }

    public override async Task<List<Project>> GetListAsync<TPagedAndSortiedAndFilteredResultDto>(TPagedAndSortiedAndFilteredResultDto input, ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var projects = context.Projects.AsQueryable();

        projects = FilterHelper.GetFiltered(projects, input.Filter);
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        var models = await projects
            .Include(i => i.Status)
            .Include(i => i.Domain)
            .Join(translations,
                x => x.Status.NameTranslationId,
                y => y.Id,
                (x, y) => new { Project = x, StatusTranslation = y })
            .Join(translations,
                x => x.Project.Domain.NameTranslationId,
                y => y.Id,
                (x, y) => new { x.Project, x.StatusTranslation, DomainTranslation = y })
            .Select(x => new
            {
                x.Project.Id,
                x.Project.UserId,
                x.Project.Name,
                x.Project.Description,
                x.Project.DomainId,
                DomainName = x.DomainTranslation.Translate,
                x.Project.StatusId,
                StatusName = x.StatusTranslation.Translate,
                x.Project.ProjectUrl,
                x.Project.GitUrl,
                x.Project.ImageId,
                x.Project.CreateOn
            }).OrderBy(input.Sorting ?? nameof(Project.CreateOn) + " desc")
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToListAsync(cancellationToken);

        return models.Select(x => new Project(x.Id)
        {
            Name = x.Name,
            UserId = x.UserId,
            Description = x.Description,
            StatusId = x.StatusId,
            StatusName = x.StatusName,
            DomainId = x.DomainId,
            DomainName = x.DomainName,
            ProjectUrl = x.ProjectUrl,
            GitUrl = x.GitUrl,
            ImageId = x.ImageId,
        }).ToList();
    }

    public override async Task<int> GetCountAsync<TPagedAndSortiedAndFilteredResultDto>(
        TPagedAndSortiedAndFilteredResultDto input,
        ELanguage language,
        CancellationToken cancellationToken = default)
    {
        var context = await GetDbContextAsync();
        var projects = context.Projects.AsQueryable();
        projects = FilterHelper.GetFiltered(projects, input.Filter);
        var translations = context.Translations.Where(x => x.Language == language).AsQueryable();

        return await projects
            .Include(i => i.Status)
            .Include(i => i.Domain)
            .Join(translations,
                x => x.Status.NameTranslationId,
                y => y.Id,
                (x, y) => new { Project = x, StatusTranslation = y })
            .Join(translations,
                x => x.Project.Domain.NameTranslationId,
                y => y.Id,
                (x, y) => new { x.Project, x.StatusTranslation, DomainTranslation = y })
            .Select(x => new
            {
                x.Project.Id,
                x.Project.Name,
                x.Project.Description,
                DomainName = x.DomainTranslation.Translate,
                StatusName = x.StatusTranslation.Translate,
                x.Project.ProjectUrl,
                x.Project.GitUrl,
                x.Project.ImageId
            }).CountAsync(cancellationToken);
    }
}