using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules.Import.Models
{
    public class ArtistInfo
    {
        public string Name { get; set; }
        public List<AlbumInfo> Albums { get; set; }
    }
}
