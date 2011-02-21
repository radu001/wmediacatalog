
using System.Collections.Generic;
using BusinessObjects;
using Common.Entities.Pagination;
using DataServices;
namespace Modules.Listenings.Services
{
    public class DataService : IDataService
    {
        #region IDataService Members

        public IPagedList<Album> GetAlbums(ILoadOptions loadOptions)
        {
            DataProvider provider = new DataProvider();
            return provider.GetAlbums(loadOptions);
        }

        public IPagedList<Listening> GetListenings(ILoadOptions loadOptions)
        {
            DataProvider provider = new DataProvider();
            return provider.GetListenings(loadOptions);
        }

        public bool SaveListening(Listening listening)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveListening(listening);
        }

        public IList<Mood> GetMoods()
        {
            DataProvider provider = new DataProvider();
            return provider.GetMoods();
        }

        public IList<Place> GetPlaces()
        {
            DataProvider provider = new DataProvider();
            return provider.GetPlaces();
        }

        public bool SaveMood(Mood mood)
        {
            DataProvider provider = new DataProvider();
            return provider.SaveMood(mood);
        }

        public bool SavePlace(Place place)
        {
            DataProvider provider = new DataProvider();
            return provider.SavePlace(place);
        }

        public bool RemoveListening(Listening listening)
        {
            DataProvider provider = new DataProvider();
            return provider.RemoveListening(listening);
        }

        #endregion
    }
}
