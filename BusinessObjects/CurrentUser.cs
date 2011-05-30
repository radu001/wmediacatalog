
namespace BusinessObjects
{
    /// <summary>
    /// Stores information about currently logged user
    /// </summary>
    public static class CurrentUser
    {
        public static void Assign(User user)
        {
            body = user;
        }

        public static User GetUser()
        {
            return body;
        }

        public static UserSettings GetSettings()
        {
            return body.Settings;
        }

        private static User body;
    }
}
