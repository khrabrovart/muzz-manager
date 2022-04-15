namespace MuzzManager.CLI
{
	using System;
	using System.Linq;
	using Interfaces;
	using Microsoft.Extensions.Options;
	using Models;

	public class PreferencesService : IPreferencesService
	{
		private readonly PersistentPreferences _persistentPreferences;
		private readonly IMenuService _menuService;

		public PreferencesService(
			IOptions<PersistentPreferences> persistentPreferences,
			IMenuService menuService)
		{
			_persistentPreferences = persistentPreferences.Value ?? throw new ArgumentNullException(nameof(persistentPreferences));
			_menuService = menuService;
		}

		public UserPreferences SelectUserPreferences()
		{
			var preferences = new UserPreferences
			{
				LocalDirectory = SelectDirectory("Select local directory", _persistentPreferences.LocalDirectories),
				ExternalDirectory =  SelectDirectory("Select external directory", _persistentPreferences.ExternalDirectories)
			};

			_menuService.OpenMessage(
				$"Local directory is set to '{preferences.LocalDirectory}'" +
				$"{Environment.NewLine}" +
				$"External directory is set to '{preferences.ExternalDirectory}'");

			return preferences;
		}

		private string SelectDirectory(string title, string[] directories)
		{
			if (directories.Length == 1)
			{
				return directories.First();
			}

			var localDirectoryMenuResult = _menuService.OpenMenu(title, directories);
			return directories[localDirectoryMenuResult];
		}
	}
}