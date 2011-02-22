using Microsoft.Practices.Composite.Presentation.Commands;

namespace Common.ViewModels
{
    public interface IWasteableViewModel
    {
        bool IsWasteVisible { get; set; }

        DelegateCommand<object> ShowWasteCommand { get; }
        DelegateCommand<object> HideWasteCommand { get; }
        DelegateCommand<object> MarkAsWasteCommand { get; }
    }
}
