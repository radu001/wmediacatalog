using System.Collections.ObjectModel;
using BusinessObjects;
using Microsoft.Practices.Prism.Commands;

namespace Modules.Import.ViewModels
{
    public interface IImportViewModel
    {
        DelegateCommand<object> ScanFilesCommand { get; }

        ObservableCollection<Artist> ImportedArtists { get; }
    }
}
