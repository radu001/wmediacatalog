using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Modules.Import.Model;
using CueSharp;
using Common;

namespace Modules.Import.Services.Utils.Scanners
{
    public class CueSheetScanner : IScanner
    {
        public FileTagCollection GetTags(string filePath)
        {
            try
            {
                var cueSheet = new CueSheet(filePath);
                return new FileTagCollection(GetTags(cueSheet));
            }
            catch (Exception ex)
            {
                Logger.Write(ex);
            }

            return new FileTagCollection();

            /*
             *         public static readonly string ArtistTagKey = "ARTIST";
        public static readonly string AlbumTagKey = "ALBUM";
        public static readonly string GenreTagKey = "GENRE";*/
        }

        private FileTag GetArtist(CueSheet cs)
        {
            if (!String.IsNullOrEmpty(cs.Performer))
                return new FileTag()
                {
                    Key = TagMatchingConstants.ArtistTagKey,
                    Value = cs.Performer
                };
            else
                return new FileTag()
                {
                    Key = TagMatchingConstants.ArtistTagKey,
                    Value = cs.Songwriter
                };
        }

        private FileTag GetAlbum(CueSheet cs)
        {
            return new FileTag()
            {
                Key = TagMatchingConstants.AlbumTagKey,
                Value = cs.Title
            };
        }

        private IEnumerable<FileTag> GetTags(CueSheet cueSheet)
        {
            var result = new List<FileTag>();

            result.Add(GetArtist(cueSheet));
            result.Add(GetAlbum(cueSheet));

            var comments = cueSheet.Comments;
            if (comments != null && comments.Length > 0)
            {
                //GENRE, DATE, DISCID
                for (int i = 0; i < comments.Length; ++i)
                {
                    var c = comments[i];
                    if (c.StartsWith("GENRE"))
                    {
                        string genre = c.Split(spaceSeparator)[1];
                        result.Add(new FileTag()
                        {
                            Key = TagMatchingConstants.GenreTagKey,
                            Value = genre
                        });
                    }
                    else if (c.StartsWith("DATE") || c.StartsWith("YEAR"))
                    {
                        string year = c.Split(spaceSeparator)[1];
                        result.Add(new FileTag()
                        {
                            Key = TagMatchingConstants.YearTagKeys[0],
                            Value = year
                        });
                    }
                }
            }

            return result;
        }

        private static readonly char[] spaceSeparator = new char[] { ' ' };
    }
}
