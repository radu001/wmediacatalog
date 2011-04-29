
using System;
using System.Collections;
using System.Collections.Generic;
using BusinessObjects;
using Common;
using Common.Data;
using Common.Entities.Pagination;
using DataLayer;
using DataLayer.Entities;
using DataServices.Enums;
using NHibernate;
using NHibernate.Criterion;
namespace DataServices
{
    public class DataProvider
    {
        public Exception ValidateConnection()
        {
            Exception result = null;

            ISession session = null;

            try
            {
                session = SessionFactory.GetSession();
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                result = ex;
            }
            finally
            {
                if (session != null)
                    session.Close();
            }

            return result;
        }

        public User GetUser(string userName, string password)
        {
            User result = null;
            ISession session = SessionFactory.GetSession();

            try
            {
                var query =
                    session.QueryOver<UserEntity>().
                    Where(u => u.UserName == userName).And(u => u.Password == password);

                IList<UserEntity> users = query.List<UserEntity>();

                if (users.Count > 0)
                {
                    UserEntity foundUser = users[0];

                    EntityConverter converter = new EntityConverter();

                    result = converter.FromDataEntity(foundUser);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public void UserLoggedIn(User user)
        {
            if (user == null)
                return;

            ISession session = SessionFactory.GetSession();

            try
            {
                var query =
                    session.QueryOver<UserEntity>().
                    Where(u => u.UserName == user.UserName).And(u => u.Password == user.Password);

                IList<UserEntity> users = query.List<UserEntity>();

                if (users.Count > 0)
                {
                    UserEntity loggedUser = users[0];

                    UserLoginEntity userLogin = new UserLoginEntity()
                    {
                        User = loggedUser,
                        LoginDate = DateTime.Now
                    };

                    session.SaveOrUpdate(userLogin);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }
        }

        public IList<UserSettings> GetUserSettings(User user)
        {
            if (user == null)
                return new List<UserSettings>();

            IList<UserSettings> result = new List<UserSettings>();

            ISession session = SessionFactory.GetSession();

            try
            {
                var query = session.QueryOver<UserSettingsEntity>().
                    Where(u => u.ID == user.ID);

                IList<UserSettingsEntity> dataEntities = query.List<UserSettingsEntity>();

                foreach (UserSettingsEntity de in dataEntities)
                {
                    EntityConverter converter = new EntityConverter();

                    UserSettings businessEntity = converter.FromDataEntity(de);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool UserExists(string userName)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            try
            {
                var usersCount = session.QueryOver<UserEntity>().
                    Where(u => u.UserName == userName).
                    RowCount();

                result = usersCount > 0;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool ArtistExists(string artistName)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            try
            {
                var query = session.QueryOver<ArtistEntity>().
                    WhereRestrictionOn(a => a.Name).IsInsensitiveLike(artistName, MatchMode.Exact);

                int artistsCount = query.RowCount();

                result = artistsCount > 0;

            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool SaveUser(User user)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            try
            {
                EntityConverter converter = new EntityConverter();

                UserEntity dataEntity = converter.FromBusinessEntity(user);

                session.SaveOrUpdate(dataEntity);

                user.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public Artist GetArtist(int artistID)
        {
            Artist result = null;

            ISession session = SessionFactory.GetSession();

            try
            {
                ArtistEntity dataEntity = session.QueryOver<ArtistEntity>().
                    Where(a => a.ID == artistID).SingleOrDefault();

                EntityConverter entityConverter = new EntityConverter();

                result = entityConverter.FromDataEntity(dataEntity, ArtistConvertOptions.Full);
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public IPagedList<Artist> GetArtists(ILoadOptions options)
        {
            IPagedList<Artist> result = new PagedList<Artist>();

            if (options == null)
                return result;

            if (options.MaxResults <= 0)
                return result;

            ISession session = SessionFactory.GetSession();

            try
            {
                DetachedCriteria countCriteria = GetArtistsImpl(options);
                DetachedCriteria listCriteria = GetArtistsImpl(options);

                countCriteria.SetProjection(Projections.RowCount());
                countCriteria.ClearOrders();

                listCriteria.
                    SetFirstResult(options.FirstResult).
                    SetMaxResults(options.MaxResults);

                IMultiCriteria multiCriteria = session.CreateMultiCriteria();
                multiCriteria.Add(countCriteria);
                multiCriteria.Add(listCriteria);


                IList queryResult = multiCriteria.List();

                result.TotalItems = (int)((IList)queryResult[0])[0];

                IList recordsList = (IList)queryResult[1];

                EntityConverter entityConverter = new EntityConverter();

                foreach (var e in recordsList)
                {
                    ArtistEntity dataEntity = e as ArtistEntity;
                    Artist businessEntity = entityConverter.FromDataEntity(dataEntity, ArtistConvertOptions.Small);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        private DetachedCriteria GetArtistsImpl(ILoadOptions options)
        {
            if (options == null)
                return null;

            if (options.MaxResults <= 0)
                return null;

            string fieldName = options.FilterField.FieldName;
            string filterValue = options.FilterValue;

            DetachedCriteria criteria = DetachedCriteria.For<ArtistEntity>();

            if (!options.IncludeWaste)
                criteria.Add(Restrictions.Eq("IsWaste", false));

            if (fieldName != "Tag")
            {
                criteria.
                    Add(Restrictions.InsensitiveLike(fieldName, filterValue, MatchMode.Anywhere)).
                    AddOrder(new Order(fieldName, true));
            }
            else
            {
                DetachedCriteria tagsCriteria = DetachedCriteria.For<ArtistEntity>().CreateCriteria("Tags").
                    Add(Restrictions.InsensitiveLike("Name", filterValue, MatchMode.Anywhere)).
                    SetProjection(Property.ForName("ID"));

                criteria.Add(
                    Subqueries.PropertyIn("ID", tagsCriteria)).AddOrder(new Order("ID", true));
            }

            return criteria;
        }

        public IList<Tag> GetTags()
        {
            IList<Tag> result = new List<Tag>();

            ISession session = SessionFactory.GetSession();

            try
            {
                ICriteria criteria = session.CreateCriteria<TagEntity>();

                IList<TagEntity> tags = criteria.List<TagEntity>();

                EntityConverter entityConverter = new EntityConverter();

                foreach (TagEntity dataEntity in tags)
                {
                    Tag businessEntity = entityConverter.FromDataEntity(dataEntity);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool SaveTag(Tag tag)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                TagEntity dataEntity = converter.FromBusinessEntity(tag);

                session.SaveOrUpdate(dataEntity);

                tx.Commit();

                tag.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public IList<Mood> GetMoods()
        {
            IList<Mood> result = new List<Mood>();

            ISession session = SessionFactory.GetSession();

            try
            {
                ICriteria criteria = session.CreateCriteria<MoodEntity>();

                IList<MoodEntity> moods = criteria.List<MoodEntity>();

                EntityConverter entityConverter = new EntityConverter();

                foreach (MoodEntity dataEntity in moods)
                {
                    Mood businessEntity = entityConverter.FromDataEntity(dataEntity, MoodConvertOptions.Short);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool SaveMood(Mood mood)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                MoodEntity dataEntity = converter.FromBusinessEntity(mood);

                session.SaveOrUpdate(dataEntity);

                tx.Commit();

                mood.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public IList<Place> GetPlaces()
        {
            IList<Place> result = new List<Place>();

            ISession session = SessionFactory.GetSession();

            try
            {
                ICriteria criteria = session.CreateCriteria<PlaceEntity>();

                IList<PlaceEntity> places = criteria.List<PlaceEntity>();

                EntityConverter entityConverter = new EntityConverter();

                foreach (PlaceEntity dataEntity in places)
                {
                    Place businessEntity = entityConverter.FromDataEntity(dataEntity, PlaceConvertOptions.Short);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public bool SavePlace(Place place)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                PlaceEntity dataEntity = converter.FromBusinessEntity(place);

                session.SaveOrUpdate(dataEntity);

                tx.Commit();

                place.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool SaveArtist(Artist artist)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                ArtistEntity dataEntity = converter.FromBusinessEntity(artist, ArtistConvertOptions.Full);

                session.Merge(dataEntity);

                tx.Commit();

                artist.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool SaveArtistWasted(Artist artist, bool includeAlbums)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                var dataEntity = session.Load<ArtistEntity>(artist.ID);
                dataEntity.IsWaste = artist.IsWaste;

                session.Update(dataEntity);

                if (includeAlbums)
                {
                    foreach (var albumEntity in dataEntity.Albums)
                    {
                        albumEntity.IsWaste = true;
                        session.Update(albumEntity);
                    }
                }

                tx.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool RemoveArtist(Artist artist)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {

                ArtistEntity dataEntity = session.CreateCriteria<ArtistEntity>().
                    Add(Restrictions.Eq("ID", artist.ID)).UniqueResult<ArtistEntity>();

                if (dataEntity != null)
                {
                    session.Delete(dataEntity);

                    tx.Commit();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public Album GetAlbum(int albumID)
        {
            Album result = null;

            ISession session = SessionFactory.GetSession();

            try
            {
                AlbumEntity dataEntity = session.CreateCriteria<AlbumEntity>().
                    Add(Restrictions.Eq("ID", albumID)).UniqueResult<AlbumEntity>();

                EntityConverter entityConverter = new EntityConverter();

                result = entityConverter.FromDataEntity(dataEntity, AlbumConvertOptions.Full);
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public IList<Album> GetAlbumsByArtistID(int artistID)
        {
            IList<Album> result = new List<Album>();

            ISession session = SessionFactory.GetSession();

            try
            {

                DetachedCriteria artistsCriteria = DetachedCriteria.For<AlbumEntity>().CreateCriteria("Artists").
                    Add(Restrictions.Eq("ID", artistID)).
                    SetProjection(Property.ForName("ID"));

                ICriteria criteria = session.CreateCriteria<AlbumEntity>().
                    Add(Subqueries.PropertyIn("ID", artistsCriteria)).AddOrder(new Order("ID", true));

                IList<AlbumEntity> albums = criteria.List<AlbumEntity>();

                EntityConverter converter = new EntityConverter();

                foreach (AlbumEntity dataEntity in albums)
                {
                    Album businessEntity = converter.FromDataEntity(dataEntity, AlbumConvertOptions.Small);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        public IPagedList<Album> GetAlbums(ILoadOptions options)
        {
            IPagedList<Album> result = new PagedList<Album>();

            if (options == null)
                return result;

            if (options.MaxResults <= 0)
                return result;

            ISession session = SessionFactory.GetSession();

            try
            {
                DetachedCriteria countCriteria = GetAlbumsImpl(options);
                DetachedCriteria listCriteria = GetAlbumsImpl(options);

                countCriteria.SetProjection(Projections.RowCount());
                countCriteria.ClearOrders();

                listCriteria.
                    SetFirstResult(options.FirstResult).
                    SetMaxResults(options.MaxResults);

                IMultiCriteria multiCriteria = session.CreateMultiCriteria();
                multiCriteria.Add(countCriteria);
                multiCriteria.Add(listCriteria);


                IList queryResult = multiCriteria.List();

                result.TotalItems = (int)((IList)queryResult[0])[0];

                IList recordsList = (IList)queryResult[1];

                EntityConverter entityConverter = new EntityConverter();

                foreach (var e in recordsList)
                {
                    AlbumEntity dataEntity = e as AlbumEntity;
                    Album businessEntity = entityConverter.FromDataEntity(dataEntity, AlbumConvertOptions.Small);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        private DetachedCriteria GetAlbumsImpl(ILoadOptions options)
        {
            if (options == null)
                return null;

            if (options.MaxResults <= 0)
                return null;

            string fieldName = options.FilterField.FieldName;
            string filterValue = options.FilterValue;

            DetachedCriteria criteria = DetachedCriteria.For<AlbumEntity>();

            if (!options.IncludeWaste)
                criteria.Add(Restrictions.Eq("IsWaste", false));

            switch (fieldName)
            {
                case "Tag":
                    {
                        DetachedCriteria tagsCriteria = DetachedCriteria.For<AlbumEntity>().CreateCriteria("Tags").
                            Add(Restrictions.InsensitiveLike("Name", filterValue, MatchMode.Anywhere)).
                            SetProjection(Property.ForName("ID"));

                        criteria.Add(
                            Subqueries.PropertyIn("ID", tagsCriteria)).AddOrder(new Order("ID", true));
                    }
                    break;
                case "Genre":
                    {
                        DetachedCriteria tagsCriteria = DetachedCriteria.For<AlbumEntity>().CreateCriteria("Genres").
                            Add(Restrictions.InsensitiveLike("Name", filterValue, MatchMode.Anywhere)).
                            SetProjection(Property.ForName("ID"));

                        criteria.Add(
                            Subqueries.PropertyIn("ID", tagsCriteria)).AddOrder(new Order("ID", true));
                    }
                    break;
                case "Artist":
                    {
                        DetachedCriteria tagsCriteria = DetachedCriteria.For<AlbumEntity>().CreateCriteria("Artists").
                            Add(Restrictions.InsensitiveLike("Name", filterValue, MatchMode.Anywhere)).
                            SetProjection(Property.ForName("ID"));

                        criteria.Add(
                            Subqueries.PropertyIn("ID", tagsCriteria)).AddOrder(new Order("ID", true));
                    }
                    break;
                default:
                    {
                        criteria.
                            Add(Restrictions.InsensitiveLike(fieldName, filterValue, MatchMode.Anywhere)).
                            AddOrder(new Order(fieldName, true));
                    }
                    break;
            }

            return criteria;
        }

        public bool SaveAlbum(Album album)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                AlbumEntity dataEntity = converter.FromBusinessEntity(album);

                session.Merge(dataEntity);

                tx.Commit();

                album.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool SaveAlbumWasted(Album album)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                var dataEntity = session.Load<AlbumEntity>(album.ID);
                dataEntity.IsWaste = album.IsWaste;

                session.Update(dataEntity);

                tx.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool RemoveAlbum(Album album)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {

                AlbumEntity dataEntity = session.CreateCriteria<AlbumEntity>().
                    Add(Restrictions.Eq("ID", album.ID)).UniqueResult<AlbumEntity>();

                if (dataEntity != null)
                {
                    session.Delete(dataEntity);

                    tx.Commit();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public IPagedList<Genre> GetGenres(ILoadOptions options)
        {
            IPagedList<Genre> result = new PagedList<Genre>();

            if (options == null)
                return result;

            if (options.MaxResults <= 0)
                return result;

            ISession session = SessionFactory.GetSession();

            try
            {
                DetachedCriteria countCriteria = GetGenresImpl(options);
                DetachedCriteria listCriteria = GetGenresImpl(options);

                countCriteria.SetProjection(Projections.RowCount());
                countCriteria.ClearOrders();

                listCriteria.
                    SetFirstResult(options.FirstResult).
                    SetMaxResults(options.MaxResults);

                IMultiCriteria multiCriteria = session.CreateMultiCriteria();
                multiCriteria.Add(countCriteria);
                multiCriteria.Add(listCriteria);


                IList queryResult = multiCriteria.List();

                result.TotalItems = (int)((IList)queryResult[0])[0];

                IList recordsList = (IList)queryResult[1];

                EntityConverter entityConverter = new EntityConverter();

                foreach (var e in recordsList)
                {
                    GenreEntity dataEntity = e as GenreEntity;
                    Genre businessEntity = entityConverter.FromDataEntity(dataEntity);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        private DetachedCriteria GetGenresImpl(ILoadOptions options)
        {
            if (options == null)
                return null;

            if (options.MaxResults <= 0)
                return null;

            string fieldName = options.FilterField.FieldName;
            string filterValue = options.FilterValue;

            DetachedCriteria criteria = DetachedCriteria.For<GenreEntity>();

            criteria.
                Add(Restrictions.InsensitiveLike(fieldName, filterValue, MatchMode.Anywhere)).
                AddOrder(new Order(fieldName, true));

            return criteria;
        }

        public bool SaveGenre(Genre genre)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                GenreEntity dataEntity = converter.FromBusinessEntity(genre);

                session.SaveOrUpdate(dataEntity);

                tx.Commit();

                genre.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public IPagedList<Listening> GetListenings(ILoadOptions options)
        {
            IPagedList<Listening> result = new PagedList<Listening>();

            if (options == null)
                return result;

            if (options.MaxResults <= 0)
                return result;

            ISession session = SessionFactory.GetSession();

            try
            {
                DetachedCriteria countCriteria = GetListeningsImpl(options);
                DetachedCriteria listCriteria = GetListeningsImpl(options);

                countCriteria.SetProjection(Projections.RowCount());
                countCriteria.ClearOrders();

                listCriteria.
                    SetFirstResult(options.FirstResult).
                    SetMaxResults(options.MaxResults);

                IMultiCriteria multiCriteria = session.CreateMultiCriteria();
                multiCriteria.Add(countCriteria);
                multiCriteria.Add(listCriteria);


                IList queryResult = multiCriteria.List();

                result.TotalItems = (int)((IList)queryResult[0])[0];

                IList recordsList = (IList)queryResult[1];

                EntityConverter entityConverter = new EntityConverter();

                foreach (var e in recordsList)
                {
                    ListeningEntity dataEntity = e as ListeningEntity;
                    Listening businessEntity = entityConverter.FromDataEntity(dataEntity);
                    result.Add(businessEntity);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }
            finally
            {
                session.Close();
            }

            return result;
        }

        private DetachedCriteria GetListeningsImpl(ILoadOptions options)
        {
            if (options == null)
                return null;

            if (options.MaxResults <= 0)
                return null;

            string fieldName = options.FilterField.FieldName;
            string filterValue = options.FilterValue;

            DetachedCriteria criteria = DetachedCriteria.For<ListeningEntity>();

            switch (fieldName)
            {
                /*
                 *             fields.Add(new Field("Album", "Album"));
        fields.Add(new Field("Date", "Date", FieldTypeEnum.DateInterval));*/
                case "ListenRating":
                    {
                        int intFilterValue = -1;
                        Int32.TryParse(filterValue, out intFilterValue);

                        criteria.
                            Add(Restrictions.Eq(fieldName, intFilterValue));
                    }
                    break;
                case "Album":
                    {
                        DetachedCriteria tagsCriteria = DetachedCriteria.For<ListeningEntity>().CreateCriteria("Album").
                            Add(Restrictions.InsensitiveLike("Name", filterValue, MatchMode.Anywhere)).
                            SetProjection(Property.ForName("ID"));

                        criteria.Add(Subqueries.PropertyIn("ID", tagsCriteria));
                    }
                    break;
                case "Date":
                    {
                        DateInterval interval = DateInterval.Parse(filterValue);

                        if (interval != null)
                        {

                            criteria.
                                Add(Restrictions.Ge("Date", interval.StartDate)).
                                Add(Restrictions.Le("Date", interval.EndDate));
                        }
                    }
                    break;
                default:
                    {
                        criteria.Add(Restrictions.InsensitiveLike(fieldName, filterValue, MatchMode.Anywhere));
                    }
                    break;
            }

            criteria.AddOrder(new Order("Date", false));

            return criteria;
        }

        public bool SaveListening(Listening listening)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                EntityConverter converter = new EntityConverter();

                ListeningEntity dataEntity = converter.FromBusinessEntity(listening);

                session.SaveOrUpdate(dataEntity);

                tx.Commit();

                listening.ID = dataEntity.ID;

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool RemoveListening(Listening listening)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {

                ListeningEntity dataEntity = session.CreateCriteria<ListeningEntity>().
                    Add(Restrictions.Eq("ID", listening.ID)).UniqueResult<ListeningEntity>();

                if (dataEntity != null)
                {
                    session.Delete(dataEntity);

                    tx.Commit();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }

        public bool BulkImportData(IEnumerable<Artist> artists)
        {
            bool result = false;

            ISession session = SessionFactory.GetSession();

            ITransaction tx = session.BeginTransaction();

            try
            {
                var xmlStr = new XmlSerializer().CreateBulkImportXml(artists);

                var query = session.GetNamedQuery("ImportData");
                query.SetParameter("xmlData", xmlStr);

                query.ExecuteUpdate();

                tx.Commit();

                result = true;
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
                tx.Rollback();
            }
            finally
            {
                session.Close();
                tx.Dispose();
            }

            return result;
        }
    }
}
