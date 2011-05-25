using System;
using Common.Data;

namespace DbTool.Data
{
    public class ScriptTask : NotificationObject, IDeployTask
    {
        public string Script { get; set; }

        public ScriptTask(int index, string name, string description, string script)
        {
            Index = index;
            Name = name;
            Description = description;
            Script = script;
            Status = ItemStatus.Pending;
        }

        #region IDeployTask Members

        public int Index { get; private set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ItemStatus Status
        {
            get
            {
                return status;
            }
            private set
            {
                status = value;
                NotifyPropertyChanged(() => Status);
            }
        }

        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }
            private set
            {
                startTime = value;
                NotifyPropertyChanged(() => StartTime);
            }
        }

        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            private set
            {
                endTime = value;
                NotifyPropertyChanged(() => EndTime);
            }
        }

        public virtual bool Deploy()
        {
            StartTime = DateTime.Now;
            Status = ItemStatus.InProgress;
            EndTime = null;

            return true;
        }

        protected void FinishDeploy(bool success)
        {
            EndTime = DateTime.Now;
            Status = success ? ItemStatus.Success : ItemStatus.Error;
        }

        #endregion

        #region Private fields

        private ItemStatus status;
        private DateTime? startTime;
        private DateTime? endTime;

        #endregion
    }
}
