
using System.Linq;
using Modules.Import.Model;
using Modules.Import.Services.Utils;
using NUnit.Framework;
namespace MediaCatalog.Tests.TestSuites.Import
{
    [TestFixture]
    public class TagsAccumulatorTests
    {
        [Test]
        public void AccumulateNullCollection_ReturnsEmptyCollection()
        {
            var accumulator = CreateAccumulator();
            accumulator.AccumulateTags(null);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void AccumulateEmptyCollection_ReturnsEmptyCollection()
        {
            var accumulator = CreateAccumulator();
            accumulator.AccumulateTags(new FileTag[] { });

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void AccumulateDuplicateArtists_ReturnsSingleArtist()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1",
            });
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2",
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());

            var artist = result.SingleOrDefault();

            Assert.NotNull(artist);
            Assert.AreEqual("Artist1", artist.Name);
        }

        [Test]
        public void AccumulateDuplicateArtistsTwoAlbums_ReturnsTwoAlbums()
        {
            var accumulator = CreateAccumulator();

            var tags1 = new FileTagCollection();
            tags1.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags1.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1",
            });

            var tags2 = new FileTagCollection();
            tags2.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags2.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2",
            });

            accumulator.AccumulateTags(tags1);
            accumulator.AccumulateTags(tags2);

            var result = accumulator.GetAccumulatedResult();
            var artist = result.SingleOrDefault();

            Assert.NotNull(artist.Albums);
            Assert.AreEqual(2, artist.Albums.Count);
            Assert.AreEqual("Album1", artist.Albums[0].Name);
            Assert.AreEqual("Album2", artist.Albums[1].Name);
        }

        [Test]
        public void AccumulateMultiArtist_ReturnsMultipleArtists()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1,Artist2",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1",
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());

            var firstArtist = result.FirstOrDefault();

            Assert.NotNull(firstArtist);
            Assert.AreEqual("Artist1", firstArtist.Name);

            var secondArtist = result.LastOrDefault();

            Assert.NotNull(firstArtist);
            Assert.AreEqual("Artist2", secondArtist.Name);
        }

        [Test]
        public void AccumulateMultiArtistNonTrimmed_ReturnsMultipleArtists()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1, Artist2 ",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1",
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());

            var firstArtist = result.FirstOrDefault();

            Assert.NotNull(firstArtist);
            Assert.AreEqual("Artist1", firstArtist.Name);

            var secondArtist = result.LastOrDefault();

            Assert.NotNull(firstArtist);
            Assert.AreEqual("Artist2", secondArtist.Name);
        }

        [Test]
        public void AccumulateMultipleAlbumsForOneArtist_ReturnsTwoAlbums()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1",
            });
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1",
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2",
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());

            var artist = result.SingleOrDefault();

            Assert.NotNull(artist);
            Assert.AreEqual("Artist1", artist.Name);

            Assert.NotNull(artist.Albums);
            Assert.AreEqual(2, artist.Albums.Count);
            Assert.AreEqual("Album1", artist.Albums[0].Name);
            Assert.AreEqual("Album2", artist.Albums[1].Name);
        }

        [Test]
        public void AccumulateMultipleArtistsOneRun_ReturnsAll()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist2"
            });
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist3"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album3"
            });

            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count());

            var firstArtist = result.Where(a => a.Name == "Artist1");
            var secondArtist = result.Where(a => a.Name == "Artist2");
            var thirdArtist = result.Where(a => a.Name == "Artist2");

            Assert.NotNull(firstArtist);
            Assert.NotNull(secondArtist);
            Assert.NotNull(thirdArtist);
        }

        [Test]
        public void AccumulateMissingAlbum_ReturnsArtistOnly()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());

            var artist = result.SingleOrDefault();
            Assert.NotNull(artist);
            Assert.AreEqual("Artist1", artist.Name);

            Assert.NotNull(artist.Albums);
            Assert.AreEqual(0, artist.Albums.Count);
        }

        [Test]
        public void AccumulateMissingArtist_ReturnsEmptyResult()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void SingleArtistAlbum_ArtistsAreAttachedToAlbum()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2"
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());

            var artist = result.SingleOrDefault();
            var firstAlbum = artist.Albums.Where(a => a.Name == "Album1").FirstOrDefault();
            var secondAlbum = artist.Albums.Where(a => a.Name == "Album2").FirstOrDefault();

            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondAlbum);
            Assert.AreEqual(1, firstAlbum.Artists.Count);
            Assert.AreEqual(1, secondAlbum.Artists.Count);
            Assert.AreSame(artist, firstAlbum.Artists[0]);
            Assert.AreSame(artist, secondAlbum.Artists[0]);
        }

        [Test]
        public void MultiArtistAlbum_ArtistsAreAttachedToAlbum()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1,Artist2"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2"
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());

            var artist1 = result.Where(a => a.Name == "Artist1").FirstOrDefault();
            var artist2 = result.Where(a => a.Name == "Artist2").FirstOrDefault();

            Assert.AreEqual(2, artist1.Albums.Count);
            Assert.AreEqual(2, artist2.Albums.Count);

            Assert.AreNotSame(artist1.Albums[0], artist1.Albums[1]);
            Assert.AreNotSame(artist2.Albums[0], artist2.Albums[1]);

            //first artist albums test
            var firstAlbum = artist1.Albums.Where(a => a.Name == "Album1").FirstOrDefault();
            var secondtAlbum = artist1.Albums.Where(a => a.Name == "Album2").FirstOrDefault();
            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondtAlbum);
            Assert.AreEqual(2, firstAlbum.Artists.Count);
            Assert.AreEqual(2, secondtAlbum.Artists.Count);
            Assert.True(firstAlbum.Artists.Contains(artist1) && firstAlbum.Artists.Contains(artist2));
            Assert.True(secondtAlbum.Artists.Contains(artist1) && secondtAlbum.Artists.Contains(artist2));

            //second artists albums test
            firstAlbum = artist2.Albums.Where(a => a.Name == "Album1").FirstOrDefault();
            secondtAlbum = artist2.Albums.Where(a => a.Name == "Album2").FirstOrDefault();
            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondtAlbum);
            Assert.AreEqual(2, firstAlbum.Artists.Count);
            Assert.AreEqual(2, secondtAlbum.Artists.Count);
            Assert.True(firstAlbum.Artists.Contains(artist1) && firstAlbum.Artists.Contains(artist2));
            Assert.True(secondtAlbum.Artists.Contains(artist1) && secondtAlbum.Artists.Contains(artist2));
        }

        [Test]
        public void TwoAlbumsTwoArtists_ArtistsAreAttachedToAlbum()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist2"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album2"
            });

            accumulator.AccumulateTags(tags);

            var result = accumulator.GetAccumulatedResult();

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());

            var artist1 = result.Where(a => a.Name == "Artist1").FirstOrDefault();
            var artist2 = result.Where(a => a.Name == "Artist2").FirstOrDefault();

            Assert.AreEqual(2, artist1.Albums.Count);
            Assert.AreEqual(2, artist2.Albums.Count);

            Assert.AreNotSame(artist1.Albums[0], artist1.Albums[1]);
            Assert.AreNotSame(artist2.Albums[0], artist2.Albums[1]);

            //first artist albums test
            var firstAlbum = artist1.Albums.Where(a => a.Name == "Album1").FirstOrDefault();
            var secondtAlbum = artist1.Albums.Where(a => a.Name == "Album2").FirstOrDefault();
            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondtAlbum);
            Assert.AreEqual(2, firstAlbum.Artists.Count);
            Assert.AreEqual(2, secondtAlbum.Artists.Count);
            Assert.True(firstAlbum.Artists.Contains(artist1) && firstAlbum.Artists.Contains(artist2));
            Assert.True(secondtAlbum.Artists.Contains(artist1) && secondtAlbum.Artists.Contains(artist2));

            //second artists albums test
            firstAlbum = artist2.Albums.Where(a => a.Name == "Album1").FirstOrDefault();
            secondtAlbum = artist2.Albums.Where(a => a.Name == "Album2").FirstOrDefault();
            Assert.NotNull(firstAlbum);
            Assert.NotNull(secondtAlbum);
            Assert.AreEqual(2, firstAlbum.Artists.Count);
            Assert.AreEqual(2, secondtAlbum.Artists.Count);
            Assert.True(firstAlbum.Artists.Contains(artist1) && firstAlbum.Artists.Contains(artist2));
            Assert.True(secondtAlbum.Artists.Contains(artist1) && secondtAlbum.Artists.Contains(artist2));
        }

        [Test]
        public void AlbumSingleGenreAttached_Success()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Genre",
                Value = "Genre1"
            });

            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();
            var album = result.SingleOrDefault().Albums.SingleOrDefault();

            Assert.NotNull(album);
            Assert.NotNull(album.Genres);
            Assert.AreEqual(1, album.Genres.Count);
            Assert.AreEqual("Genre1", album.Genres.SingleOrDefault().Name);
        }

        [Test]
        public void AlbumMultipleGenresAttached_Success()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Genre",
                Value = "Genre1"
            });
            tags.Add(new FileTag()
            {
                Key = "Genre",
                Value = "Genre2"
            });

            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();
            var album = result.SingleOrDefault().Albums.SingleOrDefault();

            Assert.NotNull(album);
            Assert.NotNull(album.Genres);
            Assert.AreEqual(2, album.Genres.Count);

            var firstGenre = album.Genres.Where(g => g.Name == "Genre1").FirstOrDefault();
            var secondGenre = album.Genres.Where(g => g.Name == "Genre2").FirstOrDefault();

            Assert.NotNull(firstGenre);
            Assert.NotNull(secondGenre);
        }

        [Test]
        public void AlbumMultipleGenresInOneTagAttached_Success()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Genre",
                Value = "Genre1, Genre2"
            });


            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();
            var album = result.SingleOrDefault().Albums.SingleOrDefault();

            Assert.NotNull(album);
            Assert.NotNull(album.Genres);
            Assert.AreEqual(2, album.Genres.Count);

            var firstGenre = album.Genres.Where(g => g.Name == "Genre1").FirstOrDefault();
            var secondGenre = album.Genres.Where(g => g.Name == "Genre2").FirstOrDefault();

            Assert.NotNull(firstGenre);
            Assert.NotNull(secondGenre);
        }

        [Test]
        public void SingleYearTagPresent_AppliedToAlbum()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Year",
                Value = "1999"
            });

            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();
            var album = result.SingleOrDefault().Albums.SingleOrDefault();

            Assert.IsNotNull(album);
            Assert.AreEqual(1999, album.Year.Year);
        }

        [Test]
        public void MultipleYearsTagsPresent_FirstAppliedToAlbum()
        {
            var accumulator = CreateAccumulator();

            var tags = new FileTagCollection();
            tags.Add(new FileTag()
            {
                Key = "Artist",
                Value = "Artist1"
            });
            tags.Add(new FileTag()
            {
                Key = "Album",
                Value = "Album1"
            });
            tags.Add(new FileTag()
            {
                Key = "Year",
                Value = "1999"
            });
            tags.Add(new FileTag()
            {
                Key = "Year",
                Value = "2001"
            });

            accumulator.AccumulateTags(tags);
            var result = accumulator.GetAccumulatedResult();
            var album = result.SingleOrDefault().Albums.SingleOrDefault();

            Assert.IsNotNull(album);
            Assert.AreEqual(1999, album.Year.Year);
        }

        private ITagsAccumulator CreateAccumulator()
        {
            return new TagsAccumulator();
        }
    }
}
