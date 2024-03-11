using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projectvil.DbMigrator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projectiv.IdentityService.EntityFrameworkCore.Context;
using Projectiv.PetprojectsService.EntityFrameworkCore.Context;
using Projectvil.Shared.EntityFramework.Helpers;
using Projectvil.Shared.EntityFramework.Interfaces;

await CreateHostBuilder(args).RunConsoleAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            var env = hostingContext.HostingEnvironment;
            config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddTransient<ProjectivDbMigratorService>();
            services.AddHostedService<DbMigratorHostedService>();
            services.AddScoped<IEntityTimestampUpdater, EntityTimestampUpdater>();
            
            services.AddDbContext<ProjectivIdentityDbContext>(options =>
                options.UseNpgsql(hostContext.Configuration.GetConnectionString("UsersDatabase")));
            services.AddDbContext<PetProjectsDbContext>(options =>
                options.UseNpgsql(hostContext.Configuration.GetConnectionString("PetProjects")));
        });