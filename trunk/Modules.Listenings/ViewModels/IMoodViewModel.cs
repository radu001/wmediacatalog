
using BusinessObjects;
using Common.ViewModels;
namespace Modules.Listenings.ViewModels
{
    public interface IMoodViewModel : IDialogViewModel
    {
        Mood Mood { get; set; }
    }
}
