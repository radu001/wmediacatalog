using System.Collections.ObjectModel;
using BusinessObjects;
using Common.ViewModels;
using Microsoft.Practices.Composite.Presentation.Commands;

namespace Modules.Listenings.ViewModels
{
    public interface IListeningEditViewModel : IDialogViewModel
    {
        ObservableCollection<Mood> Moods { get; }
        ObservableCollection<Place> Places { get; }
        Listening Listening { get; set; }

        DelegateCommand<object> SearchAlbumCommand { get; }
        DelegateCommand<object> CreatePlaceCommand { get; }
        DelegateCommand<object> CreateMoodCommand { get; }
    }
}
