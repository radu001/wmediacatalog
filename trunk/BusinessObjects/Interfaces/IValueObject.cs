
namespace BusinessObjects.Interfaces
{
    public interface IValueObject<T>
    {
        T Clone();
    }
}
