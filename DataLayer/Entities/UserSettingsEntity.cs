
namespace DataLayer.Entities
{
    public class UserSettingsEntity : PersistentObject
    {
        public virtual UserEntity User { get; set; }
        public virtual string Value1 { get; set; }
    }
}
