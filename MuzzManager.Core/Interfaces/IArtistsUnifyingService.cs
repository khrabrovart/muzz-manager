namespace MuzzManager.Core.Interfaces
{
	using System.Collections.Generic;

	public interface IArtistsUnifyingService
	{
		IDictionary<string, HashSet<string>> ExtractArtists(IReadOnlyCollection<string> fileNames);

		string UnifyArtists(string fileName, IDictionary<string, string> artistsCollection);
	}
}