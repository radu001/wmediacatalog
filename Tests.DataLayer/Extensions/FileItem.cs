
using Modules.Import.Services.Utils.FileSystem;
namespace MediaCatalog.Tests.Extensions
{
    public class FileItem<T>
    {
        public FsFile File { get; set; }

        public T Data { get; set; }
    }
}
