namespace MuzzManager.CLI
{
    using System;
    using System.Collections.Generic;
    using Application;
    using Core.Interfaces;
    using Core.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Interfaces;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ServiceProvider _provider;

        public Startup()
        {
            _configuration = BuildConfiguration();
            _provider = ConfigureServices().BuildServiceProvider();
        }

        public IConfiguration Configuration => _configuration;

        public IServiceProvider Provider => _provider;

        private IConfigurationRoot BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.Configure<PersistentPreferences>(_configuration.GetSection("Preferences"));
            services.Configure<List<Replacement>>(_configuration.GetSection("Replacements"));

            services.AddSingleton(_configuration);

            services.AddTransient<IMainMusicService, MainMusicService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IPreferencesService, PreferencesService>();

            services.AddTransient<ICoreFilesService, CoreFilesService>();
            services.AddTransient<IArtistsUnifyingService, ArtistsUnifyingService>();
            services.AddTransient<IFileNameFixingService, FileNameFixingService>();
            services.AddTransient<IDirectorySynchronizationService, DirectorySynchronizationService>();

            return services;
        }
    }
}