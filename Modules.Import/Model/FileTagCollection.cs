using System.Collections.Generic;

namespace Modules.Import.Model
{
    public class FileTagCollection : List<FileTag>
    {
        public FileTagCollection() { }

        public FileTagCollection(IEnumerable<FileTag> source)
            : base(source) { }
    }
}
