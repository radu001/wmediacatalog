
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

        public static UserSettings GetSettings()
        {
            return settings;
        }

        public static void SetSettings(UserSettings settings)
        {
            CurrentUser.settings = settings;
        }

        private static User body;
        private static UserSettings settings;
    }
}
