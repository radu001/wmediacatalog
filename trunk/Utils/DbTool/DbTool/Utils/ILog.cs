using System;

namespace DbTool.Utils
{
    public interface ILog
    {
        void Write(Exception ex);
        string[] GetAllLog();
    }
}
