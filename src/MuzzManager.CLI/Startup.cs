namespace MuzzManager.CLI;

using System;
using System.Collections.Generic;
using Application;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        var services = new ServiceCollection()
            .Configure<PersistentPreferences>(_configuration.GetSection("Preferences"))
            .Configure<List<Replacement>>(_configuration.GetSection("Replacements"))

            .AddSingleton(_configuration)

            .AddTransient<IMainMusicService, MainMusicService>()
            .AddTransient<IMenuService, MenuService>()

            .AddTransient<IPreferencesService, PreferencesService>()
            .AddTransient<ICoreFilesService, CoreFilesService>()
            .AddTransient<IArtistsUnifyingService, ArtistsUnifyingService>()
            .AddTransient<IFileNameFixingService, FileNameFixingService>()
            .AddTransient<IDirectorySynchronizationService, DirectorySynchronizationService>();

        return services;
    }
}