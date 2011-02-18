using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class GenreEntity : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Comments { get; set; }
        public virtual string Description { get; set; }
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

        private IList<AlbumEntity> albums = new List<AlbumEntity>();
    }
}
