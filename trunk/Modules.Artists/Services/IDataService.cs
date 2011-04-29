
using System.Collections.Generic;
using BusinessObjects;
using Common.Entities.Pagination;
namespace Modules.Artists.Services
{
    public interface IDataService
    {
        bool ArtistExists(string artistName);
        IPagedList<Artist> GetArtists(ILoadOptions options);
        Artist GetArtist(int artistID);
        IList<Tag> GetTags();
        bool SaveArtist(Artist artist);
        bool SaveArtistWasted(Artist artist);
        bool RemoveArtist(Artist artist);
        IList<Album> GetAlbumsByArtistID(int artistID);
    }
}
