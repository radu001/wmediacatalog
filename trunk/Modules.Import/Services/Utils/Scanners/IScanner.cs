
using Modules.Import.Model;
namespace Modules.Import.Services.Utils.Scanners
{
    public interface IScanner
    {
        FileTagCollection GetTags(string filePath);
    }
}
