namespace MuzzManager.Core.Interfaces
{
    public interface IDirectorySynchronizationService
    {
        string[] GetNotSyncedFiles(string directory1, string directory2);
    }
}