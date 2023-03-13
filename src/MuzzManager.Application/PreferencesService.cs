namespace MuzzManager.Application
{
	using System;
	using System.Linq;
	using Microsoft.Extensions.Options;
	using Core.Interfaces;
	using Core.Models;

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
				WorkingDirectory = SelectDirectory("Select working directory"),
			};

			return preferences;
		}

		public string SelectDirectory(string caption)
		{
			if (_persistentPreferences.Directories.Length == 1)
			{
				return _persistentPreferences.Directories.First();
			}

			var directoryMenuResult = _menuService.OpenMenu(caption, _persistentPreferences.Directories);
			return _persistentPreferences.Directories[directoryMenuResult];
		}
	}
}