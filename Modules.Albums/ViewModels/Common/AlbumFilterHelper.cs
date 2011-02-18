using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.Pagination;

namespace Modules.Albums.ViewModels.Common
{
    public class AlbumFilterHelper
    {
        public IEnumerable<IField> InitializeFields()
        {
            IList<IField> fields = new List<IField>();
            fields.Add(new Field("Name", "Album name", "General filters"));
            fields.Add(new Field("Label", "Label", "General filters"));
            fields.Add(new Field("ASIN", "ASIN", "General filters"));
            fields.Add(new Field("FreedbID", "FreedbID", "General filters"));
            fields.Add(new Field("PrivateMarks", "Private notes", "General filters"));
            fields.Add(new Field("Description", "Description", "General filters"));

            fields.Add(new Field("Tag", "Tag", "Advanced filters"));
            fields.Add(new Field("Genre", "Genre", "Advanced filters"));
            fields.Add(new Field("Artist", "Artist", "Advanced filters"));

            return fields;
        }
    }
}
