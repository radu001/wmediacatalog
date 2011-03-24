using System.Collections.ObjectModel;
using BusinessObjects;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;

namespace Modules.Listenings.ViewModels
{
    public interface IListeningEditViewModel : IDialogViewModel
    {
        ObservableCollection<Mood> Moods { get; }
        ObservableCollection<Place> Places { get; }
        Listening Listening { get; set; }
        bool IsViewMode { get; set; }

        DelegateCommand<object> SearchAlbumCommand { get; }
        DelegateCommand<object> CreatePlaceCommand { get; }
        DelegateCommand<object> CreateMoodCommand { get; }
    }
}
