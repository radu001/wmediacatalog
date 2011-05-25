using System;

namespace DbTool.Data
{
    public interface IDeployTask
    {
        int Index { get; }
        string Name { get; }
        string Description { get; }
        DateTime? StartTime { get; }
        DateTime? EndTime { get; }
        ItemStatus Status { get; }

        bool Deploy();
    }
}
