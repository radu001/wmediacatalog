using System.Collections.Generic;
using BusinessObjects;

namespace Modules.Import.Model.Comparers
{
    public class GenreByNameComparer : IEqualityComparer<Genre>
    {
        #region IEqualityComparer<Genre> Members

        public bool Equals(Genre x, Genre y)
        {
            return x.Name.ToUpper() == y.Name.ToUpper();
        }

        public int GetHashCode(Genre obj)
        {
            return obj.Name.ToUpper().GetHashCode();
        }

        #endregion
    }
}
