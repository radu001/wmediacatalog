
using BusinessObjects;
using Microsoft.Practices.Composite.Presentation.Commands;
using Common.ViewModels;
namespace Modules.Tags.ViewModels
{
    public interface ITagEditViewModel : IDialogViewModel
    {
        Tag Tag { get; set; }
    }
}
