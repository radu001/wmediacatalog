
namespace BusinessObjects
{
    public class UserSettings : BusinessObject
    {
        public string Value1
        {
            get
            {
                return value1;
            }
            set
            {
                if (value != value1)
                {
                    value1 = value;
                    NotifyPropertyChanged(() => Value1);
                }
            }
        }

        #region Private fields

        private string value1;

        #endregion
    }
}
