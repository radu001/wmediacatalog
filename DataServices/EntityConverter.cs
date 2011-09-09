
using System;
using System.Linq;
using BusinessObjects;
using BusinessObjects.Artificial;
using DataLayer.Entities;
using DataServices.Enums;
namespace DataServices
{
    public class EntityConverter
    {
        public User FromDataEntity(UserEntity dataEntity)
        {
            if (dataEntity == null)
                return null;

            User businessEntity = new User()
            {
                ID = dataEntity.ID,
                UserName = dataEntity.UserName,
                Password = dataEntity.Password,
                Settings = new UserSettings(dataEntity.Settings)
            };

            return businessEntity;
        }

        public UserEntity FromBusinessEntity(User businessEntity)
        {
            if (businessEntity == null)
                return null;

            UserEntity dataEntity = new UserEntity()
            {
                ID = businessEntity.ID,
                UserName = businessEntity.UserName,
                Password = businessEntity.Password,
                Settings = businessEntity.Settings.ToXml()
            };

            return dataEntity;
        }

        public Artist FromDataEntity(ArtistEntity dataEntity, ArtistConvertOptions options)
        {
            if (dataEntity == null)
                return null;

            Artist businessEntity = new Artist()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Biography = dataEntity.Biography,
                IsWaste = dataEntity.IsWaste
            };

            if (options == ArtistConvertOptions.Full)
            {
                foreach (TagEntity tagDataEntity in dataEntity.Tags)
                {
                    businessEntity.Tags.Add(FromDataEntity(tagDataEntity));
                }

                foreach (AlbumEntity albumEntity in dataEntity.Albums)
                {
                    businessEntity.Albums.Add(FromDataEntity(albumEntity, AlbumConvertOptions.Small));
                }
            }

            return businessEntity;
        }

        public ArtistEntity FromBusinessEntity(Artist businessEntity, ArtistConvertOptions options)
        {
            if (businessEntity == null)
                return null;

            ArtistEntity dataEntity = new ArtistEntity()
            {
                ID = businessEntity.ID,
                Name = businessEntity.Name,
                PrivateMarks = businessEntity.PrivateMarks,
                Biography = businessEntity.Biography,
                IsWaste = businessEntity.IsWaste
            };

            if (options == ArtistConvertOptions.Full)
            {
                foreach (Tag tagBusinessEntity in businessEntity.Tags)
                {
                    TagEntity tagDataEntity = FromBusinessEntity(tagBusinessEntity);
                    dataEntity.Tags.Add(tagDataEntity);
                }

                foreach (Album albumBusinessEntity in businessEntity.Albums)
                {
                    AlbumEntity albumDataEntity = FromBusinessEntity(albumBusinessEntity);
                    dataEntity.Albums.Add(albumDataEntity);
                }
            }

            return dataEntity;
        }

        public Tag FromDataEntity(TagEntity dataEntity)
        {
            if (dataEntity == null)
                return null;

            Tag businessEntity = new Tag()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Comments = dataEntity.Comments,
                CreateDate = dataEntity.CreateDate,
                Description = dataEntity.Description,
                AssociatedEntitiesCount = dataEntity.AssociatedEntitiesCount,
                Color = dataEntity.Color,
                TextColor = dataEntity.TextColor
            };

