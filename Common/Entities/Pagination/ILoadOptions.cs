
namespace Common.Entities.Pagination
{
    public interface ILoadOptions
    {
        int FirstResult { get; set; }
        int MaxResults { get; set; }
        IField FilterField { get; set; }
        string FilterValue { get; set; }
        bool IncludeWaste { get; set; }
    }
}
