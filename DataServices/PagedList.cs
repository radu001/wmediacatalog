using System.Collections.Generic;
using Common.Entities.Pagination;

namespace DataServices
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        #region IPagedCollection<T> Members

        public int TotalItems
        {
            get
            {
                return totalItems;
            }
            set
            {
                totalItems = value;
            }
        }

        #endregion

        #region Private fields

        private int totalItems;

        #endregion
    }
}
