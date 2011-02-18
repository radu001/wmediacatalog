using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class AlbumEntity : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Label { get; set; }
        public virtual string ASIN { get; set; }
        public virtual string FreedbID { get; set; }
        public virtual DateTime Year { get; set; }
        public virtual int DiscsCount { get; set; }
        public virtual int Rating { get; set; }

        public virtual IList<GenreEntity> Genres
        {
            get
            {
                return genres;
            }
            set
            {
                genres = value;
            }
        }
        public virtual IList<ArtistEntity> Artists
        {
            get
            {
                return artists;
            }
            set
            {
                artists = value;
            }
        }
        public virtual IList<TrackEntity> Tracks
        {
            get
            {
                return tracks;
            }
            set
            {
                tracks = value;
            }
        }
        public virtual IList<TagEntity> Tags
        {
            get
            {
                return tags;
            }
            set
            {
                tags = value;
            }
        }
        public virtual IList<ListeningEntity> Listens
        {
            get
            {
                return listens;
            }
            set
            {
                listens = value;
            }
        }

        private IList<ArtistEntity> artists = new List<ArtistEntity>();
        private IList<GenreEntity> genres = new List<GenreEntity>();
        private IList<TrackEntity> tracks = new List<TrackEntity>();
        private IList<ListeningEntity> listens = new List<ListeningEntity>();
        private IList<TagEntity> tags = new List<TagEntity>();
    }
}
