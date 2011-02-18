using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class ArtistEntity : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Biography { get; set; }
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
        public IList<TagEntity> Tags
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

        private IList<AlbumEntity> albums = new List<AlbumEntity>();
        private IList<TagEntity> tags = new List<TagEntity>();
    }
}
