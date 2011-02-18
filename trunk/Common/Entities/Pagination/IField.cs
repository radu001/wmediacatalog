
namespace Common.Entities.Pagination
{
    public interface IField
    {
        string FieldName { get; }
        string FieldDisplayName { get; }
        string GroupName { get; }
        FieldTypeEnum FieldType { get; }
    }
}
