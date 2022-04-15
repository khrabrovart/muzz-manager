namespace MuzzManager.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Core.Interfaces;
    using Models;
    using Interfaces;

    public class MainMusicService : IMainMusicService
    {
        private const string FileExtension = ".mp3";

        private readonly string[] _mainMenuItems =
        {
            "Fix file names",
            "Unify artists",
            "Synchronize directories",
            "Exit"
        };

        private readonly IMenuService _menuService;
        private readonly IPreferencesService _preferencesService;
        private readonly IFileNameFixingService _fileNameFixingService;
        private readonly IArtistsUnifyingService _artistsUnifyingService;
        private readonly IDirectorySynchronizationService _directorySynchronizationService;

        public MainMusicService(
            IMenuService menuService,
            IPreferencesService preferencesService,
            IFileNameFixingService fileNameFixingService,
            IArtistsUnifyingService artistsUnifyingService,
            IDirectorySynchronizationService directorySynchronizationService)
        {
            _menuService = menuService;
            _preferencesService = preferencesService;
            _fileNameFixingService = fileNameFixingService;
            _artistsUnifyingService = artistsUnifyingService;
            _directorySynchronizationService = directorySynchronizationService;
        }

        public void Start()
        {
            var preferences = _preferencesService.SelectUserPreferences();
            ShowMainMenu(preferences);
        }

        private void ShowMainMenu(UserPreferences preferences)
        {
            var exit = false;

            while (!exit)
            {
                var actionMenuResult = _menuService.OpenMenu("Main menu", _mainMenuItems);

                switch (actionMenuResult)
                {
                    case 0:
                        FixFileNames(preferences);
                        break;

                    case 1:
                        UnifyArtists(preferences);
                        break;

                    case 2:
                        SynchronizeDirectories(preferences);
                        break;

                    case 3:
                        exit = true;
                        break;
                }
            }
        }

        private void FixFileNames(UserPreferences preferences)
        {
            RenameFilesInDirectory(preferences.LocalDirectory, _fileNameFixingService.FixName);
        }

        private void UnifyArtists(UserPreferences preferences)
        {
            var filePaths = Directory.GetFiles(
                preferences.LocalDirectory,
                $"*{FileExtension}",
                SearchOption.TopDirectoryOnly);

            var fileNames = filePaths.Select(Path.GetFileNameWithoutExtension).ToList();

            var artistsDirtyCollection = _artistsUnifyingService.ExtractArtists(fileNames);
            var artistsCleanCollection = new Dictionary<string, string>();

            foreach (var artist in artistsDirtyCollection)
            {
                string correctArtistName;

                if (artist.Value.Count > 1)
                {
                    var artistMenu = artist.Value.ToArray();
                    var artistMenuResult = _menuService.OpenMenu("Choose the correct one", artistMenu);

                    correctArtistName = artistMenu[artistMenuResult];
                }
                else
                {
                    correctArtistName = artist.Value.First();
                }

                artistsCleanCollection.Add(artist.Key, correctArtistName);
            }

            RenameFilesInDirectory(
                preferences.LocalDirectory,
                val => _artistsUnifyingService.UnifyArtists(val, artistsCleanCollection));
        }

        private void SynchronizeDirectories(UserPreferences preferences)
        {
            var notSyncedFiles = _directorySynchronizationService.GetNotSyncedFiles(
                preferences.LocalDirectory,
                preferences.ExternalDirectory);

            for (var i = 0; i < notSyncedFiles.Length; i++)
            {
                var file = notSyncedFiles[i];

                var removeFileResult = _menuService.OpenMenu(
                    $"({i + 1}/{notSyncedFiles.Length}) Not synced file: {file}",
                    new[] { "Remove", "Do nothing" });

                if (removeFileResult == 1)
                {
                    continue;
                }

                File.Delete(file);
            }

            ShowCompletionMessage();
        }

        private void RenameFilesInDirectory(string directory, Func<string, string> renamingFunction)
        {
            var filePaths = Directory.GetFiles(directory, $"*{FileExtension}", SearchOption.TopDirectoryOnly);

            foreach (var filePath in filePaths)
            {
                var from = Path.GetFileNameWithoutExtension(filePath);
                var to = renamingFunction(from);

                if (from != to)
                {
                    var applyMenuResult = _menuService.OpenMenu(
                        $"From: {from}{Environment.NewLine}To:   {to}",
                        new[] {"Apply", "Cancel"});

                    if (applyMenuResult == 1)
                    {
                        continue;
                    }

                    var fromFilePath = Path.Combine(directory, from + FileExtension);
                    var toFilePath = Path.Combine(directory, to + FileExtension);

                    File.Move(fromFilePath, toFilePath);
                }
            }

            ShowCompletionMessage();
        }

        private void ShowCompletionMessage() => _menuService.OpenMessage("All complete");
    }
}