namespace MuzzManager.Application
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Core.Interfaces;

    public class DirectorySynchronizationService : IDirectorySynchronizationService
    {
        private readonly ICoreFilesService _coreFilesService;

        public DirectorySynchronizationService(ICoreFilesService coreFilesService)
        {
            _coreFilesService = coreFilesService;
        }

        public string[] GetNotSyncedFiles(string directory1, string directory2)
        {
            var filesFromDirectory1 = _coreFilesService.GetMusicFiles(directory1).Select(Path.GetFileName).ToList();
            var filesFromDirectory2 = _coreFilesService.GetMusicFiles(directory2).Select(Path.GetFileName).ToList();

            var filesFromDirectory1NotFoundInDirectory2 = filesFromDirectory1
                .Except(filesFromDirectory2)
                .Select(f => Path.Combine(directory1, f))
                .ToList();

            var filesFromDirectory2NotFoundInDirectory1 = filesFromDirectory2
                .Except(filesFromDirectory1)
                .Select(f => Path.Combine(directory2, f))
                .ToList();

            return filesFromDirectory1NotFoundInDirectory2.Concat(filesFromDirectory2NotFoundInDirectory1).ToArray();
        }
    }
}