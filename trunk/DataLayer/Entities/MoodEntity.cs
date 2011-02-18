using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class MoodEntity : PersistentObject
    {
        public virtual string Name { get; set; }
        public virtual string PrivateMarks { get; set; }
        public virtual string Comments { get; set; }
        public virtual string Description { get; set; }
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

        private IList<ListeningEntity> listens = new List<ListeningEntity>();
    }
}
