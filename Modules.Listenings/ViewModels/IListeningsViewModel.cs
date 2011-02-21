
using System.Collections.ObjectModel;
using BusinessObjects;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Microsoft.Practices.Composite.Presentation.Commands;
namespace Modules.Listenings.ViewModels
{
    public interface IListeningsViewModel
    {
        ILoadOptions LoadOptions { get; }
        ObservableCollection<Listening> ListeningsCollection { get; }
        int ListeningsCount { get; }
        Listening SelectedListening { get; set; }

        DelegateCommand<object> ViewLoadedCommand { get; }
        DelegateCommand<object> AddListeningCommand { get; }
        DelegateCommand<object> RemoveListeningCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
        DelegateCommand<object> DisplayListeningCommand { get; }
    }
}
