
using BusinessObjects;
using Common.ViewModels;
namespace Modules.Albums.ViewModels
{
    public interface IGenreEditViewModel : IDialogViewModel
    {
        Genre Genre { get; set; }
    }
}
