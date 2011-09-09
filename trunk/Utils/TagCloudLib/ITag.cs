
namespace TagCloudLib
{
    public interface ITag
    {
        int ID { get; }
        string Name { get; }
        int Rank { get; }

        string Color { get; }
        string TextColor { get; }
    }
}
