
using System.Collections.Generic;
namespace BusinessObjects
{
    /// <summary>
    /// Stores information about currently logged user
    /// </summary>
    public static class CurrentUser
    {
        public static void Assign(User user)
        {
            body = new User(user);
        }

        public static IList<UserSettings> GetSettings()
        {
            return settings;
        }

        public static void SetSettings(IList<UserSettings> settings)
        {
            CurrentUser.settings = settings;
        }

        private static User body;
        private static IList<UserSettings> settings;
    }
}
