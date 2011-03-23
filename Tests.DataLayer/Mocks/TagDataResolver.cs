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
            FileTagCollection result = new FileTagCollection();

            var tagElements = fileElement.Descendants("tag");
            foreach (var te in tagElements)
            {
                FileTag tag = new FileTag()
                {
                    Key = te.Attribute("key").Value,
                    Value = te.Attribute("value").Value
                };
                result.Add(tag);
            }

            return result;
        }

        #endregion
    }
}
