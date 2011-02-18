using System;

namespace DataLayer.Entities
{
    public class ListeningEntity : PersistentObject
    {
        public virtual DateTime Date { get; set; }
        public virtual string Review { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Comments { get; set; }
        public virtual int ListenRating { get; set; }
        public virtual MoodEntity Mood { get; set; }
        public virtual PlaceEntity Place { get; set; }
        public virtual AlbumEntity Album { get; set; }
    }
}
