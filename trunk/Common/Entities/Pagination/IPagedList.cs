
using System.Collections.Generic;
namespace Common.Entities.Pagination
{
    public interface IPagedList<T> : IList<T>
    {
        int TotalItems { get; set; }
    }
}
