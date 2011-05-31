
using Common.Data;
namespace BusinessObjects
{
    public class BusinessObject : NotificationObject
    {
        public const int NewEntityID = 0;

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                if (value != id)
                {
                    id = value;
                    NotifyPropertyChanged(() => ID);
                }
            }
        }

        public bool NeedValidate
        {
            get
            {
                return needValidate;
            }
            set
            {
                if (value != needValidate)
                {
                    needValidate = value;
                    NotifyPropertyChanged(() => NeedValidate);
                }
            }
        }

        public bool IsNew
        {
            get
            {
                return ID == NewEntityID;
            }
        }

        public object OptionalTag
        {
            get
            {
                return optionalTag;
            }
            set
            {
                optionalTag = value;
                NotifyPropertyChanged(() => OptionalTag);
            }
        }

        #region Private fields

        private int id;
        private bool needValidate;
        private object optionalTag;

        #endregion
    }
}
