
namespace DataServices.Enums
{
    /// <summary>
    /// Indicates that additional dependencies graph should be loaded from database
    /// </summary>
    public enum ArtistConvertOptions
    {
        Unknown = 0,

        /// <summary>
        /// Load only basic data
        /// </summary>
        Small = 1,

        /// <summary>
        /// Load all data (including tags)
        /// </summary>
        Full = 2
    }
}
