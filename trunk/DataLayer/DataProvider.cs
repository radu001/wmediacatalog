using System;
using System.Collections.Generic;
using Common;
using DataLayer.Entities;
using NHibernate;

namespace DataLayer
{
    public class DataProvider : IDataProvider
    {
        public DataProvider(ISession session)
        {
            if (session == null)
                throw new NullReferenceException("Session can't be null");
            if (!session.IsOpen)
                throw new ArgumentException("Invalid session. Session must be opened");

            this.session = session;
        }

        #region IDataLayer Members

        public bool SaveOrUpdate(ArtistEntity artist)
        {
            return SaveOrUpdatePersistentObject(artist);
        }

        public IList<ArtistEntity> GetArtists()
        {
            IList<ArtistEntity> result = new List<ArtistEntity>();
            try
            {
                result = session.CreateCriteria<ArtistEntity>().List<ArtistEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool SaveOrUpdate(GenreEntity genre)
        {
            return SaveOrUpdatePersistentObject(genre);
        }

        public IList<GenreEntity> GetGenres()
        {
            IList<GenreEntity> result = new List<GenreEntity>();
            try
            {
                result = session.CreateCriteria<GenreEntity>().List<GenreEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool SaveOrUpdate(AlbumEntity album)
        {
            return SaveOrUpdatePersistentObject(album);
        }

        public IList<AlbumEntity> GetAlbums()
        {
            IList<AlbumEntity> result = new List<AlbumEntity>();
            try
            {
                result = session.CreateCriteria<AlbumEntity>().List<AlbumEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool SaveOrUpdate(MoodEntity mood)
        {
            return SaveOrUpdatePersistentObject(mood);
        }

        public IList<MoodEntity> GetMoods()
        {
            IList<MoodEntity> result = new List<MoodEntity>();
            try
            {
                result = session.CreateCriteria<MoodEntity>().List<MoodEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool SaveOrUpdate(TagEntity tag)
        {
            return SaveOrUpdatePersistentObject(tag);
        }

        public IList<TagEntity> GetTags()
        {
            IList<TagEntity> result = new List<TagEntity>();
            try
            {
                result = session.CreateCriteria<TagEntity>().List<TagEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public IList<UserEntity> GetUsers()
        {
            IList<UserEntity> result = new List<UserEntity>();
            try
            {
                result = session.CreateCriteria<UserEntity>().List<UserEntity>();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return result;
        }

        public bool SaveOrUpdate(ListeningEntity listening)
        {
            return SaveOrUpdatePersistentObject(listening);
        }

        #endregion

        protected bool SaveOrUpdatePersistentObject(PersistentObject obj)
        {
            bool result = false;

            ITransaction tx = null;
            try
            {
                tx = session.BeginTransaction();

                session.SaveOrUpdate(obj);

                tx.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                if (tx != null)
                    tx.Rollback();

                Logger.Write(ex);
            }
            finally
            {
                if (tx != null)
                    tx.Dispose();
            }

            return result;
        }

        #region Private fields

        private ISession session;

        #endregion
    }
}
