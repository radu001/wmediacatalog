using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BusinessObjects;
using DataServices;
using Npgsql;
using NUnit.Framework;

namespace MediaCatalog.Tests.TestSuites.Import
{
    [TestFixture]
    public class BulkImportTests
    {
        private readonly string ConnectionString = "Server=127.0.0.1;Port=5432;Database=media;User Id=user;Password=user;Timeout=300;";

        [TestFixtureSetUp]
        public void TestSetup()
        {
            utility = new BulkImportHelper(ConnectionString);
        }

        [SetUp]
        public void Setup()
        {
            utility.CleanupDatabase();
        }

        [Test]
        public void BulkImport_NullArtistsCollection_NoException()
        {
            var str = Serialize(null);
            Assert.NotNull(str);

            utility.ExecuteBulkImport(str);
        }

        [Test]
        public void BulkImport_EmptyArtistsCollection_NoException()
        {
            var str = Serialize(new List<Artist>());
            Assert.NotNull(str);

            utility.ExecuteBulkImport(str);
        }

        [Test]
        public void BulkImport_OneArtistNoAlbumsNoGenres_Success()
        {
            const string artistName = "John Doe";

            var str = Serialize(CreateArtists(artistName));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists(false);

            Assert.NotNull(artists);
            Assert.AreEqual(1, artists.Count());

            var artist = artists.First();

            Assert.AreEqual(artistName, artist.Name);
        }

        [Test]
        public void BulkImport_ManyArtistNoAlbumsNoGenres_Success()
        {
            string[] artistNames = new string[] { "Artist1", "Artist2", "Artist3" };
            var str = Serialize(CreateArtists(artistNames));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists();

            Assert.NotNull(artists);
            Assert.AreEqual(3, artists.Count());

            var a = artists.First();
            Assert.AreEqual("Artist1", a.Name);

            a = artists.Skip(1).First();
            Assert.AreEqual("Artist2", a.Name);

            a = artists.Last();
            Assert.AreEqual("Artist3", a.Name);
        }

        [Test]
        public void BulkImport_OneArtistManyAlbumsNoGenres_Success()
        {
            var rawData = new Dictionary<string, List<string>>();
            rawData["Artist1"] = new List<string>()
            {
                "Album1", "Album2", "Album3"
            };

            var str = Serialize(CreateArtistsWithAlbums(rawData));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists(true);

            Assert.NotNull(artists);
            Assert.AreEqual(1, artists.Count());

            var artist = artists.First();

            Assert.NotNull(artist);
            Assert.AreEqual("Artist1", artist.Name);
            Assert.NotNull(artist.Albums);
            Assert.AreEqual(3, artist.Albums.Count);

            var a = artist.Albums.First();
            Assert.AreEqual("Album1", a.Name);

            a = artist.Albums.Skip(1).First();
            Assert.AreEqual("Album2", a.Name);

            a = artist.Albums.Last();
            Assert.AreEqual("Album3", a.Name);
        }

        [Test]
        public void BulkImport_ManyArtistManyAlbumsNoGenres_Success()
        {
            var rawData = new Dictionary<string, List<string>>();
            rawData["Artist1"] = new List<string>()
            {
                "Album1", "Album2", "Album3"
            };
            rawData["Artist2"] = new List<string>()
            {
                "Album4", "Album5", "Album6"
            };

            var str = Serialize(CreateArtistsWithAlbums(rawData));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists(true);

            Assert.NotNull(artists);
            Assert.AreEqual(2, artists.Count());

            var artist = artists.First();

            Assert.NotNull(artist);
            Assert.AreEqual("Artist1", artist.Name);
            Assert.NotNull(artist.Albums);
            Assert.AreEqual(3, artist.Albums.Count);

            var a = artist.Albums.First();
            Assert.AreEqual("Album1", a.Name);

            a = artist.Albums.Skip(1).First();
            Assert.AreEqual("Album2", a.Name);

            a = artist.Albums.Last();
            Assert.AreEqual("Album3", a.Name);

            artist = artists.Last();

            Assert.NotNull(artist);
            Assert.AreEqual("Artist2", artist.Name);
            Assert.NotNull(artist.Albums);
            Assert.AreEqual(3, artist.Albums.Count);

            a = artist.Albums.First();
            Assert.AreEqual("Album4", a.Name);

            a = artist.Albums.Skip(1).First();
            Assert.AreEqual("Album5", a.Name);

            a = artist.Albums.Last();
            Assert.AreEqual("Album6", a.Name);
        }

        [Test]
        public void BulkImport_OneAlbumManyArtists_NoDuplicates()
        {
            var rawData = new Dictionary<string, List<string>>();
            rawData["Album1"] = new List<string>()
            {
                "Artist1", "Artist2", "Artist3"
            };

            var str = Serialize(CreateAlbumsWithManyArtists(rawData));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists(true);
            Assert.NotNull(artists);
            Assert.AreEqual(3, artists.Count());

            var albums = artists.SelectMany(a => a.Albums).Distinct(new DistinctAlbumByID());
            Assert.AreEqual(1, albums.Count());

            var album = albums.First();
            Assert.AreEqual("Album1", album.Name);
        }

