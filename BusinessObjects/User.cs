
using System;
namespace BusinessObjects
{
    public class User : BusinessObject
    {
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (value != userName)
                {
                    userName = value;
                    NotifyPropertyChanged(() => UserName);
                }
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != password)
                {
                    password = value;
                    NotifyPropertyChanged(() => Password);
                }
            }
        }

        public UserSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                NotifyPropertyChanged(() => Settings);
            }
        }

        public User() { }

        public User(User u)
        {
            if (u == null)
                throw new NullReferenceException("Argument can't be null");

            this.ID = u.ID;
            this.UserName = u.UserName;
            this.Password = u.Password;
        }

        #region Private fields

        private string userName;
        private string password;
        private UserSettings settings;

        #endregion
    }
}
