
using Modules.Import.Model;
namespace Modules.Import.Services.Utils
{
    public interface IScanner
    {
        FileTagCollection GetTags(string filePath);
    }
}
