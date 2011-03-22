
using System.Xml.Linq;
namespace MediaCatalog.Tests.Helpers
{
    public interface IFileDataResolver<T>
    {
        T GetFileData(XElement fileElement);
    }
}
