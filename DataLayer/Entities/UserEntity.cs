
using System.Collections.Generic;
namespace DataLayer.Entities
{
    public class UserEntity : PersistentObject
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }

        public virtual IList<UserLoginEntity> Logins
        {
            get
            {
                return logins;
            }
            set
            {
                logins = value;
            }
        }
        public virtual IList<UserSettingsEntity> Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        private IList<UserLoginEntity> logins = new List<UserLoginEntity>();
        private IList<UserSettingsEntity> settings = new List<UserSettingsEntity>();
    }
}
