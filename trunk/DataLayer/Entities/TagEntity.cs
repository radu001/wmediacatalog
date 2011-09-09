using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class TagEntity : PersistentObject
    {

        public virtual string Name { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Comments { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual string Color { get; set; }
        public virtual string TextColor { get; set; }
        public virtual int AssociatedEntitiesCount { get; set; }
        public virtual IList<AlbumEntity> Albums
        {
            get
            {
                return albums;
            }
            set
            {
                albums = value;
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

        private IList<AlbumEntity> albums = new List<AlbumEntity>();
        private IList<ArtistEntity> artists = new List<ArtistEntity>();
    }
}
