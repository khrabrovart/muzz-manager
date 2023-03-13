using System.Linq;
using MuzzManager.Core.Models;

namespace MuzzManager.Application
{
    public class AudioTagsService
    {
        public AudioTags GetTags(string filePath)
        {
            var file = TagLib.File.Create(filePath);

            return new AudioTags
            {
                Title = file.Tag.Title,
                Artist = file.Tag.AlbumArtists.First(),
                Album = file.Tag.Album
            };
        }
    }
}