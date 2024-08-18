using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FF.Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBackend(this IServiceCollection services, IConfiguration configuration)
        {
            // configuration
            var settings = AddConfig(services, configuration);

            // catalog context
            services.AddDbContext<Contexts.FFContext>(options => options.UseSqlServer(settings.ConnectionString));

            // repositories
            services.AddTransient<Repositories.ILeagueRepository, Repositories.LeagueRepository>();
            services.AddTransient<Repositories.IManagerRepository, Repositories.ManagerRepository>();
            services.AddTransient<Repositories.IPlayerRepository, Repositories.PlayerRepository>();
            services.AddTransient<Repositories.IPlayerTeamRepository, Repositories.PlayerTeamRepository>();
            services.AddTransient<Repositories.ITeamRepository, Repositories.TeamRepository>();


            // services
            services.AddTransient<Services.IDraftService, Services.DraftService>();
            services.AddTransient<Services.IGenerationService, Services.GenerationService>();
            services.AddTransient<Services.ILeagueService, Services.LeagueService>();
            services.AddTransient<Services.IManagerService, Services.ManagerService>();
            services.AddTransient<Services.IPlayerService, Services.PlayerService>();
            services.AddTransient<Services.ITeamService, Services.TeamService>();
            services.AddTransient<Services.IStatsService, Services.StatsService>();

            // unit of work
            services.AddTransient<Repositories.Framework.IUnitOfWork, Repositories.Framework.UnitOfWork>();
        }

        private static ISettings AddConfig(IServiceCollection services, IConfiguration configuration)
        {
            var sectionName = "Settings";
            if (!configuration.GetSection(sectionName).Exists())
            {
                var message = $"The configuration file is missing the required {sectionName} section";
                throw new Microsoft.Extensions.Options.OptionsValidationException(sectionName, typeof(ISettings), new string[] { message });
            }
            else
            {
                // parse the settings and add as a singleton so available to all
                ISettings settings = new Settings();
                configuration.GetSection("Settings").Bind(settings);

                // set it
                services.AddSingleton<ISettings>(settings);

                return settings;
            }
        }
    }
}
