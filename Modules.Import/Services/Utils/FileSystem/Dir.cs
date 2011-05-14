
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Modules.Import.Services.Utils.FileSystem
{
    public class Dir
    {
        public string FullName { get; private set; }

        public Dir(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentException("Illegal null or empty fullName");

            FullName = fullName;
        }

        public virtual IEnumerable<Dir> GetSubDirectories()
        {
            return Directory.GetDirectories(FullName).Select<string, Dir>((s, d) => new Dir(s));
        }
    }
}
