using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules.Import.Model;
using System.Text.RegularExpressions;
using System.IO;

namespace Modules.Import.Services.Utils.Scanners
{
    public class CompositeScanner : IScanner
    {
        public CompositeScanner()
        {
            InitScannersChain();
        }

        public FileTagCollection GetTags(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();

            IScanner scanner = null;
            if (scannersMap.TryGetValue(extension, out scanner))
            {
                return scanner.GetTags(filePath);
            }

            return new FileTagCollection();
        }

        private void InitScannersChain()
        {
            scannersMap = new Dictionary<string, IScanner>();
            scannersMap[".flac"] = new VorbisCommentsScanner();
            scannersMap[".cue"] = new CueSheetScanner();
        }

        private IDictionary<string, IScanner> scannersMap;
    }
}
