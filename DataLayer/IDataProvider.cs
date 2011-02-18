using System.Collections.Generic;
using DataLayer.Entities;

namespace DataLayer
{
    public interface IDataProvider
    {
        bool SaveOrUpdate(ArtistEntity artist);
        bool SaveOrUpdate(GenreEntity genre);
        bool SaveOrUpdate(AlbumEntity album);
        bool SaveOrUpdate(MoodEntity mood);
        bool SaveOrUpdate(TagEntity tag);
        bool SaveOrUpdate(ListeningEntity listening);

        IList<ArtistEntity> GetArtists();
        IList<GenreEntity> GetGenres();
        IList<AlbumEntity> GetAlbums();
        IList<MoodEntity> GetMoods();
        IList<TagEntity> GetTags();
    }
}
