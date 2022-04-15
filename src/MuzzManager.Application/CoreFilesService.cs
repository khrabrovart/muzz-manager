namespace MuzzManager.Application
{
	using System;
	using System.IO;
	using Core.Interfaces;

	public class CoreFilesService : ICoreFilesService
	{
		public string[] GetMusicFiles(string directory)
		{
			if (string.IsNullOrWhiteSpace(directory))
			{
				throw new ArgumentNullException(nameof(directory));
			}

			if (!Directory.Exists(directory))
			{
				throw new DirectoryNotFoundException($"Directory not found: {directory}");
			}

			return Directory.GetFiles(directory, "*.mp3", SearchOption.TopDirectoryOnly);
		}
	}
}