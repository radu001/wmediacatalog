
namespace BusinessObjects.Base
{
    public class WasteableObject : BusinessObject
    {
        public bool IsWaste
        {
            get
            {
                return isWaste;
            }
            set
            {
                isWaste = value;
                NotifyPropertyChanged(() => IsWaste);
            }
        }

        private bool isWaste;
    }
}
