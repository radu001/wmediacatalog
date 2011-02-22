
using System.Collections.Generic;
using BusinessObjects;
using Common.Entities.Pagination;
using DataServices;
namespace Modules.Albums.Services
{
    public class DataService : IDataService
    {
        #region IDataService Members

        public IPagedList<Artist> GetArtists(ILoadOptions options)
        {
            DataProvider provider = new DataProvider();
            return provider.GetArtists(options);
        }

        public IPagedList<Album> GetAlbums(ILoadOptions options)
        {
            DataProvider provider = new DataProvider();
            return provider.GetAlbums(options);
        }

        public Album GetAlbum(int albumID)
        {
            DataProvider provider = new DataProvider();
            return provider.GetAlbum(albumID);
        }

        public bool SaveAlbumWasted(Album album)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveAlbumWasted(album);
        }


        public IList<Tag> GetTags()
        {
            DataProvider provider = new DataProvider();
            return provider.GetTags();
        }

        public IPagedList<Genre> GetGenres(ILoadOptions options)
        {
            DataProvider provider = new DataProvider();
            return provider.GetGenres(options);
        }

        public bool SaveAlbum(Album album)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveAlbum(album);
        }

        public bool SaveGenre(Genre genre)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveGenre(genre);
        }

        #endregion
    }
}
