
namespace DbTool
{
    public interface IViewModel
    {
        DelegateCommand<object> ViewLoadedCommand { get; }
    }
}
