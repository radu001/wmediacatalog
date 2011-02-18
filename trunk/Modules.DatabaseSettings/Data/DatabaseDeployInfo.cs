using System;
using System.ComponentModel;

namespace Modules.DatabaseSettings.Data
{
    public class DatabaseDeployInfo : INotifyPropertyChanged
    {
        public string DatabaseName
        {
            get
            {
                return databaseName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty database name");

                databaseName = value;
                OnPropertyChanged("DatabaseName");
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty database user's login");

                userName = value;
                OnPropertyChanged("UserName");
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
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty database user's password");

                password = value;
                OnPropertyChanged("Password");
            }
        }

        public string AdminLogin
        {
            get
            {
                return adminLogin;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty admin login");

                adminLogin = value;
                OnPropertyChanged("AdminLogin");
            }
        }

        public string AdminPassword
        {
            get
            {
                return adminPassword;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty admin password");

                adminPassword = value;
                OnPropertyChanged("AdminPassword");
            }
        }

        public string PsqlPath
        {
            get
            {
                return psqlPath;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new Exception("Incorrect empty psql binary path");

                psqlPath = value;
                OnPropertyChanged("PsqlPath");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private fields

        private string databaseName;
        private string userName;
        private string password;

        private string adminLogin;
        private string adminPassword;
        private string psqlPath;

        #endregion
    }
}
