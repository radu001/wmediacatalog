
using System.IO;
namespace MediaCatalog.Tests.Extensions
{
    public class FileItem<T>
    {
        public FileInfo File { get; set; }

        public T Data { get; set; }
    }
}
