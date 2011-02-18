
using System.Collections.Generic;
using BusinessObjects;
using System;
namespace Modules.Login.Services
{
    public interface IDataService
    {
        Exception ValidateConnection();
        
        User ValidateCredentials(string userName, string password);
        void UserLoggedIn(User user);
        IList<UserSettings> GetUserSettings(User user);
        
        bool UserExists(string userName);
        bool SaveUser(User user);
    }
}
