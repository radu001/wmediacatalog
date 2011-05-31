
using System.Collections.Generic;
using BusinessObjects;
using Microsoft.Practices.Prism.Commands;
using Modules.Import.ViewModels.Wizard.Common;
namespace Modules.Import.ViewModels.Wizard
{
    public interface IBulkImportViewModel : IWizardViewModel
    {
        bool ImportCompleted { get; }
        IEnumerable<Artist> Artists { get; }
        string ArtistFilter { get; set; }
        string AlbumFilter { get; set; }
        string GenreFilter { get; set; }

        DelegateCommand<object> BeginImportCommand { get; }
        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> MarkArtistCommand { get; }
    }
}
