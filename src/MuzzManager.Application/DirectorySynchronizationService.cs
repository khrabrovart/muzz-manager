namespace MuzzManager.Application
{
    using System.IO;
    using System.Linq;
    using Core.Interfaces;

    public class DirectorySynchronizationService : IDirectorySynchronizationService
    {
        private readonly ICoreFilesService _coreFilesService;
        private readonly IPreferencesService _preferencesService;

        public DirectorySynchronizationService(
            ICoreFilesService coreFilesService,
            IPreferencesService preferencesService)
        {
            _coreFilesService = coreFilesService;
            _preferencesService = preferencesService;
        }

        public (string filePath, bool existsInWorkingDirectory)[] GetNotSyncedFiles(string workingDirectory)
        {
            var comparableDirectory = _preferencesService.SelectDirectory("Select comparable directory");

            var workingDirectoryFiles = _coreFilesService.GetMusicFiles(workingDirectory).Select(Path.GetFileName).ToList();
            var comparableDirectoryFiles = _coreFilesService.GetMusicFiles(comparableDirectory).Select(Path.GetFileName).ToList();

            var workingFilesThatDontExistInComparableDirectory = workingDirectoryFiles
                .Except(comparableDirectoryFiles)
                .Select(f => (Path.Combine(workingDirectory, f), existsInWorkingDirectory: true))
                .ToList();

            var comparableFilesThatDontExistInWorkingDirectory = comparableDirectoryFiles
                .Except(workingDirectoryFiles)
                .Select(f => (Path.Combine(comparableDirectory, f), existsInWorkingDirectory: false))
                .ToList();

            return workingFilesThatDontExistInComparableDirectory.Concat(comparableFilesThatDontExistInWorkingDirectory).ToArray();
        }
    }
}