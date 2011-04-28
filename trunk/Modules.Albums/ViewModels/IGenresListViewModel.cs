
using System.Collections.Generic;
using BusinessObjects;
using Common.Commands;
using Common.Controls.Controls;
using Common.Entities.Pagination;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Albums.ViewModels
{
    public interface IGenresListViewModel : IEventSubscriber
    {
        IList<Genre> Genres { get; }
        IList<Genre> SelectedGenres { get; }
        ILoadOptions LoadOptions { get; }
        IField FilterField { get; }
        int TotalGenresCount { get; }

        bool IsGenresListVisible { get; }
        string GenresFilterValue { get; set; }

        DelegateCommand<object> HideShowGenresListCommand { get; }
        DelegateCommand<object> AttachGenresCommand { get; }
        DelegateCommand<object> DetachGenresCommand { get; }
        DelegateCommand<MultiSelectionChangedArgs> SelectedGenresChangedCommand { get; }
        DelegateCommand<MouseMoveArgs> DragGenresCommand { get; }
        DelegateCommand<object> CreateGenreCommand { get; }
        DelegateCommand<PageChangedArgs> PageChangedCommand { get; }
        DelegateCommand<MouseDoubleClickArgs> EditGenreCommand { get; }
    }
}
