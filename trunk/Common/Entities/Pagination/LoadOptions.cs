
namespace Common.Entities.Pagination
{
    public class LoadOptions : ILoadOptions
    {
        public int FirstResult { get; set; }
        public int MaxResults { get; set; }
        public IField FilterField { get; set; }
        public string FilterValue { get; set; }
    }
}
