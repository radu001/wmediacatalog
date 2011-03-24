using System;

namespace Common.Dialogs.Helpers
{
    public class HeaderTextHelper
    {
        public static string CreateHeaderText(Type entityType, bool isEditMode)
        {
            DialogModeEnum mode = DialogModeEnum.Create;
            if (isEditMode)
                mode = DialogModeEnum.Edit;

            return CreateHeaderText(entityType, mode);
        }

        public static string CreateHeaderText(Type entityType, DialogModeEnum mode)
        {
            string typeName = entityType.Name.ToLower();

            switch (mode)
            {
                case DialogModeEnum.Create:
                    return String.Format("Create new {0}", typeName);
                case DialogModeEnum.Edit:
                    return String.Format("Edit {0}", typeName);
                case DialogModeEnum.View:
                    return String.Format("View {0}", typeName);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
