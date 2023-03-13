namespace MuzzManager.Core.Interfaces
{
    public interface IDirectorySynchronizationService
    {
        (string filePath, bool existsInWorkingDirectory)[] GetNotSyncedFiles(string workingDirectory);
    }
}