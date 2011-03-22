using System.Xml.Linq;
using MediaCatalog.Tests.Extensions;
using Modules.Import.Model;

namespace MediaCatalog.Tests.Mocks
{
    public class TagDataResolver : IFileDataResolver<FileTagCollection>
    {
        #region IFileDataResolver<IEnumerable<FileTag>> Members

        public FileTagCollection GetFileData(XElement fileElement)
        {
            return null;
        }

        #endregion
    }
}
