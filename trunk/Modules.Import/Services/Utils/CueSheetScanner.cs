using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules.Import.Model;

namespace Modules.Import.Services.Utils
{
    public class CueSheetScanner : IScanner
    {
        public FileTagCollection GetTags(string filePath)
        {
            return new FileTagCollection();
        }
    }
}
