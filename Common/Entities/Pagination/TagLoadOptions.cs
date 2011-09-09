using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Pagination
{
    public class TagLoadOptions : LoadOptions
    {
        /// <summary>
        /// Use *and* concatenation instead of *or* one for filtering entities associated with tags 
        /// </summary>
        public bool UseAndConcatenation { get; set; }

        public bool ExcludeAlbums { get; set; }

        public bool ExcludeArtists { get; set; }

        public bool DistinctByEntityID { get; set; }
    }
}
