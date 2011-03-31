
using System.Collections.Generic;
using BusinessObjects;
namespace Modules.Import.Model
{
    public interface IImportDataModel : IEnumerable<Artist>
    {
        int ArtistsCount { get; }
        int AlbumsCount { get; }
        int GenresCount { get; }
    }
}