            return businessEntity;
        }

        public TagEntity FromBusinessEntity(Tag businessEntity)
        {
            if (businessEntity == null)
                return null;

            TagEntity dataEntity = new TagEntity()
            {
                ID = businessEntity.ID,
                Name = businessEntity.Name,
                PrivateMarks = businessEntity.PrivateMarks,
                Description = businessEntity.Description,
                CreateDate = businessEntity.CreateDate,
                Comments = businessEntity.Comments,
                Color = businessEntity.Color,
                TextColor = businessEntity.TextColor
            };

            return dataEntity;
        }

        public Mood FromDataEntity(MoodEntity dataEntity, MoodConvertOptions options)
        {
            if (dataEntity == null)
                return null;

            Mood businessEntity = new Mood()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Comments = dataEntity.Comments,
                Description = dataEntity.Description
            };

            return businessEntity;
        }

        public Place FromDataEntity(PlaceEntity dataEntity, PlaceConvertOptions options)
        {
            if (dataEntity == null)
                return null;

            Place businessEntity = new Place()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Comments = dataEntity.Comments,
                Description = dataEntity.Description
            };

            return businessEntity;
        }

        public Track FromDataEntity(TrackEntity dataEntity, Album parent)
        {
            if (dataEntity == null || parent == null)
                return null;

            Track businessEntity = new Track()
            {
                ID = dataEntity.ID,
                Index = dataEntity.Index,
                Length = new TimeSpan(0, 0, 0, dataEntity.Length, 0),
                Name = dataEntity.Name,
                Album = parent
            };

            return businessEntity;
        }

        public TrackEntity FromBusinessEntity(Track businessEntity, AlbumEntity parent)
        {
            if (businessEntity == null || parent == null)
                return null;

            TrackEntity dataEntity = new TrackEntity()
            {
                ID = businessEntity.ID,
                Index = businessEntity.Index,
                Length = (int)businessEntity.Length.TotalSeconds,
                Name = businessEntity.Name,
                Album = parent
            };

            return dataEntity;
        }

        public Album FromDataEntity(AlbumEntity dataEntity, AlbumConvertOptions options)
        {
            if (dataEntity == null)
                return null;

            Album businessEntity = new Album()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Label = dataEntity.Label,
                ASIN = dataEntity.ASIN,
                FreedbID = dataEntity.FreedbID,
                Description = dataEntity.Description,
                Year = dataEntity.Year,
                Rating = dataEntity.Rating,
                DiscsCount = dataEntity.DiscsCount,
                IsWaste = dataEntity.IsWaste
            };

            if (options == AlbumConvertOptions.Full)
            {
                foreach (TagEntity tagDataEntity in dataEntity.Tags)
                {
                    businessEntity.Tags.Add(FromDataEntity(tagDataEntity));
                }

                foreach (GenreEntity genreDataEntity in dataEntity.Genres)
                {
                    businessEntity.Genres.Add(FromDataEntity(genreDataEntity));
                }

                foreach (ArtistEntity artistDataEntity in dataEntity.Artists)
                {
                    businessEntity.Artists.Add(FromDataEntity(artistDataEntity, ArtistConvertOptions.Small));
                }

                foreach (TrackEntity trackDataEntity in dataEntity.Tracks)
                {
                    businessEntity.Tracks.Add(FromDataEntity(trackDataEntity, businessEntity));
                }

                dataEntity.Tracks.OrderBy(t => t.Index);
            }

            return businessEntity;
        }

        public AlbumEntity FromBusinessEntity(Album businessEntity)
        {
            if (businessEntity == null)
                return null;

            AlbumEntity dataEntity = new AlbumEntity()
            {
                ID = businessEntity.ID,
                Name = businessEntity.Name,
                PrivateMarks = businessEntity.PrivateMarks,
                Label = businessEntity.Label,
                ASIN = businessEntity.ASIN,
                FreedbID = businessEntity.FreedbID,
                Description = businessEntity.Description,
                Year = businessEntity.Year,
                Rating = businessEntity.Rating,
                DiscsCount = businessEntity.DiscsCount,
                IsWaste = businessEntity.IsWaste
            };

            foreach (Tag tagBusinessEntity in businessEntity.Tags)
            {
                dataEntity.Tags.Add(FromBusinessEntity(tagBusinessEntity));
            }

            foreach (Genre genreBusinessEntity in businessEntity.Genres)
            {
                dataEntity.Genres.Add(FromBusinessEntity(genreBusinessEntity));
            }

            foreach (Artist artistBusinessEntity in businessEntity.Artists)
            {
                dataEntity.Artists.Add(FromBusinessEntity(artistBusinessEntity, ArtistConvertOptions.Small));
            }

            foreach (Track trackBusinessEntity in businessEntity.Tracks)
            {
                dataEntity.Tracks.Add(FromBusinessEntity(trackBusinessEntity, dataEntity));
            }

            return dataEntity;
        }

        public Genre FromDataEntity(GenreEntity dataEntity)
        {
            if (dataEntity == null)
                return null;

            Genre businessEntity = new Genre()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                PrivateMarks = dataEntity.PrivateMarks,
                Description = dataEntity.Description,
                Comments = dataEntity.Comments
            };

            return businessEntity;
        }

        public GenreEntity FromBusinessEntity(Genre businessEntity)
        {
            if (businessEntity == null)
                return null;

            GenreEntity dataEntity = new GenreEntity()
            {
                ID = businessEntity.ID,
                Name = businessEntity.Name,
                PrivateMarks = businessEntity.PrivateMarks,
                Description = businessEntity.Description,
                Comments = businessEntity.Comments
            };

            return dataEntity;
        }

        public ListeningEntity FromBusinessEntity(Listening businessEntity)
        {
            if (businessEntity == null)
                return null;

            ListeningEntity dataEntity = new ListeningEntity()
            {
                ID = businessEntity.ID,
                ListenRating = businessEntity.ListenRating,
                PrivateMarks = businessEntity.PrivateMarks,
                Review = businessEntity.Review,
                Comments = businessEntity.Comments,
                Date = businessEntity.Date,
                Album = new AlbumEntity()
                {
                    ID = businessEntity.Album.ID
                },
                Mood = FromBusinessEntity(businessEntity.Mood),
                Place = FromBusinessEntity(businessEntity.Place)
            };

            return dataEntity;
        }

        public Listening FromDataEntity(ListeningEntity dataEntity)
        {
            if (dataEntity == null)
                return null;

            Listening businessEntity = new Listening()
            {
                ID = dataEntity.ID,
                Date = dataEntity.Date,
                ListenRating = dataEntity.ListenRating,
                PrivateMarks = dataEntity.PrivateMarks,
                Review = dataEntity.Review,
                Comments = dataEntity.Comments
            };

            businessEntity.Album = FromDataEntity(dataEntity.Album, AlbumConvertOptions.Small);
            businessEntity.Mood = FromDataEntity(dataEntity.Mood, MoodConvertOptions.Short);
            businessEntity.Place = FromDataEntity(dataEntity.Place, PlaceConvertOptions.Short);

            return businessEntity;
        }

        public MoodEntity FromBusinessEntity(Mood mood)
        {
            if (mood == null)
                return null;

            return new MoodEntity()
            {
                ID = mood.ID,
                Name = mood.Name,
                Comments = mood.Comments,
                PrivateMarks = mood.PrivateMarks
            };
        }

        public PlaceEntity FromBusinessEntity(Place place)
        {
            if (place == null)
                return null;

            return new PlaceEntity()
            {
                ID = place.ID,
                Name = place.Name,
                Comments = place.Comments,
                PrivateMarks = place.PrivateMarks
            };
        }

        public TaggedObject FromDataEntity(TaggedObjectEntity dataEntity)
        {
            if (dataEntity == null)
                return null;

            var businessEntity = new TaggedObject()
            {
                ID = dataEntity.ID,
                Name = dataEntity.Name,
                ObjectType = (TaggedObjectType)dataEntity.ObjectType
            };

            return businessEntity;
        }
    }
}
