
namespace Common.Entities.Pagination
{
    public class TagLoadOptions : LoadOptions
    {
        public bool ExcludeAlbums { get; set; }

        public bool ExcludeArtists { get; set; }

        public string EntityName { get; set; }
    }
}
