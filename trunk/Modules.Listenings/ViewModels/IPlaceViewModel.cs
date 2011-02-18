
using BusinessObjects;
using Common.ViewModels;
namespace Modules.Listenings.ViewModels
{
    public interface IPlaceViewModel : IDialogViewModel
    {
        Place Place { get; set; }
    }
}
