using System.Collections.Generic;
using DataLayer;
using DataLayer.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace Tests.DataLayer
{
    [TestClass]
    public class DataLayerTest
    {
        [TestInitialize()]
        public void Startup()
        {
            session = SessionFactory.GetSession();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            session.Close();
        }

        [TestMethod]
        public void DataProvider_CreateNewArtist_Success()
        {
            TruncateArtists();

            DataProvider provider = new DataProvider(session);

            string artistName = "John Doe";
            string artistBiography = "Some biography";
            string artistPrivateMarks = "Some marks";

            ArtistEntity a = new ArtistEntity() { Name = artistName, Biography = artistBiography, PrivateMarks = artistPrivateMarks };
            Assert.IsTrue(provider.SaveOrUpdate(a));
            Assert.AreEqual(1, CountArtists());

            //validate fields
            ArtistEntity persistentArtist = GetTopArtist();

            Assert.IsTrue(persistentArtist.ID > 0);
            Assert.AreEqual(a.ID, persistentArtist.ID);
            Assert.AreEqual(persistentArtist.Name, artistName);
            Assert.AreEqual(persistentArtist.Biography, artistBiography);
            Assert.AreEqual(persistentArtist.PrivateMarks, artistPrivateMarks);
        }

        [TestMethod]
        public void DataProvider_CreateNewArtistEmptyName_Fail()
        {
            TruncateArtists();

            DataProvider provider = new DataProvider(session);

            ArtistEntity a = new ArtistEntity() { Name = null };
            Assert.IsFalse(provider.SaveOrUpdate(a));
            Assert.AreEqual(0, CountArtists());
        }

        [TestMethod]
        public void DataProvider_CreateArtistsSameName_Fail()
        {
            TruncateArtists();

            DataProvider provider = new DataProvider(session);

            string artistName = "John Doe";

            ArtistEntity a1 = new ArtistEntity() { Name = artistName };
            ArtistEntity a2 = new ArtistEntity() { Name = artistName };

            Assert.IsTrue(provider.SaveOrUpdate(a1));
            Assert.IsFalse(provider.SaveOrUpdate(a2));

            Assert.AreEqual(1, CountArtists());
        }

        [TestMethod]
        public void DataProvider_CreateManyArtists_Success()
        {
            TruncateArtists();

            DataProvider provider = new DataProvider(session);

            string baseArtistName = "John Doe";
            int itemsCount = 100;

            for (int i = 0; i < itemsCount; ++i)
            {
                ArtistEntity a = new ArtistEntity() { Name = baseArtistName + i.ToString() };
                Assert.IsTrue(provider.SaveOrUpdate(a));
            }

            Assert.AreEqual(itemsCount, CountArtists());
        }

        [TestMethod]
        public void DataProvider_UpdateArtistName_Success()
        {
            TruncateArtists();

            string oldName = "John Doe";

            DataProvider provider = new DataProvider(session);
            ArtistEntity a = new ArtistEntity() { Name = oldName };
            provider.SaveOrUpdate(a);

            string newName = oldName + oldName + "x";
            a.Name = newName;

            Assert.IsTrue(provider.SaveOrUpdate(a));

            ArtistEntity persistentArtist = GetTopArtist();
            Assert.AreEqual(a.ID, persistentArtist.ID);
            Assert.AreEqual(a.Name, persistentArtist.Name);
        }

        [TestMethod]
        public void DataProvider_UpdateArtistBiography_Success()
        {
            TruncateArtists();

            string oldBiography = "Biography";

            DataProvider provider = new DataProvider(session);
            ArtistEntity a = new ArtistEntity() { Name = "John Doe", Biography = oldBiography };
            provider.SaveOrUpdate(a);

            string newBiography = oldBiography + oldBiography + "x";
            a.Biography = newBiography;

            Assert.IsTrue(provider.SaveOrUpdate(a));

            ArtistEntity persistentArtist = GetTopArtist();
            Assert.AreEqual(a.ID, persistentArtist.ID);
            Assert.AreEqual(a.Biography, persistentArtist.Biography);
        }

        [TestMethod]
        public void DataProvider_UpdateArtistPrivateMarks_Success()
        {
            TruncateArtists();

            string oldPrivateMarks = "Private Marks";

            DataProvider provider = new DataProvider(session);
            ArtistEntity a = new ArtistEntity() { Name = "John Doe", PrivateMarks = oldPrivateMarks };
            provider.SaveOrUpdate(a);

            string newPrivateMarks = oldPrivateMarks + oldPrivateMarks + "x";
            a.PrivateMarks = newPrivateMarks;

            Assert.IsTrue(provider.SaveOrUpdate(a));

            ArtistEntity persistentArtist = GetTopArtist();
            Assert.AreEqual(a.ID, persistentArtist.ID);
            Assert.AreEqual(a.PrivateMarks, persistentArtist.PrivateMarks);
        }

        [TestMethod]
        public void DataProvider_GetArtists_Success()
        {
            TruncateArtists();

            DataProvider provider = new DataProvider(session);

            string artistName = "John Doe";
            string artistBiography = "Some biography";
            string artistPrivateMarks = "Some marks";

            ArtistEntity a = new ArtistEntity() { 
                Name = artistName, 
                Biography = artistBiography, 
                PrivateMarks = artistPrivateMarks 
            };

            provider.SaveOrUpdate(a);

            IList<ArtistEntity> persistentArtists = provider.GetArtists();
            Assert.IsNotNull(persistentArtists);
            Assert.AreEqual(1, persistentArtists.Count);

            ArtistEntity persistentArtist = persistentArtists[0];

            //compare fields
            Assert.IsTrue(persistentArtist.ID > 0);
            Assert.AreEqual(a.ID, persistentArtist.ID);
            Assert.AreEqual(persistentArtist.Name, artistName);
            Assert.AreEqual(persistentArtist.Biography, artistBiography);
            Assert.AreEqual(persistentArtist.PrivateMarks, artistPrivateMarks);
        }


        protected void TruncateArtists()
        {
            using (ITransaction tx = session.BeginTransaction())
            {
                session.CreateQuery("delete ArtistEntity a").ExecuteUpdate();
                tx.Commit();

                IList<ArtistEntity> artists = session.CreateCriteria<ArtistEntity>().List<ArtistEntity>();
                Assert.AreEqual(0, artists.Count);
            }
        }
        protected int CountArtists()
        {
            int result = -1;

            result = session.CreateCriteria<ArtistEntity>().List<ArtistEntity>().Count;

            return result;
        }
        protected ArtistEntity GetTopArtist()
        {
            IList<ArtistEntity> artists = session.CreateCriteria<ArtistEntity>().SetMaxResults(1).List<ArtistEntity>();
            return artists[0];
        }

        private ISession session;
    }
}
