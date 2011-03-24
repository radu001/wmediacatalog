using System;

namespace Common.Dialogs
{
    [Flags]
    public enum DialogButtons
    {
        NoButtons = 0x0,
        Ok = 0x1,
        Cancel = 0x2,
        All = Ok | Cancel
    }
}
