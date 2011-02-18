
namespace DataLayer.Entities
{
    public class TrackEntity : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual int Index { get; set; }
        public virtual int Length { get; set; }
        public virtual AlbumEntity Album { get; set; }
    }
}
