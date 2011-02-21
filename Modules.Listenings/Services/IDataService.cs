
using System.Collections.Generic;
using BusinessObjects;
using Common.Entities.Pagination;
namespace Modules.Listenings.Services
{
    public interface IDataService
    {
        IPagedList<Album> GetAlbums(ILoadOptions loadOptions);
        IPagedList<Listening> GetListenings(ILoadOptions loadOptions);
        IList<Mood> GetMoods();
        IList<Place> GetPlaces();
        bool SaveListening(Listening listening);
        bool SaveMood(Mood mood);
        bool SavePlace(Place place);
        bool RemoveListening(Listening listening);
    }
}
