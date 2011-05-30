
using System.Collections.Generic;
namespace DataLayer.Entities
{
    public class UserEntity : PersistentObject
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Settings { get; set; }

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

        private IList<UserLoginEntity> logins = new List<UserLoginEntity>();
    }
}
