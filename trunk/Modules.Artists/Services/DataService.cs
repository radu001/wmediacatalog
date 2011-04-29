
using System.Collections.Generic;
using BusinessObjects;
using Common.Entities.Pagination;
using DataServices;
namespace Modules.Artists.Services
{
    public class DataService : IDataService
    {
        #region IDataService Members

        public bool ArtistExists(string artistName)
        {
            DataProvider provider = new DataProvider();
            return provider.ArtistExists(artistName);
        }

        public IPagedList<Artist> GetArtists(ILoadOptions options)
        {
            DataProvider provider = new DataProvider();
            return provider.GetArtists(options);
        }

        public Artist GetArtist(int artistID)
        {
            DataProvider provider = new DataProvider();
            return provider.GetArtist(artistID);
        }

        public IList<Tag> GetTags()
        {
            DataProvider provider = new DataProvider();
            return provider.GetTags();
        }

        public bool SaveArtist(Artist artist)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveArtist(artist);
        }

        public bool SaveArtistWasted(Artist artist)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveArtistWasted(artist);
        }

        public bool RemoveArtist(Artist artist)
        {
            DataProvider provider = new DataProvider();
            return provider.RemoveArtist(artist);
        }

        public IList<Album> GetAlbumsByArtistID(int artistID)
        {
            DataProvider provider = new DataProvider();
            return provider.GetAlbumsByArtistID(artistID);
        }

        #endregion
    }
}
