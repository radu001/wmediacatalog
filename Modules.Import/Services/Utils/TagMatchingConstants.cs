using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules.Import.Services.Utils
{
    public class TagMatchingConstants
    {
        public static readonly string ArtistTagKey = "ARTIST";
        public static readonly string AlbumTagKey = "ALBUM";
        public static readonly string GenreTagKey = "GENRE";
        public static readonly string[] YearTagKeys = new string[] { "YEAR", "DATE" };
    }
}
