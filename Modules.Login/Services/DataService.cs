
using System;
using BusinessObjects;
using DataServices;
namespace Modules.Login.Services
{
    public class DataService : IDataService
    {
        #region IDataService Members

        public Exception ValidateConnection()
        {
            DataProvider provider = new DataProvider();
            return provider.ValidateConnection();
        }

        public User ValidateCredentials(string userName, string password)
        {
            DataProvider provider = new DataProvider();
            return provider.GetUser(userName, password);
        }

        public void UserLoggedIn(User user)
        {
            DataProvider provider = new DataProvider();
            provider.UserLoggedIn(user);
        }

        public bool UserExists(string userName)
        {
            DataProvider provider = new DataProvider();
            return provider.UserExists(userName);
        }

        public bool SaveUser(User user)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveUser(user);
        }

        #endregion
    }
}
