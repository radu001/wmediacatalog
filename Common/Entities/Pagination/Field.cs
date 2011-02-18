
namespace Common.Entities.Pagination
{
    public class Field : IField
    {
        public Field(string name, string displayName) :
            this(name, displayName, null, FieldTypeEnum.Text) { }

        public Field(string name, string displayName, string groupName) :
            this(name, displayName, groupName, FieldTypeEnum.Text) { }

        public Field(string name, string displayName, FieldTypeEnum fieldType) :
            this(name, displayName, null, fieldType) { }

        public Field(string name, string displayName, string groupName, FieldTypeEnum fieldType)
        {
            this.fieldName = name;
            this.displayName = displayName;
            this.groupName = groupName;
            this.fieldType = fieldType;
        }

        #region IField Members

        public string FieldName
        {
            get
            {
                return fieldName;
            }
        }

        public string FieldDisplayName
        {
            get
            {
                return displayName;
            }
        }

        public string GroupName
        {
            get
            {
                return groupName;
            }
        }

        public FieldTypeEnum FieldType
        {
            get
            {
                return fieldType;
            }
        }

        #endregion

        private string fieldName;
        private string displayName;
        private string groupName;
        private FieldTypeEnum fieldType;
    }
}
