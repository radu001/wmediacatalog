﻿
using System;
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
        public void AccumulateMultipleArtistsOneRun_ReturnsOnlyFirst()
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
            Assert.AreEqual(1, result.Count());

            var artist = result.SingleOrDefault();
            Assert.AreEqual("Artist1", artist.Name);
            Assert.AreEqual(3, artist.Albums.Count);
            Assert.AreEqual("Album1", artist.Albums[0].Name);
            Assert.AreEqual("Album2", artist.Albums[1].Name);
            Assert.AreEqual("Album3", artist.Albums[2].Name);
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
        public void MultiArtistAlbum_ArtistsAreAttachedToAlbum()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void SingleArtistAlbum_ArtistsAreAttachedToAlbum()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TwoAlbumsTwoArtists_ArtistsAreAttachedToAlbum()
        {
            throw new NotImplementedException();
        }

        private ITagsAccumulator CreateAccumulator()
        {
            return new TagsAccumulator();
        }
    }
}
