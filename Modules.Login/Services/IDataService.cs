
using System;
using BusinessObjects;
namespace Modules.Login.Services
{
    public interface IDataService
    {
        Exception ValidateConnection();
        
        User ValidateCredentials(string userName, string password);
        void UserLoggedIn(User user);
        
        bool UserExists(string userName);
        bool SaveUser(User user);
    }
}
