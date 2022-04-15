namespace MuzzManager.Application
{
	using System.Collections.Generic;
	using System.Linq;
	using Core.Interfaces;

	public class ArtistsUnifyingService : IArtistsUnifyingService
	{
		public IDictionary<string, HashSet<string>> ExtractArtists(IReadOnlyCollection<string> fileNames)
		{
			var artistsCollection = new Dictionary<string, HashSet<string>>();

			foreach (var fileName in fileNames)
			{
				var artistPart = fileName.Split(" - ");
				var artists = artistPart.First().Split('&').Select(fnp => fnp.Trim());

				foreach (var artist in artists)
				{
					var genericArtist = artist.ToLower();

					if (artistsCollection.ContainsKey(genericArtist))
					{
						artistsCollection[genericArtist].Add(artist);
					}
					else
					{
						artistsCollection.Add(genericArtist, new HashSet<string> { artist });
					}
				}
			}

			return artistsCollection;
		}

		public string UnifyArtists(string fileName, IDictionary<string, string> artistsCollection)
		{
			var artistPart = fileName.Split(" - ");
			var artists = artistPart.First().Split('&').Select(fnp => fnp.Trim());

			foreach (var artist in artists)
			{
				var genericArtist = artist.ToLower();

				if (artistsCollection.TryGetValue(genericArtist, out var correctArtistName))
				{
					fileName = fileName.Replace(artist, correctArtistName);
				}
			}

			return fileName;
		}
	}
}