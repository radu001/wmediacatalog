
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public class TagsAccumulator : ITagsAccumulator
    {
        public static readonly string ArtistTagKey = "ARTIST";
        public static readonly string AlbumTagKey = "ALBUM";
        public static readonly string GenreTagKey = "GENRE";

        public TagsAccumulator()
        {
            artists = new Dictionary<string, Artist>();
            albums = new Dictionary<string, Album>();
            genres = new Dictionary<string, Genre>();
        }

        #region ITagProcessor Members

        public void AccumulateTags(IEnumerable<FileTag> tags)
        {
            Artist artist = null;
            Album album = null;

            var artistTag = tags.Where(t => t.Key == ArtistTagKey).FirstOrDefault();
            if ( artistTag == null )
                return;

            string artistName = artistTag.Value;
            if (!String.IsNullOrEmpty(artistName))
            {
                if (!artists.TryGetValue(artistName, out artist))
                {
                    artist = new Artist()
                    {
                        Name = artistName
                    };
                    artists[artistName] = artist;
                }

                ProcessAlbum(tags, artist, album);
            }
        }

        private void ProcessAlbum(IEnumerable<FileTag> tags, Artist artist, Album album)
        {
            var albumTag = tags.Where(t => t.Key == AlbumTagKey).FirstOrDefault();
            if (albumTag != null)
            {
                string albumName = albumTag.Value;
                if (!String.IsNullOrEmpty(albumName))
                {
                    if (!albums.TryGetValue(albumName, out album))
                    {
                        album = new Album()
                        {
                            Name = albumName
                        };
                        albums[albumName] = album;
                    }

                    if (artist.Albums.Where(a => a.Name == albumName).Count() == 0)
                    {
                        artist.Albums.Add(album);
                    }

                    ProcessGenre(tags, album);
                }
            }
        }

        private void ProcessGenre(IEnumerable<FileTag> tags, Album album)
        {
            var genreTags = tags.Where(t => t.Key == GenreTagKey);
            foreach (var genreTag in genreTags)
            {
                if (genreTag != null)
                {
                    string genreName = genreTag.Value;
                    if (!String.IsNullOrEmpty(genreName))
                    {
                        Genre genre = null;
                        if (!genres.TryGetValue(genreName, out genre))
                        {
                            genre = new Genre()
                            {
                                Name = genreName
                            };

                            genres[genreName] = genre;
                        }

                        if (album.Genres.Where(g => g.Name == genreName).Count() == 0)
                        {
                            album.Genres.Add(genre);
                        }
                    }
                }
            }
        }

        #endregion

        #region Private fields

        private Dictionary<string, Artist> artists;
        private Dictionary<string, Album> albums;
        private Dictionary<string, Genre> genres;

        #endregion
    }
}
