
using System;
using System.Collections.Generic;
using BusinessObjects;
using Common.Commands;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
namespace Modules.Artists.ViewModels
{
    public interface IArtistEditViewModel : IDialogViewModel
    {
        Artist Artist { get; set; }

        Func<object, string, bool> FilterTag { get; }
        IList<Tag> Tags { get; set; }
        string TagName { get; set; }

        DelegateCommand<object> AttachTagCommand { get; }
        DelegateCommand<object> DetachTagCommand { get; }
        DelegateCommand<MouseDoubleClickArgs> EditTagCommand { get; }
    }
}