        [Test]
        public void BulkImport_TwoAlbumsManyArtists_NoDuplicates()
        {
            var rawData = new Dictionary<string, List<string>>();
            rawData["Album1"] = new List<string>()
            {
                "Artist1", "Artist2", "Artist3"
            };
            rawData["Album2"] = new List<string>()
            {
                "Artist1", "Artist2", "Artist3"
            };

            var str = Serialize(CreateAlbumsWithManyArtists(rawData));
            utility.ExecuteBulkImport(str);

            var artists = utility.LoadArtists(true);
            Assert.NotNull(artists);
            Assert.AreEqual(3, artists.Count());

            var albums = artists.SelectMany(a => a.Albums).Distinct(new DistinctAlbumByID());
            Assert.AreEqual(2, albums.Count());

            Assert.AreEqual("Album1", albums.First().Name);
            Assert.AreEqual("Album2", albums.Last().Name);
        }

        private IEnumerable<Artist> CreateArtists(params string[] artistNames)
        {
            List<Artist> result = new List<Artist>();

            foreach (var aName in artistNames)
            {
                result.Add(new Artist()
                {
                    Name = aName
                });
            }

            return result;
        }

        private IEnumerable<Artist> CreateArtistsWithAlbums(IDictionary<string, List<string>> artistsWithMultipleAlbums)
        {
            List<Artist> result = new List<Artist>();

            foreach (var kvp in artistsWithMultipleAlbums)
            {
                var a = new Artist()
                {
                    Name = kvp.Key
                };

                foreach (var albumName in kvp.Value)
                {
                    var al = new Album()
                    {
                        Name = albumName
                    };

                    a.Albums.Add(al);
                    al.Artists.Add(a);
                }

                result.Add(a);
            }

            return result;
        }

        private IEnumerable<Artist> CreateAlbumsWithManyArtists(IDictionary<string, List<string>> albumsWithMultipleArtists)
        {
            List<Artist> result = new List<Artist>();

            var artistNames = albumsWithMultipleArtists.SelectMany(kvp => kvp.Value).Distinct();

            //create artists first
            Dictionary<string, Artist> artistsCache = new Dictionary<string, Artist>();
            foreach (var a in artistNames)
            {
                var artist = new Artist()
                {
                    Name = a
                };

                artistsCache[a] = artist;
                result.Add(artist);
            }

            //enumerate albums
            foreach (var kvp in albumsWithMultipleArtists)
            {
                var album = new Album()
                {
                    Name = kvp.Key
                };

                foreach (var artistName in kvp.Value)
                {
                    var cachedArtist = artistsCache[artistName];
                    album.Artists.Add(cachedArtist);
                    cachedArtist.Albums.Add(album);
                }
            }

            return result;
        }

        private string Serialize(IEnumerable<Artist> artists)
        {
            return new XmlSerializer().CreateBulkImportXml(artists);
        }

        private BulkImportHelper utility;
    }

    public class BulkImportHelper
    {
        public string ConnectionString { get; private set; }

        public BulkImportHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IEnumerable<Artist> LoadArtists(bool includeAlbums = false, bool includeGenres = false)
        {
            var result = new List<Artist>();

            Dictionary<int, Artist> artistsCache = new Dictionary<int, Artist>();

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("select id,name from artists", conn);
                using (var read = cmd.ExecuteReader())
                {
                    if (read.HasRows)
                    {
                        while (read.Read())
                        {
                            var artist = new Artist()
                            {
                                ID = read.GetInt32(0),
                                Name = read.GetString(1)
                            };

                            result.Add(artist);
                            artistsCache[artist.ID] = artist;
                        }
                    }
                }

                if (includeAlbums)
                {
                    Dictionary<int, Album> albumsCache = new Dictionary<int, Album>();

                    cmd.CommandText = "select al.id,al.name,ar.artist_id from albums al inner join artists_albums ar on al.id = ar.album_id";
                    using (var read = cmd.ExecuteReader())
                    {
                        if (read.HasRows)
                        {
                            while (read.Read())
                            {
                                var album = new Album()
                                {
                                    ID = read.GetInt32(0),
                                    Name = read.GetString(1)
                                };

                                int artistID = read.GetInt32(2);

                                albumsCache[album.ID] = album;

                                var artist = artistsCache[artistID];
                                artist.Albums.Add(album);
                                album.Artists.Add(artist);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void CleanupDatabase()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand("truncate table albums, artists, tags, genres cascade", conn);
                int resultCode = cmd.ExecuteNonQuery();
            }
        }

        public void ExecuteBulkImport(string xml)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(String.Format("import_media('{0}');", xml), conn);
                cmd.CommandType = CommandType.StoredProcedure;
                int resultCode = cmd.ExecuteNonQuery();
            }
        }
    }

    public class DistinctAlbumByID : IEqualityComparer<Album>
    {
        #region IEqualityComparer<Album> Members

        public bool Equals(Album x, Album y)
        {
            return x.ID == y.ID && x.Name == y.Name;
        }

        public int GetHashCode(Album obj)
        {
            return obj.ID.GetHashCode() + 17 * obj.Name.GetHashCode();
        }

        #endregion
    }
}
