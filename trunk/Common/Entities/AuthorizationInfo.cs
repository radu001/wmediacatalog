
using System;
namespace Common.Entities
{
    public class AuthorizationInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime LoginDate { get; set; }

        public AuthorizationInfo() { }

        public AuthorizationInfo(string userName, string password)
        {
            UserName = userName;
            Password = password;
            LoginDate = DateTime.Now;
        }
    }
}
