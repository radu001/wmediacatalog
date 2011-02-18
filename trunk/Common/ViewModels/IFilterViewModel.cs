
using System.Collections.ObjectModel;
using Common.Entities.Pagination;
using Common.Enums;
namespace Common.ViewModels
{
    public interface IFilterViewModel
    {
        IField SelectedField { get; set; }
        string SelectedValue { get; set; }
        ObservableCollection<IField> Fields { get; set; }
        bool GroupingEnabled { get; set; }

        IField GetInitialField();
    }
}
