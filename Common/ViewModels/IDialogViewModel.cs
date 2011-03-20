
using Microsoft.Practices.Prism.Commands;
namespace Common.ViewModels
{
    public interface IDialogViewModel
    {
        bool? DialogResult { get; }
        bool IsBusy { get; }
        bool IsEditMode { get; set; }

        DelegateCommand<object> CancelCommand { get; }
        DelegateCommand<object> SuccessCommand { get; }
    }
}
