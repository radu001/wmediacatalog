
using System.Collections.Generic;
using System.Windows.Controls;
using BusinessObjects;
using Common.Commands;
using Microsoft.Practices.Composite.Presentation.Commands;
using Common.ViewModels;
namespace Modules.Artists.ViewModels
{
    public interface IArtistEditViewModel : IDialogViewModel
    {
        Artist Artist { get; set; }

        AutoCompleteFilterPredicate<object> FilterTagCommand { get; }

        IList<Tag> Tags { get; set; }
        Tag SelectedTag { get; set; }
        string NewTagName { get; set; }

        DelegateCommand<object> AttachTagCommand { get; }
        DelegateCommand<KeyDownArgs> AttachTagKeyboardCommand { get; }
        DelegateCommand<object> DetachTagCommand { get; }
        DelegateCommand<MouseDoubleClickArgs> EditTagCommand { get; }
    }
}
