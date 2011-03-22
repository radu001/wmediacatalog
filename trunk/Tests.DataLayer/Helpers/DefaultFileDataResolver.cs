using System.Xml.Linq;

namespace MediaCatalog.Tests.Helpers
{
    public class DefaultFileDataResolver<T> : IFileDataResolver<T>
    {
        #region IFileDataResolver<T> Members

        public T GetFileData(XElement fileElement)
        {
            return default(T);
        }

        #endregion
    }
}
