
using System.Xml.Linq;
namespace MediaCatalog.Tests.Extensions
{
    public interface IFileDataResolver<T>
    {
        T GetFileData(XElement fileElement);
    }
}
