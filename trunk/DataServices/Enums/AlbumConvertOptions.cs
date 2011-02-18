
namespace DataServices.Enums
{
    /// <summary>
    /// Indicates that additional dependencies graph should be loaded from database
    /// </summary>
    public enum AlbumConvertOptions
    {
        Unknown = 0,

        /// <summary>
        /// Load only basic data
        /// </summary>
        Small = 1,

        /// <summary>
        /// Load all data (including genres, tags, artists etc)
        /// </summary>
        Full = 2
    }
}
