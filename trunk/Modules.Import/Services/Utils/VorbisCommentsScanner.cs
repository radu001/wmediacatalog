
using System;
using Modules.Import.Model;
using SharpFlac;
namespace Modules.Import.Services.Utils
{
    public class VorbisCommentsScanner : IScanner
    {
        public VorbisCommentsScanner()
        {
            flacParser = new FlacParser();
        }

        #region IScanner Members

        public FileTagCollection GetTags(string filePath)
        {
            FileTagCollection result = new FileTagCollection();

            FlacParser parser = new FlacParser();
            var vorbisComments = parser.ReadVorbisComments(filePath);

            foreach (var c in vorbisComments)
            {
                result.Add(new FileTag()
                {
                    Key = c.Key,
                    Value = c.Value
                });
            }

            return result;
        }

        #endregion

        #region Private fields

        private FlacParser flacParser;

        #endregion
    }
}
