
using System;
namespace DataLayer.Entities
{
    public class UserLoginEntity : PersistentObject
    {
        public virtual UserEntity User { get; set; }
        public virtual DateTime LoginDate { get; set; }
    }
}
