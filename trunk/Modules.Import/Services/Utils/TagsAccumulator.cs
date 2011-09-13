
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public class TagsAccumulator : ITagsAccumulator
    {


        public TagsAccumulator()
        {
            artists = new Dictionary<string, Artist>();
            albums = new Dictionary<string, Album>();
            genres = new Dictionary<string, Genre>();
        }

        #region ITagProcessor Members

        public void AccumulateTags(IEnumerable<FileTag> tags)
        {
            if (tags == null)
                return;

            Artist artist = null;
            Album album = null;

            var artistTags = tags.Where(t => t.Key.ToUpper() == TagMatchingConstants.ArtistTagKey);
            if ( artistTags == null )
                return;

            foreach (var artistTag in artistTags)
            {
                string artistNames = artistTag.Value;
                if (!String.IsNullOrEmpty(artistNames))
                {
                    var separatedArtists = artistNames.Split(new char[] { ',' }).Select(an => an.Trim());

                    foreach (var artistName in separatedArtists)
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
            }
        }

        public IEnumerable<Artist> GetAccumulatedResult()
        {
            return artists.Select(dv => dv.Value);
        }

        #endregion

        #region Private methods

        private void ProcessAlbum(IEnumerable<FileTag> tags, Artist artist, Album album)
        {
            var albumTags = tags.Where(t => t.Key.ToUpper() == TagMatchingConstants.AlbumTagKey);
            if (albumTags != null)
            {
                foreach (var albumTag in albumTags)
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

                        ProcessYear(tags, album);

                        if (artist.Albums.Where(a => a.Name == albumName).Count() == 0)
                        {
                            artist.Albums.Add(album);
                        }

                        if (album.Artists.Where(a => a.Name == artist.Name).Count() == 0)
                        {
                            album.Artists.Add(artist);
                        }

                        ProcessGenres(tags, album);
                    }
                }
            }
        }

        private void ProcessYear(IEnumerable<FileTag> tags, Album album)
        {
            var yearTag = tags.Where(t => TagMatchingConstants.YearTagKeys.Contains(t.Key.ToUpper())).FirstOrDefault();
            if (yearTag != null)
            {
                int yearValue = 1900;
                Int32.TryParse(yearTag.Value, out yearValue);
                album.Year = new DateTime(yearValue, 1, 1);
            }
        }

        private void ProcessGenres(IEnumerable<FileTag> tags, Album album)
        {
            var genreTags = tags.Where(t => t.Key.ToUpper() == TagMatchingConstants.GenreTagKey);
            foreach (var genreTag in genreTags)
            {
                if (genreTag != null)
                {
                    string genreNames = genreTag.Value;

                    var strippedGenreNames = genreNames.Split(new char[] { ',' }).Select(gn => gn.Trim());

                    foreach (var genreName in strippedGenreNames)
                    {
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
        }

        #endregion

        #region Private fields

        private Dictionary<string, Artist> artists;
        private Dictionary<string, Album> albums;
        private Dictionary<string, Genre> genres;

        #endregion
    }
}
