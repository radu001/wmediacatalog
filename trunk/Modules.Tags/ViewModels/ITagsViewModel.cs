
using Microsoft.Practices.Prism.Commands;
using BusinessObjects;
using System.Collections.ObjectModel;
using TagCloudLib;
namespace Modules.Tags.ViewModels
{
    public interface ITagsViewModel
    {
        ObservableCollection<ITag> Tags { get; }

        DelegateCommand<object> ViewLoadedCommand { get; }
    }
}
